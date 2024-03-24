using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    void Update()
    {
        // Declare width and height values
        float width = ScreenSize.GetScreenToWorldWidth;
        float height = ScreenSize.GetScreenToWorldHeight;

        // Check if the window is landscape
        if (width > height)
        {
            transform.localScale = Vector3.one * height;
            //transform.localScale = Vector3.one * ((width + height) / 2);
        }

        // Check if the window is portrait
        if (height > width)
        {
            transform.localScale = Vector3.one * width;
        }

        // Check if the length of the window edges are equal
        if (width == height)
        {
            transform.localScale = Vector3.one * ((width + height) / 2);
        }
    }
}
