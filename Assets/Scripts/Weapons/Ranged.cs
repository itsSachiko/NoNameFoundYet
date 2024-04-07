using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "Bubbles", menuName = "Weapons/Ranged")]
public class Ranged : Weapons
{
    [Header("Ranged Settings:")]
    public Transform projectile;
    public float projectileSpeed;

    [Header("explosive shot")]
    public bool IsExplosive;
    public float rangeExplosion;
    public float damageExplosion;

    public event Recharge onRecharge;


    public override void Attack(Transform point)
    {
        //Debug.Log(name + " hello everybody", point);

        //Debug.LogWarning("sugma");
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

        Shoot(point);
    }

    public void Shoot(Transform from)
    {
        //Debug.Log(name + " owo ", from);
        GameObject x = Instantiate(projectile.gameObject, from.position, from.rotation);
        x.TryGetComponent(out Bullet bullet);
        BulletStats(bullet);
    }

    public void BulletStats(Bullet bullet)
    {
        bullet.iexplode = IsExplosive;
        bullet.expRange = rangeExplosion;
        bullet.expDamage = damageExplosion;
        bullet.dmg = damage;
        bullet.speed = projectileSpeed;
    }

    public void StopRecharge(BarUsage bar)
    {
        bar.bar.recharge = null;
    }
}
