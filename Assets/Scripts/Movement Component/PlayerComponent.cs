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

    [HideInInspector]
    public bool isGrounded = true;
    [HideInInspector]
    public bool canJump = true;

    float vel;

    float gravity;
    float mass;
    float torque;
    float orientSpeed;
    Transform planet;
    GRAVITY g;


    Rigidbody rb; // player's rigid body

    [Header("set the speed:")]
    public float playerSpeed;

    [Header("set jump hight:")]
    public float jumpForce;

    private void Awake()
    {
        input = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
        g = GetComponent<GRAVITY>();
        gravity = g.GRAVITATIONALPULL;
        mass = g.MASS;
        orientSpeed = g.OrientSpeed;
        planet = g.PLANET;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnHorizontal;
        input.Player.Movement.canceled += OnHorizontalCancelled;
        input.Player.Jump.started += OnJump;
    }



    private void OnDisable()
    {
        input.Player.Movement.performed -= OnHorizontal;
        input.Player.Movement.canceled -= OnHorizontalCancelled;
        input.Player.Jump.started -= OnJump;
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

        vel =  moveValue * playerSpeed * Time.fixedDeltaTime;

        rb.AddForce(transform.right*vel, ForceMode.VelocityChange);
        //rb.velocity = vel;

        CalculateGravity();
    }

    void CalculateGravity()
    {
        // Gravity is a harness. I have harnessed the harness

        Vector3 diff =transform.position - planet.position;
        rb.AddForce(gravity * mass * diff.normalized);
        Orient(-diff);

    }

    void Orient(Vector3 down)
    {
        Quaternion orientationDir =Quaternion.FromToRotation(transform.up,down.normalized) * transform.rotation;
        Vector3 rotEuler = orientationDir.eulerAngles;
        rotEuler.x = 0;
        rotEuler.y = 0;
        orientationDir = Quaternion.Euler(rotEuler);
        transform.rotation = Quaternion.Slerp(transform.rotation, orientationDir, Time.deltaTime * orientSpeed);
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
        if (isGrounded)
        {
            rb.AddForce(jumpForce * Time.fixedDeltaTime * transform.up, ForceMode.Impulse);

            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
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
