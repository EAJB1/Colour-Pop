using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] Colours colours;
    [SerializeField] Indicator indicator;
    [SerializeField] TMP_Text waveNumber;

    public float indicatorDuration, minDuration = .25f, maxDuration = 2f, indicatorDecrease = .01f;
    public int currentWave = -1, maxCircleCount = 1;

    private void Start()
    {
        indicatorDuration = maxDuration;
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
        indicatorDuration = Mathf.Clamp(indicatorDuration -= indicatorDecrease, minDuration, maxDuration);
    }
}
