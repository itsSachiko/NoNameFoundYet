using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pillow", menuName = "Weapons/Ranged")]
public class Ranged : Weapons
{
    [Header("Ranged Settings:")]
    public Transform projectile;
    public float projectileSpeed;

    [Header("explosive shot")]
    public bool IsExplosive;
    public float rangeExplosion;
    public float damageExplosion;

    [Header("Hitscan")]
    public bool isHitscan;
    [Tooltip("if left at 0 the ray will be infinite")]
    public float rangeHitscan;

    public void Shoot(Transform from)
    {
        GameObject x = Instantiate(projectile.gameObject,from.position,from.rotation);
        x.TryGetComponent(out Bullet bullet);
        bullet.iexplode = IsExplosive;
        bullet.expRange = rangeExplosion;
        bullet.expDamage = damageExplosion;
        bullet.dmg = damage;
        bullet.speed = projectileSpeed;
    }
}
