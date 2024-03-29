using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    [SerializeField] Colours colours;
    [SerializeField] WaveManager waveManager;

    [SerializeField] Transform circles;

    public Graphic indicatorGraphic;
    public Color[] availableColours;
    public Dictionary<Color, int> colourWeights;
    public Color currentIndicatorColour;
    public int weightThreshold = 5;

    int totalWeight = 4;
    int weightIndex;
    bool coroutineRunning = false;

    void Start()
    {
        // Find sprite renderer
        indicatorGraphic = GetComponent<Graphic>();

        // Allocate a base colour
        indicatorGraphic.color = Color.white;

        colourWeights = new Dictionary<Color, int>();

        foreach (Color col in availableColours)
        {
            colourWeights.Add(col, 1);
        }
    }

    void Update()
    {
        // Run the coroutine when it is not running
        if (!coroutineRunning)
        {
            StartCoroutine(IndicatorWait());
            ChooseColour();
            OverrideColour();

            // Set the object sprite renderer to the current colour
            indicatorGraphic.color = currentIndicatorColour;
        }
    }

    /// <summary>
    /// Time allowed to destroy circles.
    /// </summary>
    IEnumerator IndicatorWait()
    {
        coroutineRunning = true;
        waveManager.currentDuration = waveManager.totalIndicatorDuration;
        yield return new WaitForSeconds(waveManager.totalIndicatorDuration);
        coroutineRunning = false;
    }

    void ChooseColour()
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
            waveManager.currentDuration = .00f;
        }
    }
}