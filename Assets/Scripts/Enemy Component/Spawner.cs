using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawner;

    [HideInInspector] public int waveCounter;

    [SerializeField] public Wave[] waves;

    [HideInInspector] public bool isChoosingWeapon;

    [SerializeField] Transform playerPrefab;

    Transform latestEnemy;

    int ran;

    public static Action onLastWave;

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

        for (int i = 0; i < enemy.numberToSpawn; i++)
        {

            latestEnemy = Instantiate(enemy.enemyPrefab, spawner[ran].position, Quaternion.identity, spawner[ran]);
            latestEnemy.gameObject.SetActive(false);
            EnemyVariableSet(latestEnemy, enemy);

            //Transform spawnedEnemy = Instantiate(enemy.enemyPrefab, spawner[ran].position, Quaternion.identity);
        }
    }
    void GetEnemy(EnemyPull enemyPull)
    {
        ran = Random.Range(0, spawner.Length);
        enemyPull.pulledEnemies[0].position = spawner[ran].position;
        enemyPull.pulledEnemies[0].gameObject.SetActive(true);
    }

    IEnumerator Wave(Wave currentWave)
    {
        waveCounter++;
        foreach (Enemy enemy in currentWave.enemies)
        {
            GetEnemy(enemy.enemyPull);
            yield return new WaitForSeconds(currentWave.waitNextSpawn);
        }
        if (currentWave.isLast)
        {
            onLastWave?.Invoke();
            while (isChoosingWeapon)
            {
                yield return null;
            }

        }
        yield return new WaitForSeconds(currentWave.waitNextWave);
        
        if (waveCounter < waves.Length)
        {

        }
    }
    void EnemyReactivation(GameObject x)
    {
        int ran = Random.Range(0, spawner.Length);
        x.transform.position = spawner[ran].position;
        x.SetActive(false);

    }

    void EnemyVariableSet(Transform enemy, Enemy stats)
    {
        latestEnemy.TryGetComponent(out StateManager state);
        state.enemyPull = stats.enemyPull;
    }

    void OnWin()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(0);
    }
}
