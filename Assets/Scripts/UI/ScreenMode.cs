using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class ScreenMode : MonoBehaviour
{
    [SerializeField] SpriteRenderer screenBackground;
    [SerializeField] Graphic[] backgroundGraphics, middlegroundGraphics, foregroundGraphics;
    public List<Theme> themes = new List<Theme>();
    int currentThemeIndex = 0;

    void Start()
    {
        foreach (Theme theme in themes)
        {
            theme.background.a = 1f;
            theme.middleground.a = 1f;
            theme.foreground.a = 1f;
        }

        screenBackground.color = themes[currentThemeIndex].background;
        UpdateTheme(backgroundGraphics, themes[currentThemeIndex].background);
        UpdateTheme(middlegroundGraphics, themes[currentThemeIndex].middleground);
        UpdateTheme(foregroundGraphics, themes[currentThemeIndex].foreground);
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

            screenBackground.color = themes[currentThemeIndex].background;
            UpdateTheme(backgroundGraphics, themes[currentThemeIndex].background);
            UpdateTheme(middlegroundGraphics, themes[currentThemeIndex].middleground);
            UpdateTheme(foregroundGraphics, themes[currentThemeIndex].foreground);
        }
    }

    void UpdateTheme(Graphic[] graphics, Color c)
    {
        foreach (Graphic g in graphics)
        {
            g.color = c;
        }
    }
}