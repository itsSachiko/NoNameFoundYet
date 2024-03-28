using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerComponent : MonoBehaviour
{
    private PlayerInputs input= null;
    private float moveValue;

    [SerializeField] public float playerSpeed;

    private void Awake()
    {
        input = new PlayerInputs();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.performed += OnMovementCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.performed -= OnMovementCancelled;
    }

    private void FixedUpdate()
    {
        Debug.Log(moveValue);
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveValue = value.ReadValue<float>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveValue = 0;
    }
}
