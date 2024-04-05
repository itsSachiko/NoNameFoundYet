using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BarUsage
{
    public Bars bar;
    public float usagePerShot = 1f;

    public void Use()
    {
        bar.actualBar -= usagePerShot;

    }

    public void StartRecharge(PlayerWeapons mono)
    {
        if (bar.canRecharge)
        {
            mono.StopCoroutine(bar.WaitForRecharge());
            mono.StartCoroutine(bar.WaitForRecharge());
        }
        else
        {
            mono.StartCoroutine(bar.WaitForRecharge());
        }
    }
}
