using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class PointsManager
{
    public static float AddPoints(float current, float change)
    {
        return current += change;
    }

    /// <summary>
    /// Sets the number of points to be incremented or decrimented from current points.
    /// </summary>
    public static float SetChangePoints(float change, float newPoints, float multiplier)
    {
        return Mathf.Ceil(newPoints * multiplier);

    }

    public static string UpdatePointsText(TMP_Text text, float points)
    {
        return text.text = points.ToString();
    }
}
