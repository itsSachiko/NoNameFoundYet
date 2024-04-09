using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyPull", menuName ="Enemy/EnemyPull")]
public class EnemyPull : ScriptableObject
{
    public float enemyID;
    
    [HideInInspector] public List<Transform> pulledEnemies = new List<Transform>();

    public Transform enemyPrefab;

    public Action<Transform> getEnemy;

    public Action<Transform> setEnemy;
}
