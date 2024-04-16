using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveBar : MonoBehaviour
{
    public static Action<Wave> CalculateWaveTime;
    float currentWaveDur = 0;
    float timer = 0;
    [SerializeField] Image image;

    private void OnEnable()
    {
        CalculateWaveTime += WaveTime;
    }

    private void WaveTime(Wave wave)
    {
        foreach (Enemy enemy in wave.enemies)
        {
            currentWaveDur += enemy.numberToSpawn * wave.waitNextSpawn;
        }
        StartCoroutine(WaveCounter());
    }

    IEnumerator WaveCounter()
    {
        timer = 0;
        while (timer < currentWaveDur)
        {
            timer += Time.deltaTime;
            image.fillAmount = timer / currentWaveDur;
            yield return null;
        }
    }
}
