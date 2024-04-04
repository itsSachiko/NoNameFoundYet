using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawner;

    [HideInInspector] public int waveCounter;

    [SerializeField] public Wave[] waves;

    [HideInInspector] public bool isChoosingWeapon;

    [HideInInspector] public List<GameObject> ranged = new List<GameObject>();
    [HideInInspector] public List<GameObject> melee = new List<GameObject>();
    [HideInInspector] public List<GameObject> mine = new List<GameObject>();

    private void Awake()
    {
        foreach (Wave wave in waves)
        {
            foreach (Enemy enemy in wave.enemies)
            {
                SpawnEnemy(enemy);
            }
        }
    }

    //private void Update()
    //{
    //    if (isChoosingWeapon)
    //    {
    //        return;
    //    }
    //}

    void SpawnEnemy(Enemy enemy)
    {
        int ran = Random.Range(0, spawner.Length);

        for (int i = 0; i < enemy.numberToSpawn; i++)
        {
             
            
            if (enemy.enemyPrefab.CompareTag("Ranged") && ranged.Count > 0)
            {
                ranged[0].SetActive(true);
                ranged.RemoveAt(0);
            }

            else if (enemy.enemyPrefab.CompareTag("Melee") && melee.Count > 0)
            {
                melee[0].SetActive(true);
                melee.RemoveAt(0);
            }

            else if (enemy.enemyPrefab.CompareTag("Mine") && mine.Count > 0)
            {
                mine[0].SetActive(true);
                mine.RemoveAt(0);
            }

            else
            {
                Instantiate(enemy.enemyPrefab, spawner[ran].position, Quaternion.identity);
            }

            //Transform spawnedEnemy = Instantiate(enemy.enemyPrefab, spawner[ran].position, Quaternion.identity);
        }
    }
}
