using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;



public class PlayerWeapons : MonoBehaviour
{
    public Ranged rangeWeapon;
    public Melee meleeWeapon;
    PlayerInputs input;
    public Transform pointToStartAttack;
    Transform attackRot;

    bool canShoot = true;
    bool canSwing = true;

    [SerializeField] private LayerMask enemyLayerMask;

    [SerializeField] Transform Rotator;
    [SerializeField] Transform trailRendererObj;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] float SwingAnimDur = 0.5f;
    [SerializeField] float stabAnimDur = 0.25f;

    IEnumerator SwingCorutine;
    public IEnumerator ShootinCorutine;

    private void Awake()
    {
        input = new PlayerInputs();
        attackRot = pointToStartAttack.parent;
        Rotator.gameObject.SetActive(true);
        trailRenderer = trailRendererObj.GetComponent<TrailRenderer>();
        Rotator.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.RangedButton.performed += OnShoot;
        input.Player.RangedButton.canceled += OnShootEnd;

        input.Player.Meleebutton.performed += OnSwing;
        input.Player.Meleebutton.canceled += OnSwingCanceled;
    }

    private void OnDisable()
    {
        input.Player.RangedButton.performed -= OnShoot;
        input.Player.RangedButton.canceled -= OnShootEnd;

        input.Player.Meleebutton.performed -= OnSwing;
        input.Player.Meleebutton.canceled -= OnSwingCanceled;
        input.Disable();
    }

    private IEnumerator RangeCoolodwn(float seconds)
    {

        canShoot = false;
        yield return new WaitForSeconds(seconds);

        while (ShootinCorutine != null)
        {
            Debug.Log("aaaaaaaaaaaa");
            yield return null;
        }

        canShoot = true;

    }

    private IEnumerator MeleeCooldown(float seconds)
    {
        canSwing = false;
        yield return new WaitForSeconds(seconds);
        canSwing = true;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (ShootinCorutine != null)
            return;
        if (canShoot != true)
            return;

        PlayerComponent.onShoot?.Invoke();
        rangeWeapon.onRecharge += Recharge;
        rangeWeapon.onCorutine += RangedCorutine;
        rangeWeapon.Attack(transform);

        
        StartCoroutine(RangeCoolodwn(rangeWeapon.realoadTime));
    }

    void RangedCorutine(float time, Transform from)
    {
        ShootinCorutine = rangeWeapon.waitNextBullet(time, transform);
        StartCoroutine(ShootinCorutine);
    }

    private void OnShootEnd(InputAction.CallbackContext context)
    {
        rangeWeapon.onRecharge -= Recharge;
    }

    private void OnSwing(InputAction.CallbackContext context)
    {
       
        if (canSwing != true)
            return;
        meleeWeapon.onRecharge += Recharge;

        if (meleeWeapon.onLineAtk == null)
            meleeWeapon.onLineAtk += LineAtk;

        if (meleeWeapon.onConeAtk == null)
            meleeWeapon.onConeAtk += ConeAtk;

        if (meleeWeapon.onCircleAtk == null)
            meleeWeapon.onCircleAtk += CircleAtk;
        PlayerComponent.onSwing?.Invoke();

        //pointToStartAttack.gameObject.SetActive(true);
        meleeWeapon.Attack(pointToStartAttack);
        StartCoroutine(MeleeCooldown(meleeWeapon.realoadTime));
    }

    void LineAtk(float timer)
    {
        trailRendererObj.rotation = pointToStartAttack.rotation;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + trailRendererObj.right * meleeWeapon.range;
        trailRendererObj.position = startPos;
        trailRenderer.startWidth = meleeWeapon.thickness;

        Vector3 dir = pointToStartAttack.right;
        dir.z = 0;

        pointToStartAttack.position = pointToStartAttack.position + dir.normalized * (meleeWeapon.range / 2);

        Collider[] colliders = Physics.OverlapCapsule(startPos, endPos, meleeWeapon.thickness / 2, enemyLayerMask);



        StartCoroutine(Stab());
        foreach (Collider hitted in colliders)
        {
            if (hitted.transform.TryGetComponent(out IHp hp))
            {
                hp.TakeDmg(meleeWeapon.damage);
            }
        }

        IEnumerator Stab() 
        {
            Rotator.gameObject.SetActive(true);


            float stabTimer = 0;
            trailRendererObj.parent = null;
            Vector3 pos = new();

            while (stabTimer < stabAnimDur)
            {
                pos = Vector2.Lerp(startPos, endPos, stabTimer / stabAnimDur);
                trailRendererObj.position = pos;
                stabTimer += Time.deltaTime;
                yield return null;
            }

            Rotator.gameObject.SetActive(false);
        }
    }

    void ConeAtk(float speed)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, meleeWeapon.range /*/ 2*/, enemyLayerMask);
        foreach (Collider collider in colliders)
        {
            if (InsideCone(collider.transform))
                if (collider.transform.TryGetComponent(out IHp hp))
                {
                    hp.TakeDmg(meleeWeapon.damage);
                }
        }


        bool InsideCone(Transform enemy)
        {
            Vector3 dirToEnemy = enemy.position - transform.position;
            dirToEnemy.z = 0;
            dirToEnemy = dirToEnemy.normalized;

            Debug.Log(Vector2.Angle(attackRot.right, dirToEnemy) + " " + (meleeWeapon.angleOfAttack / 2));

            if (Vector2.Angle(attackRot.right, dirToEnemy) < meleeWeapon.angleOfAttack / 2)
            {
                Debug.Log("hello");
                float dist = Vector2.Distance(transform.position, enemy.position);
                Debug.Log(dist + " " + (meleeWeapon.range * 2 /*+ enemy.lossyScale.x*/));
                if (dist > meleeWeapon.range + 0.5f /*+ enemy.lossyScale.x*/)
                {
                    Debug.Log("false");
                    return false;
                }
                else
                {
                    Debug.Log("true");
                    return true;
                }
            }
            else
                return false;
        }

        StartCoroutine(Swing());
    }

    void CircleAtk(float speed)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, meleeWeapon.range / 2, enemyLayerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.TryGetComponent(out IHp hp))
            {
                hp.TakeDmg(meleeWeapon.damage);
            }
        }
        StartCoroutine(Swing());
    }


    IEnumerator Swing()
    {
        #region setting values:
        trailRendererObj.rotation = pointToStartAttack.rotation;
        trailRendererObj.position = transform.position + pointToStartAttack.right * meleeWeapon.range;

        trailRenderer.startWidth = trailRenderer.endWidth = meleeWeapon.range;

        Rotator.parent = null;

        Quaternion startRot = Rotator.rotation;

        float angle = meleeWeapon.angleOfAttack;

        if (meleeWeapon.isCircle)
        {
            angle = 360;
        }

        float animTimer = 0;

        Quaternion endRot = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion editedStartRot = Quaternion.Euler(startRot.eulerAngles - Vector3.forward * meleeWeapon.angleOfAttack / 2);
        #endregion

        #region start rotatating:

        Rotator.gameObject.SetActive(true);
        while (animTimer < SwingAnimDur)
        {
            Rotator.rotation = Quaternion.Slerp(editedStartRot, endRot, animTimer / SwingAnimDur);
            animTimer += Time.deltaTime;
            yield return null;
        }
        #endregion

        //yield return new WaitForSeconds(trailRenderer.time);
        #region return to normal:
        Rotator.gameObject.SetActive(false);
        trailRendererObj.position = pointToStartAttack.position;
        trailRenderer.startWidth = trailRenderer.endWidth = 1f;
        Rotator.rotation = startRot;
        Rotator.parent = transform;
        Rotator.localPosition = Vector3.zero;
        #endregion
    }


    private void OnSwingCanceled(InputAction.CallbackContext context)
    {
        //pointToStartAttack.gameObject.SetActive(false);
        meleeWeapon.onRecharge -= Recharge;
    }

    void Recharge(Bars bar)
    {
        if (bar.recharge != null)
        {
            Debug.Log("save me donald trump SAVE ME");
            StopRecharge(bar);
            bar.recharge = WaitForRecharge(bar);
            StartCoroutine(bar.recharge);
        }
        else
        {
            bar.recharge = WaitForRecharge(bar);
            StartCoroutine(bar.recharge);
        }
    }

    public IEnumerator WaitForRecharge(Bars bar)
    {
        yield return new WaitForSeconds(bar.waitAfterUse);

        while (bar.actualBar < bar.fullBar)
        {
            bar.actualBar += bar.rateRechargePerSeconds * Time.deltaTime;
            yield return null;
        }

        bar.actualBar = bar.fullBar;
        bar.recharge = null;
    }

    void StopRecharge(Bars bar)
    {
        StopCoroutine(bar.recharge);
        bar.recharge = null;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(trailRendererObj.position, 3);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(trailRendererObj.position, meleeWeapon.angleOfAttack);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, trailRendererObj.right);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, meleeWeapon.range);
        Vector3 halfSize = new Vector3(meleeWeapon.range / 2, meleeWeapon.thickness / 2, 0);
        Gizmos.DrawWireCube(pointToStartAttack.position, halfSize * 2);
        Gizmos.DrawLine(transform.position, transform.position + trailRendererObj.right * meleeWeapon.range);
    }

#endif
}
