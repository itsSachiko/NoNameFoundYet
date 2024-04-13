using UnityEngine;

public class BulletsPuller : MonoBehaviour
{
    Transform bulletSpawn;
    public BulletPool bullets;

    private void Start()
    {
        for (int i = 0; i <= bullets.howManyToSpawn; i++)
        {
            bulletSpawn = Instantiate(bullets.bullet, transform.position, Quaternion.identity);
            bullets.Bullets.Add(bulletSpawn);
            bulletSpawn.gameObject.SetActive(false);
        }
    }

}
