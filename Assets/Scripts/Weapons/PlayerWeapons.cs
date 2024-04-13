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

    bool canShoot = true;
    bool canSwing = true;

    [SerializeField] private LayerMask enemyLayerMask;

    [SerializeField] Transform Rotator;
    [SerializeField] Transform trailRendererObj;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] float SwingAnimDur = 0.5f;
    [SerializeField] float stabAnimDur = 0.25f;

    IEnumerator SwingCorutine;

    private void Awake()
    {
        input = new PlayerInputs();
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
        if (canShoot != true)
            return;

        rangeWeapon.onRecharge += Recharge;
        rangeWeapon.Attack(transform);
        StartCoroutine(RangeCoolodwn(rangeWeapon.realoadTime));
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

        //pointToStartAttack.gameObject.SetActive(true);
        meleeWeapon.Attack(pointToStartAttack);
        StartCoroutine(MeleeCooldown(meleeWeapon.realoadTime));
    }

    void LineAtk(float timer)
    {
        Vector3 dir = pointToStartAttack.forward;
        dir.z = 0;

        Vector3 halfSize = new Vector3(meleeWeapon.range / 2, meleeWeapon.thickness / 2, 0);

        pointToStartAttack.position = pointToStartAttack.position + dir.normalized * (meleeWeapon.range / 2);
        pointToStartAttack.localScale = halfSize;
        Collider[] colliders = Physics.OverlapBox(pointToStartAttack.position, halfSize);
        Image image = pointToStartAttack.GetComponent<Image>();
        image.sprite = meleeWeapon.lineAttackImg;
        pointToStartAttack.gameObject.SetActive(true);

        foreach (Collider hitted in colliders)
        {
            if (hitted.transform.TryGetComponent(out IHp hp))
            {
                hp.TakeDmg(meleeWeapon.damage);
            }
        }
        StartCoroutine(Stab());

        IEnumerator Stab()
        {
            trailRendererObj.position = pointToStartAttack.position;
            float stabTimer = 0;
            trailRendererObj.parent = null;

            while (stabTimer < stabAnimDur)
            {

                yield return null;
            }
        }
    }



    void ConeAtk(float speed)
    {
        Vector3 dir = pointToStartAttack.forward;

        Vector3 halfSize = new Vector3(meleeWeapon.range / 2, meleeWeapon.thickness / 2, 0);

        Collider[] colliders = Physics.OverlapSphere(transform.position, halfSize.x, enemyLayerMask);
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

            if (Vector3.Angle(transform.right, dirToEnemy) < meleeWeapon.angleOfAttack / 2)
            {
                float dist = Vector3.Distance(transform.position, enemy.position);
                if (dist > meleeWeapon.range)
                {
                    return false;
                }
                else
                    return true;
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
        #endregion

        #region start rotatating:

        Rotator.gameObject.SetActive(true);
        while (animTimer < SwingAnimDur)
        {
            Rotator.rotation = Quaternion.Slerp(startRot, endRot, animTimer / SwingAnimDur);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(trailRendererObj.position, 3);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(trailRendererObj.position, meleeWeapon.angleOfAttack);
    }
}
