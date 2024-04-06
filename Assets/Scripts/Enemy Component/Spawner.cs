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

    [SerializeField] Transform playerPrefab;

    Transform latestEnemy;

    private void Awake()
    {
        foreach (Wave wave in waves)
        {
            foreach (Enemy enemy in wave.enemies)
            {
                SpawnEnemy(enemy);
            }
        }

        waveCounter = 0;

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
            
            latestEnemy = Instantiate(enemy.enemyPrefab, spawner[ran].position, Quaternion.identity, spawner[ran]);
            latestEnemy.gameObject.SetActive(false);

            //Transform spawnedEnemy = Instantiate(enemy.enemyPrefab, spawner[ran].position, Quaternion.identity);
        }
    }

    void EnemyReactivation(GameObject x)
    {
        int ran = Random.Range(0, spawner.Length);
        x.transform.position = spawner[ran].position;
        x.SetActive(false);
       
    }

    IEnumerator Wave(Wave wave)
    {

        foreach (Enemy y in wave.enemies)
        {
            if (y.enemyPrefab.CompareTag("Ranged"))
            {
                ranged[0].SetActive(true);
                ranged.RemoveAt(0);
            }

            else if (y.enemyPrefab.CompareTag("Melee"))
            {
                ranged[0].SetActive(true);
                ranged.RemoveAt(0);
            }

            else if (y.enemyPrefab.CompareTag("Melee"))
            {
                ranged[0].SetActive(true);
                ranged.RemoveAt(0);
            }

            yield return new WaitForSeconds(wave.waitNextSpawn);
        }

        yield return null;
    }
    public void AddRanged(GameObject enemyRanged)
    {
        ranged.Add(enemyRanged);
    }
    public void AddMelee(GameObject enemyMelee)
    {
        melee.Add(enemyMelee);
    }

    public void AddMine(GameObject enemyMine)
    {
        mine.Add(enemyMine);
    }
}
