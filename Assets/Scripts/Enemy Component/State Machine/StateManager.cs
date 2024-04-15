using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class StateManager : MonoBehaviour, IHp
{
    public EnemyBaseState currentState; //reference dello stato attivo nella state machine 

    [Header("General Settings")]
    [SerializeField] public float speed;
    [SerializeField] public float damage;
    [SerializeField] public float hp;

    [Header("Player Prefab")]
    [SerializeField] public Transform playerPrefab;
    [SerializeField] private LayerMask playerLayer;

    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Rigidbody rb;

    [HideInInspector] public EnemyPull enemyPull;

    [Header("Ranged and Melee")]
    [SerializeField] public float AttackDistance;
    [SerializeField] public Weapons myWeapon;
    bool canShoot = true;
    [SerializeField] bool canAttackMelee = true;

    [Header("Charger")]
    [SerializeField, Range(1, 2000)] public float dashSpeed = 10f;
    [SerializeField] public float dashDuration;
    [SerializeField] public float dashCooldown;
    [SerializeField] public float timer;
    [HideInInspector] public bool isDashing = true;
    [SerializeField] public float dashDelay;

    [Header("Mine")]
    [SerializeField] public float waitTimeExplosion;

    public Animator anim;

    [Header("AttackIndicators")]
    [SerializeField] private Transform rotator;
    [SerializeField] private Transform trailRenderObj;
    private TrailRenderer trailRenderer;
    [SerializeField] private float stabAnimDuration = 0.2f;
    [SerializeField] private float swingAnimDuration = 0.2f;
    [SerializeField] public Transform rotatorToPlayer;

    [Header("Sprite")]
    [SerializeField] public SpriteRenderer mySpriteRenderer;

    [Header("Death")]
    [SerializeField] Transform particleSystem;

    IEnumerator shootCorutine;

    public float HP { get => hp; set => hp = value; }

    public void ChangeState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    private void OnEnable()
    {

        Ranged range = myWeapon as Ranged;
        if (range != null)
        {
            range.onCorutine += StartShootCorutine;
            range.CorutineNull += () => shootCorutine = null;
            range.getBulletDir += GiveBulletDir;
        }
    }

    private Vector3 GiveBulletDir()
    {
        return (playerPrefab.position - transform.position).normalized;
    }

    private void OnDisable()
    {
        Ranged range = myWeapon as Ranged;
        if (range != null)
        {
            range.onCorutine -= StartShootCorutine;
            range.CorutineNull = null;
            range.getBulletDir -= GiveBulletDir;
        }
        StopAllCoroutines();
    }

    private void StartShootCorutine(float seconds, Transform from)
    {
        Ranged rangedCast = myWeapon as Ranged;
        shootCorutine = rangedCast.waitNextBullet(seconds, from);
        StartCoroutine(shootCorutine);
    }


    void Start()
    {
        //anim = GetComponent<Animator>();
        currentState = new ChasingState();
        currentState.EnterState(this);
        rb = GetComponent<Rigidbody>();
        trailRenderer = trailRenderObj.GetComponent<TrailRenderer>();
        //mySpriteRenderer = GetComponent<SpriteRenderer>();
        playerPrefab = FindObjectOfType<PlayerHp>().transform;
    }



    void FixedUpdate()
    {
        currentState.UpdateState(this);

        if (transform.position.x < playerPrefab.position.x)
        {
            mySpriteRenderer.flipX = true;
        }

        else
        {
            mySpriteRenderer.flipX = false;
        }
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackDistance);

        Melee myMelee = myWeapon as Melee;
        if (myMelee == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, myMelee.range);
    }
