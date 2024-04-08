using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    Colours colours;

    [SerializeField] Indicator indicator;
    [SerializeField] TMP_Text waveNumberTxt;

    public TMP_Text totalColoursPopped;
    public int currentWave = 0, maxCircleCount = 1;
    public bool firstColourOfWave; // First indicator colour of the wave.

    void Start()
    {
        colours = GetComponent<Colours>();

        // Start the first wave.
        colours.UpdateCircleCount();
        colours.CheckWaveState();
    }

    public void InitWave()
    {
        currentWave++;
        waveNumberTxt.text = currentWave.ToString();
        maxCircleCount = currentWave;
        firstColourOfWave = true;

        if (currentWave > 1)
        {
            indicator.totalIndicatorDuration = Mathf.Clamp(
                indicator.totalIndicatorDuration -= indicator.indicatorDecrease, 
                indicator.minDuration, 
                indicator.maxDuration);
        }

        indicator.totalIndicatorDurationTxt.text = indicator.totalIndicatorDuration.ToString();
        indicator.currentDuration = indicator.totalIndicatorDuration;
    }

    public void SpawnWave()
    {
        for (int i = 0; i < maxCircleCount; i++)
        {
            colours.SpawnColour();
        }
    }
}
