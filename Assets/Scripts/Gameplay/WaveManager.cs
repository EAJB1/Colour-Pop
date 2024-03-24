using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] Colours colours;
    [SerializeField] Indicator indicator;

    public float indicatorDuration = 2.0f;
    public int currentWave = 0, maxCircleCount = 6;

    [SerializeField] float waveDelay = 2.0f;

    void Update()
    {
        if (colours.ChildTotal() == 0)
        {
            currentWave++;
            for (int i = 0; i < maxCircleCount; i++)
            {
                colours.SpawnWave();
            }
        }
    }

    IEnumerator DelayWaveSpawn()
    {
        // show wave title on screen
        Debug.Log("WAVE " + currentWave);
        yield return new WaitForSeconds(waveDelay);
        // hide wave title on screen
    }
}
