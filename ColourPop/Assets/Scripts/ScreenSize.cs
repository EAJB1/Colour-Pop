using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    // This script should be used for scaling sprites for orthographic view only

    // Returning the screen height as a float
    public static float GetScreenToWorldHeight
    {
        // Convert the screen point to world coordinate
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var height = edgeVector.y * 2;
            return height;
        }
    }

    // Returning the screen width as a float
    public static float GetScreenToWorldWidth
    {
        // Convert the screen point to world coordinate
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVector.x * 2;
            return width;
        }
    }
}