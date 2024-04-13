using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Bubbles", menuName = "Weapons/Ranged")]
public class Ranged : Weapons
{
    [Header("Ranged Settings:")]
    public BulletPool bulletPool;
    public float projectileSpeed;

    [Header("explosive shot")]
    public bool IsExplosive;
    public float rangeExplosion;
    public float damageExplosion;

    int bulletCounter;

    public event Recharge onRecharge;

    PlayerWeapons playerWeapon;
    Transform chosenBullet;
    public delegate void startWNB(float seconds, Transform from);
    public event startWNB onCorutine;

    private void OnEnable()
    {
        playerWeapon = null;
    }

    public override void Attack(Transform point)
    {
        foreach (BarUsage barUsage in allUsedBars)
        {
            barUsage.Use();

            if (barUsage.bar.actualBar <= 0)
            {
                barUsage.NoAmmo();
                return;
            }
            onRecharge(barUsage.bar);
        }
        playerWeapon = point.GetComponent<PlayerWeapons>();
        Shoot(point);
    }

    public void Shoot(Transform from)
    {
        onCorutine?.Invoke(0.2f,from);
    }
    void GetBullet(Transform from, out Transform choosenBullet)
    {
        choosenBullet = null;

        if (bulletPool.Bullets.Count > 0)
        {
            Debug.Log("i got the bullet");
            bulletPool.Bullets[0].position = from.position;
            bulletPool.Bullets[0].gameObject.SetActive(true);
            choosenBullet = bulletPool.Bullets[0];
            bulletPool.Bullets.RemoveAt(0);
        }
        else
        {
            Debug.Log("it ain't got no bullets in it");
            chosenBullet = Instantiate(bulletPool.bullet, from.position, Quaternion.identity);
        }

    }
    public IEnumerator waitNextBullet(float seconds, Transform from)
    {
        Debug.Log("is this thing on?");
        for (int i = 0; i < numberOfAttacks; i++)
        {
            GetBullet(from, out chosenBullet);
            BulletStats(chosenBullet.GetComponent<Bullet>(), from);

            yield return new WaitForSeconds(seconds);
        }
        if (playerWeapon)
        {
            playerWeapon.ShootinCorutine = null;
        }
    }

    public void BulletStats(Bullet bullet, Transform from)
    {
        Debug.LogWarning("uuuuAAAAAAAAaaaAaaAaAaAaAaAaAaAaAaAaAa");
        bullet.parent = this;
        bullet.iexplode = IsExplosive;
        bullet.expRange = rangeExplosion;
        bullet.expDamage = damageExplosion;
        bullet.dmg = damage;
        bullet.speed = projectileSpeed;
        bullet.gunPoint = from;
        bullet.dir = playerWeapon.pointToStartAttack.right;
        //bullet.dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - bullet.transform.position;
    }

    public void StopRecharge(BarUsage bar)
    {
        bar.bar.recharge = null;
    }
}
