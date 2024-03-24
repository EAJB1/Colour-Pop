using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class ScreenMode : MonoBehaviour
{
    [SerializeField] TMP_InputField[] backgroundInputFields;
    [SerializeField] Graphic[] backgroundGraphics, foregroundGraphics;
    [SerializeField] SpriteRenderer[] backgroundSprites, foregroundSprites;
    [SerializeField] TextMeshProUGUI[] foregroundCanvasUI;
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

        foreach (SpriteRenderer sprite in backgroundSprites)
        {
            sprite.color = themes[currentThemeIndex].background;
        }
        
        foreach (TMP_InputField field in backgroundInputFields)
        {
            field.image.color = themes[currentThemeIndex].background;
        }

        foreach (Graphic g in foregroundGraphics)
        {
            g.color = themes[currentThemeIndex].foreground;
        }

        foreach (SpriteRenderer sprite in foregroundSprites)
        {
            sprite.color = themes[currentThemeIndex].foreground;
        }

        foreach (TextMeshProUGUI tMP in foregroundCanvasUI)
        {
            tMP.color = themes[currentThemeIndex].foreground;
        }
    }
}