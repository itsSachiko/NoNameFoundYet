using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Weapons : ScriptableObject
{
    [Header("Bar consumption: ")]
    public float magicUsage = 0.1f;
    public float staminaUsage = 0.1f;
    public float hpUsage = 0.1f;

    [Header("Settings: ")]
    public float realoadTime = 1f;
    public float damage = 1f;
    [Tooltip("the numbers of bullets/slashes it does")]
    public float numberOfAttacks = 1f;
    [Tooltip("if you can hold to attack")]
    public bool isHold;

}
