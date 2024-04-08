using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPuller : MonoBehaviour
{
    Transform bulletToSpawn;
    BulletPool bullets;

    private void Start()
    {
        for (int i = 0; i <= bullets.howManyToSpawn; i++)
        {
            Transform bulletToSpawn = Instantiate(bullets.bullet, transform.position, Quaternion.identity);
            bullets.Bullets.Add(bulletToSpawn);
            bulletToSpawn.gameObject.SetActive(false);

        }
    }

}
