using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    Colours colours;
    [SerializeField] WaveCircle waveCircle;

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
        CheckWaveState();
    }

    /// <summary>
    /// Spawn a new wave when there are no circles left.
    /// </summary>
    public void CheckWaveState()
    {
        if (colours.currentCircleCount == 0)
        {
            waveCircle.StartWaveCircle();
            InitWave();
            SpawnWave();
        }
    }

    void InitWave()
    {
        currentWave++;
        waveNumberTxt.text = currentWave.ToString();
        maxCircleCount = currentWave;
        firstColourOfWave = true;

        if (currentWave > 1)
        {
            indicator.totalIndicatorDuration = Mathf.Clamp(
                indicator.totalIndicatorDuration *= indicator.indicatorDurationMultiplier,
                indicator.minDuration, 
                indicator.maxDuration);
        }

        indicator.totalIndicatorDurationTxt.text = indicator.totalIndicatorDuration.ToString();
        indicator.currentDuration = indicator.totalIndicatorDuration;

        indicator.ChooseRandomColour();
        indicator.indicatorGraphic.color = indicator.currentIndicatorColour;
    }

    void SpawnWave()
    {
        for (int i = 0; i < maxCircleCount; i++)
        {
            colours.SpawnColour();
        }
    }

    /// <summary>
    /// Check if current indicator colour is the same as any current colours.
    /// </summary>
    public void CheckIndicatorColour()
    {
        bool colourNotEqual = true;

        if (colours.ReturnCurrentColours().Count > 1)
        {
            foreach (Color c in colours.ReturnCurrentColours())
            {
                if (indicator.currentIndicatorColour != c)
                {
                    colourNotEqual = true;
                }
            }
        }

        if (colourNotEqual)
        {
            indicator.NewColourFromWave();
            indicator.indicatorGraphic.color = indicator.currentIndicatorColour;
        }
    }
}
