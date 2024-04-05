using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Weapons : ScriptableObject
{
    [Header("Bar consumption: ")]
    public BarUsage[] allUsedBars;
    
    [Header("Settings: ")]
    public float realoadTime = 1f;
    public float damage = 1f;
    
    [Tooltip("the numbers of bullets/slashes it does")]
    public float numberOfAttacks = 1f;
    
    [Header("Aesthetic")]
    public Image weaponImg;

    public virtual void Attack(Transform point)
    {
        foreach (BarUsage barUsage in allUsedBars)
        {
            barUsage.Use();
            if (barUsage.bar.actualBar <= 0)
            {
                barUsage.bar.actualBar += barUsage.usagePerShot;
                return;
            }
        }
    }
}
