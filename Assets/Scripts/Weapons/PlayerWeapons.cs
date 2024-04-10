using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;


public class PlayerWeapons : MonoBehaviour
{
    public Ranged rangeWeapon;
    public Melee meleeWeapon;
    PlayerInputs input;

    public Transform sword;
    public Transform Gun;

    bool canShoot = true;
    bool canSwing = true;

    Vector3 mousePos;
    Camera mainCam;
    [SerializeField] private LayerMask enemyLayerMask;

    private void Awake()
    {
        input = new PlayerInputs();
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.RangedButton.performed += OnShoot;
        input.Player.RangedButton.canceled += OnShootEnd;

        input.Player.Meleebutton.performed += OnSwing;
        input.Player.Meleebutton.canceled += OnSwingEnd;
    }

    private void OnDisable()
    {
        input.Player.RangedButton.performed -= OnShoot;
        input.Player.RangedButton.canceled -= OnShootEnd;

        input.Player.Meleebutton.performed -= OnSwing;
        input.Player.Meleebutton.canceled -= OnSwingEnd;
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
        MoveGun();
        rangeWeapon.onRecharge += Recharge;
        rangeWeapon.Attack(transform);
        StartCoroutine(RangeCoolodwn(rangeWeapon.realoadTime));
    }

    void MoveGun()
    {
        Vector3 pos = Gun.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - pos;
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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

        //sword.gameObject.SetActive(true);
        meleeWeapon.Attack(sword);
        StartCoroutine(MeleeCooldown(meleeWeapon.realoadTime));
    }

    void LineAtk(float timer)
    {
        RotateToMouse(sword, out Vector3 dir);
        dir.z = 0;

        Vector3 halfSize = new Vector3(meleeWeapon.range / 2, meleeWeapon.thickness / 2, 0);

        sword.position = Gun.position + dir.normalized * (meleeWeapon.range / 2);
        sword.localScale = halfSize;
        Collider[] colliders = Physics.OverlapBox(sword.position, halfSize);
        Image image = sword.GetComponent<Image>();
        image.sprite = meleeWeapon.lineAttackImg;
        sword.gameObject.SetActive(true);
        foreach (Collider hitted in colliders)
        {
            if (hitted.transform.TryGetComponent(out IHp hp))
            {
                hp.TakeDmg(meleeWeapon.damage);
            }
        }
    }

    void RotateToMouse(Transform toRotate, out Vector3 dir)
    {
        mousePos = mainCam.ScreenToViewportPoint(UnityEngine.Input.mousePosition);

        dir = mousePos - transform.position;
        dir = dir.normalized;

        float zRot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        toRotate.localRotation = Quaternion.Euler(Vector3.forward * zRot);
    }

    void ConeAtk(float speed)
    {
        RotateToMouse(sword, out Vector3 dir);

        Vector3 halfSize = new Vector3(meleeWeapon.range / 2, meleeWeapon.thickness / 2, 0);

        sword.position = Gun.position + dir.normalized * halfSize.x;
        sword.localScale = halfSize;
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
            dirToEnemy = dirToEnemy.normalized;

            if (Vector3.Angle(transform.forward, dirToEnemy) < meleeWeapon.angleOfAttack / 2)
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

        //StartCoroutine(Swing(speed));
    }

    //IEnumerator Swing(float duration)
    //{
    //    sword.gameObject.SetActive(true);
    //    float timer = 0;

    //    float angle = meleeWeapon.angleOfAttack;
    //    if (meleeWeapon.isCircle)
    //        angle = 360f;
    //    mousePos = mainCam.ScreenToViewportPoint(UnityEngine.Input.mousePosition);

    //    Vector3 dir = mousePos - transform.position;
    //    dir = dir.normalized;

    //    float zRot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //    transform.localRotation = Quaternion.Euler(Vector3.forward * zRot);
    //    Quaternion startRot = transform.localRotation;
    //    Quaternion endRot = Quaternion.AngleAxis(angle, sword.forward) * sword.localRotation;
    //    while (timer < duration)
    //    {
    //        transform.localRotation = Quaternion.Slerp(startRot, endRot, timer / duration);
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //    transform.localRotation = endRot;
    //    sword.gameObject.SetActive(false);
    //    transform.localRotation = startRot;
    //}

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
    }

    void SwingAttackEnd()
    {
        sword.gameObject.SetActive(false);
    }


    private void OnSwingEnd(InputAction.CallbackContext context)
    {
        //sword.gameObject.SetActive(false);
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
}
