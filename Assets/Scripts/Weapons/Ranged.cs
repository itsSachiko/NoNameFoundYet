using System.Collections;
using System.Collections.Generic;
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


    public override void Attack(Transform point)
    {
        base.Attack(point);
        Shoot(point);
    }

    public void Shoot(Transform from)
    {
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
}
