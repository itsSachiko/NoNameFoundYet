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

    public Bars allTheBars;

    public Transform sword;

    bool canShoot = true;
    bool canSwing = true;

    private void Awake()
    {
        input = new PlayerInputs();
    }

    private void OnEnable()
    {
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
        rangeWeapon.Attack(transform);
        StartCoroutine(RangeCoolodwn(rangeWeapon.realoadTime));
    }

    private void OnShootEnd(InputAction.CallbackContext context)
    {

    }

    private void OnSwing(InputAction.CallbackContext context)
    {
        if (canSwing != true)
            return;

        sword.gameObject.SetActive(true);
        meleeWeapon.Attack(transform);
        StartCoroutine(MeleeCooldown(meleeWeapon.realoadTime));
    }

    private void OnSwingEnd(InputAction.CallbackContext context)
    {
        sword.gameObject.SetActive(false);
    }
}
