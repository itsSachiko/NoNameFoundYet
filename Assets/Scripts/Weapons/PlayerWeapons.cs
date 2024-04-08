using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerWeapons : MonoBehaviour
{
    public Ranged rangeWeapon;
    public Melee meleeWeapon;
    PlayerInputs input;

    public Transform sword;

    bool canShoot = true;
    bool canSwing = true;

    Vector3 mousePos;
    Camera mainCam;


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

    private void Update()
    {


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
        meleeWeapon.onLineAtk += LineAtk;
        meleeWeapon.onConeAtk += ConeAtk;
        meleeWeapon.onCircleAtk += CircleAtk;
        //sword.gameObject.SetActive(true);
        meleeWeapon.Attack(transform);
        StartCoroutine(MeleeCooldown(meleeWeapon.realoadTime));
    }

    void LineAtk(float timer)
    {
        RotateToMouse(sword, out Vector3 dir);

        sword.gameObject.SetActive(true);
        RaycastHit[] hits = Physics.SphereCastAll(sword.position, meleeWeapon.thickness, dir, meleeWeapon.range);
        foreach (RaycastHit hitted in hits)
        {
            if(hitted.transform.TryGetComponent(out IHp hp))
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
        RotateToMouse(sword,out Vector3 dir);
        StartCoroutine(Swing(speed));
    }

    IEnumerator Swing(float duration)
    {
        sword.gameObject.SetActive(true);
        float timer = 0;

        float angle = meleeWeapon.angleOfAttack;
        if (meleeWeapon.isCircle)
            angle = 360f;
        mousePos = mainCam.ScreenToViewportPoint(UnityEngine.Input.mousePosition);

        Vector3 dir = mousePos - transform.position;
        dir = dir.normalized;

        float zRot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(Vector3.forward * zRot);
        Quaternion startRot = transform.localRotation;
        Quaternion endRot = Quaternion.AngleAxis(angle, sword.forward) * sword.localRotation;
        while (timer < duration)
        {
            transform.localRotation = Quaternion.Slerp(startRot, endRot, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = endRot;
        sword.gameObject.SetActive(false);
        transform.localRotation = startRot;
    }

    void CircleAtk(float speed)
    {

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
            Debug.Log("save me donald trump");
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
        //Debug.LogAssertion("heeeeeelp help me");
        Debug.Log("sucamajor");
        yield return new WaitForSeconds(bar.waitAfterUse);
        Debug.Log("T^T");
        while (bar.actualBar < bar.fullBar)
        {
            Debug.Log("HAHAHAHAHAHA");
            bar.actualBar += bar.rateRechargePerSeconds * Time.deltaTime;
            yield return null;
        }

        bar.actualBar = bar.fullBar;
        bar.recharge = null;
        Debug.Log(bar.actualBar);

        //Debug.LogAssertion("Thank you stranger");
    }

    void StopRecharge(Bars bar)
    {
        StopCoroutine(bar.recharge);
        bar.recharge = null;
    }
}
