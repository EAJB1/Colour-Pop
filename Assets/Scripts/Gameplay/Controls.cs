using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class Controls
{
    public static PlayerInputAction playerControls;

    /// <summary>
    /// Initialise Player Input Actions.
    /// </summary>
    public static void Init()
    {
        playerControls = new PlayerInputAction();
    }

    /// <summary>
    /// Enable 'Player' Input Action Map.
    /// </summary>
    public static void EnablePlayerControls()
    {
        playerControls.Player.Enable();
    }

    /// <summary>
    /// Disable 'Player' Input Action Map.
    /// </summary>
    public static void DisablePlayerControls()
    {
        playerControls.Player.Disable();
    }

    /// <summary>
    /// Enable 'UI' Input Action Map.
    /// </summary>
    public static void EnableUIControls()
    {
        playerControls.UI.Enable();
    }

    /// <summary>
    /// Disable 'UI' Input Action Map.
    /// </summary>
    public static void DisableUIControls()
    {
        playerControls.UI.Disable();
    }

    /// <summary>
    /// Unlock cursor and set to visible.
    /// </summary>
    public static void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Lock cursor and set to invisible.
    /// </summary>
    public static void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
