using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    Colours colours;

    [SerializeField] Indicator indicator;
    [SerializeField] TMP_Text waveNumberTxt, indicatorDurationTxt, secondsTxt, totalIndicatorDurationTxt;
    [SerializeField] string maxDurationStr = " MAX!", secStr = "(sec)", totalDurationStr = "Total Duration ";

    public float totalIndicatorDuration, currentDuration, minDuration = .25f, maxDuration = 2.25f, indicatorDecrease = .01f;
    public int currentWave = -1, maxCircleCount = 1;

    bool coroutineRunning = false;

    void Start()
    {
        colours = GetComponent<Colours>();
        totalIndicatorDuration = maxDuration;
        secondsTxt.text = secStr;
    }

    void Update()
    {
        if (colours.ChildTotal() == 0)
        {
            InitWave();

            for (int i = 0; i < maxCircleCount; i++)
            {
                colours.SpawnColour();
            }
        }

        RefreshIndicator();
    }

    void InitWave()
    {
        currentWave++;
        waveNumberTxt.text = currentWave.ToString();
        maxCircleCount = currentWave;
        totalIndicatorDuration = Mathf.Clamp(totalIndicatorDuration -= indicatorDecrease, minDuration, maxDuration);
        totalIndicatorDurationTxt.text = totalDurationStr + totalIndicatorDuration.ToString();
        
        if (currentDuration <= 0f)
        {
            currentDuration = totalIndicatorDuration;
        }
    }

    void RefreshIndicator()
    {
        if (!coroutineRunning)
        {
            if (totalIndicatorDuration == minDuration)
            {
                indicatorDurationTxt.text = minDuration.ToString() + maxDurationStr;
                secondsTxt.text = "";
            }
            else { StartCoroutine(UpdateIndicatorDuration()); }
        }
    }

    IEnumerator UpdateIndicatorDuration()
    {
        coroutineRunning = true;
        float milliseconds = .01f;

        if (currentDuration > 0f) { currentDuration -= Mathf.Clamp(milliseconds, 0f, maxDuration); }
        else { currentDuration = 0f; }
        
        indicatorDurationTxt.text = currentDuration.ToString();
        yield return new WaitForSeconds(milliseconds);
        
        coroutineRunning = false;
    }
}
