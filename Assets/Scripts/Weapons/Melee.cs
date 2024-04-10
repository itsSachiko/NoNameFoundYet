using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Pillow", menuName = "Weapons/Melee")]
public class Melee : Weapons
{
    [Header("Aesthetic: ")]
    public Sprite lineAttackImg;
    public Sprite coneAttackImg;
    public Sprite circleAttackImg;

    [Header("for All:")]
    public float range = 10f;
    [Tooltip("for Line = time it stays there\n" +
        "for Cone = how fast it goes from one end to the other\n" +
        "for circle = how fast it rotates\n")]
    public float swingSpeed = 1;
    public float thickness = 1;

    [Header("Melee Line settings: ")]
    [Tooltip("if the attack is only horizontal twoards the pointer")]
    public bool IsLine;
    [Tooltip("how long it takes to swing your sword")]
    public float durationOfAttack = 1f;
    public float rangeOfLine = 10f;

    [Header("Melee Cone: ")]
    [Tooltip("if the is the one with an angle")]
    public bool isCone;
    public float angleOfAttack = 45;

    [Header("Melee Circle: ")]
    [Tooltip("if the is the one that rotates")]
    public bool isCircle;
    public int numberOfSpiins = 1;


    public event Recharge onRecharge;

    public delegate void ActivateAttack(float speed);
    public Action<float> onLineAtk;
    public Action<float> onConeAtk;
    public Action<float> onCircleAtk;



    public override void Attack(Transform point)
    {
        foreach (BarUsage barUsage in allUsedBars)
        {
            barUsage.Use();

            if (barUsage.bar.actualBar <= 0)
            {
                //Debug.LogWarning("DICK");
                barUsage.NoAmmo();
                return;
            }
            //barUsage.StartRecharge(x);
            onRecharge(barUsage.bar);
        }
        Swing(point);
    }

    public void Swing(Transform sword)
    {

        if (isCone)
        {
            onLineAtk(swingSpeed);
        }

        if (IsLine)
        {
            onConeAtk(swingSpeed);

        }

        if (isCircle)
        {
            onCircleAtk(swingSpeed);
        }
    }
}
