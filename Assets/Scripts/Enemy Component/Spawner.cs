using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawner;

    [HideInInspector] public int waveCounter;

    [SerializeField] public Wave[] waves;

    [HideInInspector] public bool isChoosingWeapon;

    private void Update()
    {
        if (isChoosingWeapon)
        {
            return;
        }


    }
}