#endif
    public void TakeDmg(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Death();
        }

    }

    public void HpUp(float Heal)
    {

    }

    public void Death()
    {
        if (enemyPull == null)
        {
            gameObject.SetActive(false);
            return;
        }
        enemyPull.pulledEnemies.Add(transform);

        Instantiate(particleSystem.gameObject, transform.position, Quaternion.identity);

        enemyPull.OnEnemyDeath?.Invoke();
        gameObject.SetActive(false);
    }

    public IEnumerator ShootCooldown(float seconds)
    {
        if (canShoot)
        {
            AudioManager.Instance.PlaySFX("attack ranged");
            canShoot = false;
            Ranged gun = myWeapon as Ranged;
            //aggiungi gun.oncoroutine += shootingCoroutine
            gun.Shoot(transform);
            yield return new WaitForSeconds(seconds);
            canShoot = true;
        }
        else
        {
            yield return null;
        }
    }

    public void StartMeleeAttack()
    {
        Debug.Log("DO YOU GET DEJAVU");
        StartCoroutine(MeleeCooldown(myWeapon.realoadTime));
    }
    public IEnumerator MeleeCooldown(float seconds)
    {
        Debug.Log("IM CRYING");
        if (canAttackMelee)
        {
            AudioManager.Instance.PlaySFX("attack melee");
            canAttackMelee = false;
            Melee melee = myWeapon as Melee;
            if (melee == null)
                StopCoroutine(nameof(MeleeCooldown));

            if (melee.isCircle)
            {
                melee.onCircleAtk += CircleAttack;
            }

            else if (melee.isCone)
            {
                melee.onConeAtk += ConeAttack;
            }
            else if (melee.IsLine)
            {
                Debug.Log("IM A LINE");
                melee.onLineAtk += LineAttack;
            }
            melee.Swing(transform);
            yield return new WaitForSeconds(seconds);
            canAttackMelee = true;
        }
        else
        {
            Debug.Log("noooOOOOOO");
            yield return null;
        }
    }

    private void LineAttack(float obj)
    {

        trailRenderObj.rotation = rotatorToPlayer.rotation;

        Melee meleeCasting = myWeapon as Melee;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + rotatorToPlayer.right * meleeCasting.range;
        trailRenderObj.position = startPos;
        trailRenderer.startWidth = meleeCasting.thickness;

        Vector3 right = rotatorToPlayer.right;
        right.z = 0f;
        Collider[] colliders = Physics.OverlapCapsule(startPos, endPos, meleeCasting.thickness / 2, playerLayer);
        StartCoroutine(Stab());

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IHp hp))
            {
                hp.TakeDmg(damage);
            }
        }

        IEnumerator Stab()
        {
            Debug.Log("SDFSGSGAS");
            float stabTimer = 0f;
            rotator.gameObject.SetActive(true);
            trailRenderObj.parent = null;
            Vector3 pos;
            while (stabTimer < stabAnimDuration)
            {
                pos = Vector2.Lerp(startPos, endPos, stabTimer / stabAnimDuration);

                trailRenderObj.position = pos;
                stabTimer += Time.deltaTime;
                yield return null;
            }

            rotator.gameObject.SetActive(false);
            trailRenderObj.parent = rotator;

        }
    }

    private void ConeAttack(float obj)
    {

        Melee meleeCasting = myWeapon as Melee;
        Collider[] colliders = Physics.OverlapSphere(transform.position, meleeCasting.range, playerLayer);

        StartCoroutine(SwingAnimation(meleeCasting.angleOfAttack, meleeCasting));
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IHp hp))
            {
                if (InsideCone(collider.transform))
                {
                    hp.TakeDmg(damage);
                }
            }

        }

        bool InsideCone(Transform enemy)
        {
            Vector3 dirToPlayer = playerPrefab.position - transform.position;
            dirToPlayer.z = 0f;
            dirToPlayer = dirToPlayer.normalized;

            if (Vector2.Angle(transform.right, dirToPlayer) < meleeCasting.angleOfAttack / 2)
            {
                float dist = Vector2.Distance(transform.position, playerPrefab.position);

                if (dist > meleeCasting.range)
                {
                    return false;
                }

                else
                {
                    return true;
                }
            }

            else
            {
                return false;
            }
        }
    }

    private void CircleAttack(float obj)
    {
        Melee meleeCasting = myWeapon as Melee;
        Collider[] colliders = Physics.OverlapSphere(transform.position, meleeCasting.range, playerLayer);
        StartCoroutine(SwingAnimation(360, meleeCasting));
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IHp hp))
            {
                hp.TakeDmg(damage);
            }

        }
    }

    public IEnumerator MineTimer(Melee weapon)
    {
        yield return new WaitForSeconds(waitTimeExplosion);


        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, weapon.range);
        foreach (Collider collider in enemiesHit)
        {
            if (collider.TryGetComponent(out IHp hp))
            {
                hp.TakeDmg(damage);

            }

        }
        TakeDmg(9999999);

    }

    IEnumerator SwingAnimation(float angle, Melee meleeCasting)
    {
        Transform Star = transform.GetChild(transform.childCount - 1);
        //trailRenderObj.rotation = rotatorToPlayer.rotation;
        trailRenderObj.position = transform.position + rotatorToPlayer.right * meleeCasting.range;
        trailRenderer.startWidth = meleeCasting.range;
        rotator.parent = null;
        Quaternion startRot = rotator.rotation;
        float animTimer = 0;
        Quaternion endRot = rotator.rotation * Quaternion.AngleAxis(-angle/2, Vector3.forward);
        Quaternion editedStartRot = rotator.rotation * Quaternion.AngleAxis(angle/2, Vector3.forward);
        //Debug.DrawRay(transform.position,, Color.magenta,3);
        //Debug.DrawRay(transform.position, , Color.magenta,3);
        rotator.gameObject.SetActive(true);
        Debug.DrawRay(transform.position, rotatorToPlayer.right*10,Color.blue,3);
        trailRenderer.emitting = true;
        while (animTimer < swingAnimDuration)
        {
            rotator.rotation = Quaternion.Slerp(editedStartRot, endRot, animTimer / swingAnimDuration);
            Star.rotation = Quaternion.Slerp(editedStartRot, endRot, animTimer / swingAnimDuration);
            animTimer += Time.deltaTime;
            yield return null;
        }

        trailRenderer.emitting = false;
        rotator.gameObject.SetActive(false);
        rotator.rotation = Quaternion.Euler(Vector3.zero); // startRot
        Star.rotation = Quaternion.Euler(Vector3.zero);
        rotator.parent = transform;
        rotator.localPosition = Vector3.zero;
    }

}
