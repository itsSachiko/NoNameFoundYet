using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using System;

public class InputComponent : MonoBehaviour
{
    private PlayerInputs input = null;

    public delegate void panelOnOff();
    public static event panelOnOff onOff;

    public static int onOffCounter = 0;
    private void Awake()
    {
        input = new PlayerInputs();  
    }

    private void Start()
    {
        onOffCounter = onOff.GetInvocationList().Length;
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.PauseButton.performed += OnEscapeButton;
    }

    private void OnDisable()
    {
        input.Player.PauseButton.performed -= OnEscapeButton;
    }
    private void OnEscapeButton(InputAction.CallbackContext context)
    {
        if (onOff.GetInvocationList().Length > 1 || onOff == null)
        {
            Debug.Log(onOff.GetInvocationList().Length);
            return;
        }

        onOff();
    }

    public void UpdateOnOffCounter()
    {
        onOffCounter = onOff.GetInvocationList().Length;
    }

}
