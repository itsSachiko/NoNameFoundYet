using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawner;

    [HideInInspector] public int waveCounter;

    [SerializeField] public Wave[] waves;
    [SerializeField] Enemy[] enemiesToSpawnAtStart;

    [HideInInspector] public bool isChoosingWeapon;

    Transform latestEnemy;

    [SerializeField] Transform player;

    int ran;

    int numberOfEnemiesActive = 0;

    public static Action onLastWave;

    public static Action onWin;

    public Action onStarActivate;

    private void Awake()
    {
        foreach (Enemy enemy in enemiesToSpawnAtStart)
        {
            SpawnEnemy(enemy);
        }
        WaveBar.CalculateWaveTime?.Invoke(waves[waveCounter]);
        StartCoroutine(Wave(waves[waveCounter]));
        waveCounter = 0;

    }

    private void Start()
    {
        
    }

    void SpawnEnemy(Enemy enemy, bool NeedsToActivate = false)
    {
        for (int i = 0; i <= enemy.numberToSpawn; i++)
        {
            ran = Random.Range(0, spawner.Length);

            latestEnemy = Instantiate(enemy.enemyPrefab, spawner[ran].position, Quaternion.identity, spawner[ran]);

            EnemyVariableSet(latestEnemy, enemy);
            enemy.enemyPull.OnEnemyDeath += EnemyDied;

            if (NeedsToActivate)
                latestEnemy.gameObject.SetActive(true);
            else
            {

                enemy.enemyPull.pulledEnemies.Add(latestEnemy);

                latestEnemy.gameObject.SetActive(false);
            }


            //Transform spawnedEnemy = Instantiate(enemy.enemyPrefab, spawner[ran].position, Quaternion.identity);
        }
    }

    private void EnemyDied()
    {
        numberOfEnemiesActive--;
    }

    void GetEnemy(EnemyPull enemyPull, Enemy enemy)
    {
        //AudioManager.Instance.PlaySFX("spawn enemy");
        if (enemyPull.pulledEnemies.Count <= 0)
        {
            SpawnEnemy(enemy, true);
        }

        ran = Random.Range(0, spawner.Length);
        enemyPull.pulledEnemies[0].position = spawner[ran].position;
        enemyPull.pulledEnemies[0].gameObject.SetActive(true);
        numberOfEnemiesActive++;
        enemyPull.pulledEnemies.RemoveAt(0);
    }

    IEnumerator Wave(Wave currentWave)
    {
        waveCounter++;
        foreach (Enemy enemy in currentWave.enemies)
        {
            for (int i = 0; i <= enemy.numberToSpawn; i++)
            {
                GetEnemy(enemy.enemyPull, enemy);
                yield return new WaitForSeconds(currentWave.waitNextSpawn);

            }
        }
        if (currentWave.isLast)
        {
            while (numberOfEnemiesActive > 0)
            {
                yield return null;
            }

            //onLastWave?.Invoke();
            while (isChoosingWeapon)
            {
                yield return null;
            }

        }
        yield return new WaitForSeconds(currentWave.waitNextWave);

        if (waveCounter < waves.Length)
        {
            WaveBar.CalculateWaveTime(waves[waveCounter]);
            StartCoroutine(Wave(waves[waveCounter]));
        }

        else
        {
            OnWin();
        }
    }

    void EnemyVariableSet(Transform enemy, Enemy stats)
    {
        latestEnemy.TryGetComponent(out StateManager state);
        state.enemyPull = stats.enemyPull;
        state.playerPrefab = player;

    }

    public void OnWin()
    {
        StopAllCoroutines();
        onWin?.Invoke();
    }
}
