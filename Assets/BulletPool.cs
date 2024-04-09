using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "projectiles", menuName = "Weapons/Projectile")]
public class BulletPool : ScriptableObject
{
    public Transform bullet;
    public int howManyToSpawn;

    List<Transform> bullets;
    public List<Transform> Bullets { get { return bullets; } set { bullets = value; } }

    private void OnDisable()
    {
        bullets.Clear();
        bullets = new List<Transform>();
    }

    public void GetBullet(Transform newBullet)
    {
        bullets.Insert(0, newBullet);
    }
}
