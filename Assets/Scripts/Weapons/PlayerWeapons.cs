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

    private void Awake()
    {
        input = new PlayerInputs();
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

        rangeWeapon.onRecharge += Recharge;
        rangeWeapon.Attack(transform,this);
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
        //meleeWeapon.onRecharge += Recharge;
        sword.gameObject.SetActive(true);
        meleeWeapon.Attack(transform,this);
        StartCoroutine(MeleeCooldown(meleeWeapon.realoadTime));
    }

    private void OnSwingEnd(InputAction.CallbackContext context)
    {
        sword.gameObject.SetActive(false);
        //meleeWeapon.onRecharge -= Recharge;
    }

    void Recharge(Bars bar)
    {
        if(bar.recharge != null)
        {
            StopRecharge(bar);
            
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
       
        yield return new WaitForSeconds(bar.waitAfterUse);

        while (bar.actualBar < bar.fullBar)
        {
            bar.actualBar += bar.rateRechargePerSeconds * Time.deltaTime;
            yield return null;
        }
        bar.actualBar = bar.fullBar;

        //Debug.LogAssertion("Thank you stranger");
    }

    void StopRecharge(Bars bar)
    {
        StopCoroutine(bar.recharge);
    }
}
