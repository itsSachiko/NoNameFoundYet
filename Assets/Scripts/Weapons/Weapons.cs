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

    public delegate void Recharge(Bars bar);


    public virtual void Attack(Transform point)
    {
    }
}
