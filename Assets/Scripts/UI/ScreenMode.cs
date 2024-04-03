using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class ScreenMode : MonoBehaviour
{
    [SerializeField] SpriteRenderer screenBackground;
    [SerializeField] Graphic[] backgroundGraphics, middlegroundGraphics, foregroundGraphics;
    public List<Theme> themes = new List<Theme>();
    int currentThemeIndex = 0;

    void Start()
    {
        SetAlpha(1f);
        SetTheme();
    }

    void SetAlpha(float alpha)
    {
        foreach (Theme theme in themes)
        {
            theme.background.a = alpha;
            theme.middleground.a = alpha;
            theme.foreground.a = alpha;
        }
    }

    public void CycleThemes(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "ThemeForwards":
                currentThemeIndex++;
                if (currentThemeIndex == themes.Count)
                {
                    currentThemeIndex = 0;
                }
                break;
            case "ThemeBackwards":
                currentThemeIndex--;
                if (currentThemeIndex < 0)
                {
                    currentThemeIndex = themes.Count - 1;
                }
                break;
        }

        SetTheme();
    }

    void SetTheme()
    {
        foreach (Theme theme in themes)
        {
            if (theme.orderIndex == currentThemeIndex)
            {
                screenBackground.color = themes[currentThemeIndex].background;
                UpdateThemeItems(backgroundGraphics, themes[currentThemeIndex].background);
                UpdateThemeItems(middlegroundGraphics, themes[currentThemeIndex].middleground);
                UpdateThemeItems(foregroundGraphics, themes[currentThemeIndex].foreground);
            }
        }
    }

    void UpdateThemeItems(Graphic[] graphics, Color c)
    {
        foreach (Graphic g in graphics)
        {
            g.color = c;
        }
    }
}