using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class ScreenMode : MonoBehaviour
{
    [SerializeField] Graphic[] backgroundGraphics, backgroundEdgeGraphics, foregroundGraphics;
    public List<Theme> themes = new List<Theme>();
    int currentThemeIndex = 0;

    void Start()
    {
        UpdateTheme();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyBinds.ThemeMode))
        {
            currentThemeIndex++;

            if (currentThemeIndex >= themes.Count)
            {
                currentThemeIndex -= themes.Count;
            }

            UpdateTheme();
        }
    }

    void UpdateTheme()
    {
        foreach (Graphic g in backgroundGraphics)
        {
            g.color = themes[currentThemeIndex].background;
        }

        foreach (Graphic g in backgroundEdgeGraphics)
        {
            g.color = themes[currentThemeIndex].backgroundEdge;
        }

        foreach (Graphic g in foregroundGraphics)
        {
            g.color = themes[currentThemeIndex].foreground;
        }
    }
}