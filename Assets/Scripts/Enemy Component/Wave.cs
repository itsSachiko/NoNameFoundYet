using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Wave", menuName = "Waves system/Wave")]
public class Wave : ScriptableObject
{
    [Header("Settings")]
    public float waitNextSpawn = 0.2f;
    public float waitNextWave = 1f;

    public bool isLast; 

    public Enemy[] enemies;


}
