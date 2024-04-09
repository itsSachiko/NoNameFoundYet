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

    public event Recharge onRecharge;

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
        Shoot(point);
    }

    public void Shoot(Transform from)
    {
        //Debug.Log(name + " owo ", from);

        Transform chosenBullet;

        if (bulletPool.Bullets.Count > 0)
        {
            bulletPool.Bullets[0].position = from.position;
            bulletPool.Bullets[0].gameObject.SetActive(true);
            chosenBullet = bulletPool.Bullets[0];
            bulletPool.Bullets.RemoveAt(0);
        }
        else
        {
            chosenBullet = Instantiate(bulletPool.bullet, from.position, Quaternion.identity);
        }
        chosenBullet.TryGetComponent(out Bullet bullet);
        BulletStats(bullet,from);
    }

    public void BulletStats(Bullet bullet, Transform from)
    {
        bullet.parent = this;
        bullet.iexplode = IsExplosive;
        bullet.expRange = rangeExplosion;
        bullet.expDamage = damageExplosion;
        bullet.dmg = damage;
        bullet.speed = projectileSpeed;
        bullet.gunPoint = from;
        //bullet.dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - bullet.transform.position;
    }

    public void StopRecharge(BarUsage bar)
    {
        bar.bar.recharge = null;
    }
}
