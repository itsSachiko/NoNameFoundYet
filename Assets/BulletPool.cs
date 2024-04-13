using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "projectiles", menuName = "Weapons/Projectile")]
public class BulletPool : ScriptableObject
{
    public Transform bullet;
    public int howManyToSpawn;

    List<Transform> bullets = new();
    public List<Transform> Bullets { get => bullets; set => bullets = value; }

    private void OnDisable()
    {
        Bullets.Clear();
    }

    public void GetBullet(Transform newBullet)
    {
        newBullet.gameObject.SetActive(false);
        Bullets.Add(newBullet);
    }
}
