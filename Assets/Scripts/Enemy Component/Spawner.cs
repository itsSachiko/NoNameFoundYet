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

    [HideInInspector] public bool isChoosingWeapon;

    Transform latestEnemy;

    [SerializeField] Transform player;

    int ran;

    int numberOfEnemiesActive = 0;

    public static Action onLastWave;

    public static Action onWin;

    public Action onStarActivate;
    public Action onChoosedWeapon;

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

        onChoosedWeapon += () => isChoosingWeapon = false; 

    }

    private void Start()
    {
        StartCoroutine(Wave(waves[waveCounter]));
    }

    void SpawnEnemy(Enemy enemy)
    {

        for (int i = 0; i <= enemy.numberToSpawn; i++)
        {
            ran = Random.Range(0, spawner.Length);
            
            latestEnemy = Instantiate(enemy.enemyPrefab, spawner[ran].position, Quaternion.identity, spawner[ran]);
            enemy.enemyPull.pulledEnemies.Add(latestEnemy);
            Debug.Log(latestEnemy.name);
            EnemyVariableSet(latestEnemy, enemy);
            enemy.enemyPull.OnEnemyDeath += EnemyDied;
            latestEnemy.gameObject.SetActive(false);
            //Transform spawnedEnemy = Instantiate(enemy.enemyPrefab, spawner[ran].position, Quaternion.identity);
        }
    }

    private void EnemyDied()
    {
        numberOfEnemiesActive--;
    }

    void GetEnemy(EnemyPull enemyPull)
    {

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
                GetEnemy(enemy.enemyPull);
                yield return new WaitForSeconds(currentWave.waitNextSpawn);

            }
        }
        if (currentWave.isLast)
        {
            while (numberOfEnemiesActive > 0)
            {
                yield return null;
            }
            // truly ended
            isChoosingWeapon = true;
            onLastWave?.Invoke();
            while (isChoosingWeapon)
            {
                yield return null;
            }

        }
        yield return new WaitForSeconds(currentWave.waitNextWave);

        if (waveCounter < waves.Length)
        {
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
