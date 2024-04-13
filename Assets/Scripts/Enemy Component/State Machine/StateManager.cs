using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour, IHp
{
    public EnemyBaseState currentState; //reference dello stato attivo nella state machine 

    [Header("General Settings")]
    [SerializeField, Range(1, 500)] public float speed;
    [SerializeField] public float damage;
    [SerializeField] public float hp;

    [Header("Player Prefab")]
    [SerializeField] public Transform playerPrefab;

    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Rigidbody rb;

    [HideInInspector] public EnemyPull enemyPull;

    [Header("Ranged and Melee")]
    [SerializeField, Range(1, 15)] public float AttackDistance;
    [SerializeField] public Weapons myWeapon;
    bool canShoot = true;
    bool canAttackMelee = true;

    [Header("Charger")]
    [SerializeField, Range(1, 500)] public float dashSpeed = 10f;
    [SerializeField] public float dashDuration;
    [SerializeField] public float dashCooldown;
    [SerializeField] public float timer;
    [HideInInspector] public bool isDashing = true;
    [SerializeField] public float dashDelay;

    [Header("Mine")]
    [SerializeField] public float waitTimeExplosion;

    [HideInInspector] public Animator anim;


    public float HP { get; set; }

    public void ChangeState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        currentState = new ChasingState();
        currentState.EnterState(this);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        currentState.UpdateState(this);
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackDistance);

        Melee myMelee = (Melee)myWeapon;
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
        enemyPull.OnEnemyDeath?.Invoke();
        gameObject.SetActive(false);
    }

    public IEnumerator ShootCooldown(float seconds)
    {
        if (canShoot)
        {
            canShoot = false;
            Ranged gun = (Ranged)myWeapon;
            gun.Shoot(transform);
            yield return new WaitForSeconds(seconds);
            canShoot = true;
        }
        else
        {
            yield return null;
        }

    }

    public IEnumerator MeleeCooldown(float seconds)
    {
        if (canAttackMelee)
        {
            canAttackMelee = false;
            Melee melee = (Melee)myWeapon;
            melee.Swing(transform);
            yield return new WaitForSeconds(seconds);
            canAttackMelee = true;
        }
        else
        {
            yield return null;
        }
    }

    public IEnumerator MineTimer(Melee weapon)
    {
        yield return new WaitForSeconds(waitTimeExplosion);

        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, weapon.range);
        Debug.Log(enemiesHit.Length);
        foreach (Collider collider in enemiesHit)
        {
            Debug.Log(collider.gameObject.name);
            if (collider.TryGetComponent(out IHp hp))
            {
                hp.TakeDmg(damage);

            }

        }
        TakeDmg(9999999);

    }

    private void OnEnable()
    {
        HP = hp;
    }
}
