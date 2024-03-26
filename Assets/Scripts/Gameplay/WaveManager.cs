using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    Colours colours;
    [SerializeField] Indicator indicator;
    [SerializeField] TMP_Text waveNumber;

    public float currentIndicatorDuration, minDuration = .25f, maxDuration = 2.25f, indicatorDecrease = .01f;
    public int currentWave = -1, maxCircleCount = 1;

    void Start()
    {
        colours = GetComponent<Colours>();
        currentIndicatorDuration = maxDuration;
    }

    void Update()
    {
        if (colours.ChildTotal() == 0)
        {
            InitWave();

            for (int i = 0; i < maxCircleCount; i++)
            {
                colours.SpawnWave();
            }
        }
    }

    void InitWave()
    {
        currentWave++;
        waveNumber.text = currentWave.ToString();
        maxCircleCount = currentWave;
        currentIndicatorDuration = Mathf.Clamp(currentIndicatorDuration -= indicatorDecrease, minDuration, maxDuration);
    }
}
