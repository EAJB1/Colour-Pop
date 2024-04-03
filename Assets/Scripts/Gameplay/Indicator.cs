using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Indicator : MonoBehaviour
{
    [SerializeField] Colours colours;
    [SerializeField] WaveManager waveManager;

    [SerializeField] Transform circles;
    [SerializeField] string maxDurationStr = " MAX!", secStr = "(sec)";

    public TMP_Text indicatorDurationTxt, secondsTxt, totalIndicatorDurationTxt;
    public Graphic indicatorGraphic;
    public Color[] availableColours;
    public Dictionary<Color, int> colourWeights;
    public Color currentIndicatorColour;
    public int weightThreshold = 5;
    public float totalIndicatorDuration, currentDuration, minDuration = 0.25f, maxDuration = 2.25f, indicatorDecrease = .01f;

    int totalWeight = 4;
    int weightIndex;
    bool coroutineRunningCurrent = false, isFirstMillisecond = true;

    void Start()
    {
        indicatorGraphic = GetComponent<Graphic>();

        totalIndicatorDuration = maxDuration;
        secondsTxt.text = secStr;

        // Allocate a base colour
        indicatorGraphic.color = Color.white;

        colourWeights = new Dictionary<Color, int>();

        foreach (Color col in availableColours)
        {
            colourWeights.Add(col, 1);
        }
    }

    void FixedUpdate()
    {
        if (!coroutineRunningCurrent)
        {
            if (waveManager.currentWave == 1) // Set first indicator colour.
            {
                currentIndicatorColour = colours.ReturnLastColour();
                indicatorGraphic.color = currentIndicatorColour;
            }

            if (totalIndicatorDuration == minDuration) // Stop at min duration.
            {
                indicatorDurationTxt.text = minDuration.ToString() + maxDurationStr;
                secondsTxt.text = "";
            }
            else if (currentDuration <= 0) // Set new indicator colour.
            {
                isFirstMillisecond = true;
                currentDuration = totalIndicatorDuration;

                ChooseRandomColour();
                OverrideColour();

                // Spawn a circle with the new indicator colour.
                colours.SpawnIndicatorColour();

                // Set the object sprite renderer to the current colour.
                indicatorGraphic.color = currentIndicatorColour;

                StartCoroutine(CurrentIndicatorDuration());
            }
            else { StartCoroutine(CurrentIndicatorDuration()); } // Decrement current duration.
        }
    }

    IEnumerator CurrentIndicatorDuration()
    {
        coroutineRunningCurrent = true;
        float millisecond = .01f;

        if (currentDuration > 0f && !isFirstMillisecond)
        {
            currentDuration -= millisecond;
            currentDuration = (float)System.Math.Round((decimal)currentDuration, 2);
        }
        else if (currentDuration < 0f) { currentDuration = 0f; }
        else { isFirstMillisecond = false; }

        indicatorDurationTxt.text = currentDuration.ToString();

        yield return new WaitForSeconds(millisecond);
        coroutineRunningCurrent = false;
    }

    void ChooseRandomColour()
    {
        // Choose random colour
        weightIndex = Random.Range(0, totalWeight);

        // Account for colour weight
        List<int> newWeights = new List<int>();
        List<Color> colourKeys = new List<Color>();
        bool chosen = false;

        foreach (KeyValuePair<Color, int> pair in colourWeights)
        {
            colourKeys.Add(pair.Key);

            if (!chosen && (pair.Value > weightIndex || pair.Value >= weightThreshold)) // Colour chosen
            {
                currentIndicatorColour = pair.Key;
                totalWeight -= pair.Value;
                newWeights.Add(0);
                chosen = true;
            }
            else // Not chosen
            {
                weightIndex -= pair.Value;
                newWeights.Add(pair.Value + 1);
                totalWeight++;
            }
        }

        for (int i = 0; i < colourKeys.Count; i++)
        {
            colourWeights[colourKeys[i]] = newWeights[i];
        }
    }

    /// <summary>
    /// Set indicator to last colour left.
    /// </summary>
    void OverrideColour()
    {
        if (colours.currentColourCount == 1 && colours.ReturnLastColour() != Color.black)
        {
            currentIndicatorColour = colours.ReturnLastColour();
            //currentDuration = 0f;
        }
    }
}