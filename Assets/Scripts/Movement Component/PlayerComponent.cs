using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerComponent : MonoBehaviour
{
    private PlayerInputs input = null;

    private float moveValue;
    private float jumpValue;

    [HideInInspector]
    public bool isGrounded = true;
    [HideInInspector]
    public bool canJump = true;


    Rigidbody2D rb; // player's rigid body

    [Header("set the speed:")]
    public float playerSpeed;

    [Header("set jump hight:")]
    public float jumpForce;

    private void Awake()
    {
        input = new PlayerInputs();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnHorizontal;
        input.Player.Movement.canceled += OnHorizontalCancelled;
        input.Player.Jump.started += OnJump;
        input.Player.Jump.canceled += OnNotJumping;
    }



    private void OnDisable()
    {
        input.Player.Movement.performed -= OnHorizontal;
        input.Player.Movement.canceled -= OnHorizontalCancelled;
        input.Player.Jump.started -= OnJump;
        input.Player.Jump.canceled -= OnNotJumping;
        input.Disable();
    }

    private void Update()
    {
        if (input.Player.Jump.WasPressedThisFrame())
            canJump = true;
        else
            canJump = false;
    }

    private void FixedUpdate()
    {
        //Debug.Log(moveValue);

        Vector2 Vel = Vector3.zero;
        Vel.x = moveValue * playerSpeed * Time.fixedDeltaTime;

        rb.AddForce(Vel, ForceMode2D.Impulse);
    }

    private void OnHorizontal(InputAction.CallbackContext value)
    {
        moveValue = value.ReadValue<float>();
    }

    private void OnHorizontalCancelled(InputAction.CallbackContext value)
    {
        moveValue = 0;
    }

    void OnJump(InputAction.CallbackContext value)
    {
        //if (isGrounded)
        //{

        rb.AddForce(jumpForce * Time.fixedDeltaTime * Vector2.up, ForceMode2D.Impulse);

        isGrounded = false;
        //}
    }

    private void OnNotJumping(InputAction.CallbackContext context)
    {
        jumpValue = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Onlanded();
        }
    }

    void Onlanded()
    {
        isGrounded = true;
    }
}
