using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject main, controls, keyboard, controller,
        keyboardToggle, controllerToggle, mainFirstButton, controlsFirstButton;
    //[SerializeField] InputActionReference Colour1 = null, Colour2 = null, Colour3 = null, Colour4 = null;
    //[SerializeField] TMP_Text bindingDisplayName = null;
    
    InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    int buildIndex = 0;

    private void Start()
    {
        // Initialise Player Input Action
        Controls.Init();
        Controls.EnableUIControls();
        Controls.DisablePlayerControls();
        Controls.EnableCursor();

        main.SetActive(true);
        controls.SetActive(false);
    }

    public void NextScene()
    {
        buildIndex++;
        SceneManager.LoadScene(buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Open Control menu.
    /// </summary>
    public void Control()
    {
        main.SetActive(false);
        controls.SetActive(true);
        mainFirstButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    }

    /// <summary>
    /// Back to previous menu.
    /// </summary>
    public void Back()
    {
        main.SetActive(true);
        controls.SetActive(false);
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
    }

    public void FlipToggle(Toggle toggle)
    {
        toggle.isOn = !toggle.isOn;
    }

    public void SetKeyboard()
    {
        keyboard.SetActive(true);
        controller.SetActive(false);
    }

    public void SetController()
    {
        keyboard.SetActive(false);
        controller.SetActive(true);
    }

    public void SetKeyBind()
    {
        /*rebindingOperation = Colour1.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(.1f)
            .OnComplete(operation => RebindComplete())
            .Start();*/

        //Controls.playerControls.Player.Colour1.ChangeBinding(0);
    }

    void RebindComplete()
    {
        rebindingOperation.Dispose();
    }
}
