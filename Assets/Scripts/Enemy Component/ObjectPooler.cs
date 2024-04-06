using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    private ObjectPool<RangedEnemy> ranged;
    private ObjectPool<MeleeEnemy> melee;
    private ObjectPool<MineEnemy> mine;

    private void Awake()
    {
//        ranged = new ObjectPool<RangedEnemy>();
//        melee = new ObjectPool<MeleeEnemy>();
//        mine = new ObjectPool<MineEnemy>();
//    }

//    GameObject spawn()
//    {
//        return GameObject x = Instantiate()
//    }
//}
