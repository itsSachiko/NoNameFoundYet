using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    //private void OnEnable()
    //{
    //    input.Player.RangedButton.performed += OnShoot;
    //    input.Player.RangedButton.canceled += OnShootEnd;
    //}

    //private void OnDisable()
    //{
    //    input.Player.RangedButton.performed -= OnShoot;
    //    input.Player.RangedButton.canceled -= OnShootEnd;
    //}

    private void Update()
    {
        if (input.Player.RangedButton.WasPerformedThisFrame() && canShoot)
        {
            rangeWeapon.Shoot(transform);
            StartCoroutine(RangeCoolodwn(rangeWeapon.realoadTime));
        }
        else if (input.Player.MeleeButton.WasPerformedThisFrame() && canSwing)
        {
            meleeWeapon.Swing(transform);
            StartCoroutine(MeleeCooldown(meleeWeapon.realoadTime));
        }
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
        rangeWeapon.Shoot(transform);
        StartCoroutine(RangeCoolodwn(rangeWeapon.realoadTime));
    }

    private void OnShootEnd(InputAction.CallbackContext context)
    {

    }

    private void OnSwing(InputAction.CallbackContext context)
    {
        sword.gameObject.SetActive(true);
        if (canSwing != true)
            return;

        meleeWeapon.Swing(transform);
        StartCoroutine(MeleeCooldown(meleeWeapon.realoadTime));
    }

    private void OnSwingEnd(InputAction.CallbackContext context)
    {
        sword.gameObject.SetActive(false);
    }
}
