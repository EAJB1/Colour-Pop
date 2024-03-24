using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.LowLevel;

public class Colours : MonoBehaviour
{
    public PlayerInput playerInput;
    public Indicator indicator;

    [SerializeField] float spawnWidth = 4.7f, spawnHeight = 4.7f;
    float vectorX, vectorY, targetAnimationSpeed;

    public bool red, green, blue, yellow;
    public int currentColourCount, redCount, greenCount, blueCount, yellowCount;

    int indexOfTargets, randomTargetAnimation, keyColour;

    public ParticleSystem[] particleSystemColours;

    GameObject target, targetClone;
    public GameObject[] targetColours;
    public GameObject targetParent;
    SpriteRenderer targetSpriteRenderer;

    GameObject particle, particleClone;
    public GameObject[] particleColours;
    public GameObject particleParent;

    Animator targetAnimator;
    public float minAnimationSpeed = 0.25f, maxAnimationSpeed = 1.75f;

    Vector2 targetPosition;

    void Start()
    {
        Controls.Init();
        Controls.DisableUIControls();
        Controls.EnablePlayerControls();
        Controls.DisableCursor();
        Controls.playerControls.Player.Colour1.performed += Colour1;
        Controls.playerControls.Player.Colour2.performed += Colour2;
        Controls.playerControls.Player.Colour3.performed += Colour3;
        Controls.playerControls.Player.Colour4.performed += Colour4;
    }

    void Update()
    {
        CurrentColours();
    }

    /// <summary>
    /// Return number of children under this gameobject.
    /// </summary>
    public int ChildTotal()
    {
        return gameObject.transform.childCount;
    }

    void CurrentColours()
    {
        currentColourCount = 0; redCount = 0; greenCount = 0; blueCount = 0; yellowCount = 0;

        foreach (Transform t in transform)
        {
            Color temp = t.GetComponent<SpriteRenderer>().color;

            if (temp == indicator.availableColours[0]) { redCount++; }
            else if (temp == indicator.availableColours[1]) { greenCount++; }
            else if (temp == indicator.availableColours[2]) { blueCount++; }
            else if (temp == indicator.availableColours[3]) { yellowCount++; }
        }

        if (redCount > 0) { currentColourCount++; red = true; } else { red = false; }
        if (greenCount > 0) { currentColourCount++; green = true; } else { green = false; }
        if (blueCount > 0) { currentColourCount++; blue = true; } else { blue = false; }
        if (yellowCount > 0) { currentColourCount++; yellow = true; } else { yellow = false; }
    }

    public Color ReturnLastColour()
    {
        if (red) { return indicator.availableColours[0]; }
        else if (green) { return indicator.availableColours[1]; }
        else if (blue) { return indicator.availableColours[2]; }
        else if (yellow) { return indicator.availableColours[3]; }
        return Color.black;
    }

    void Colour1(InputAction.CallbackContext context)
    {
        keyColour = 0;
        CheckColour();
    }

    void Colour2(InputAction.CallbackContext context)
    {
        keyColour = 1;
        CheckColour();
    }

    void Colour3(InputAction.CallbackContext context)
    {
        keyColour = 2;
        CheckColour();
    }

    void Colour4(InputAction.CallbackContext context)
    {
        keyColour = 3;
        CheckColour();
    }

    bool Input()
    {
        if (UnityEngine.Input.GetKeyDown(KeyBinds.Key1)) { keyColour = 0; return true; }
        else if (UnityEngine.Input.GetKeyDown(KeyBinds.Key2)) { keyColour = 1; return true; }
        else if (UnityEngine.Input.GetKeyDown(KeyBinds.Key3)) { keyColour = 2; return true; }
        else if (UnityEngine.Input.GetKeyDown(KeyBinds.Key4)) { keyColour = 3; return true; }
        else { return false; }
    }

    /// <summary>
    /// Spawn a new wave.
    /// </summary>
    public void SpawnWave()
    {
        AssignRandomColour();
        InstantiateTarget();
        AnimateTarget();
    }

    /// <summary>
    /// Spawn a single colour.
    /// </summary>
    void SpawnColour()
    {
        AssignInputColour();
        InstantiateTarget();
        AnimateTarget();
    }

    void CheckColour()
    {
        if (indicator.availableColours[keyColour] == indicator.currentIndicatorColour) // Correct colour
        {
            for (int i = 0; i < targetParent.transform.childCount; i++)
            {
                targetSpriteRenderer = targetParent.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
                
                if (targetSpriteRenderer.color == indicator.availableColours[keyColour]) // Found 1st child object of the same colour
                {
                    InstantiatePS(targetParent.transform.GetChild(i).transform.position);
                    particleSystemColours[keyColour].Play();
                    Destroy(targetParent.transform.GetChild(i).gameObject);
                    particleSystemColours[keyColour].Stop();
                    break;
                }
            }
        }
        else if (indicator.availableColours[keyColour] != indicator.currentIndicatorColour) // Incorrect colour
        {
            SpawnColour();
        }
    }

    void AssignRandomColour()
    {
        // Choose random coloured object
        indexOfTargets = Random.Range(0, targetColours.Length);
        target = targetColours[indexOfTargets];
    }

    void AssignInputColour()
    {
        target = targetColours[keyColour]; // Assigns the current input colour to target
    }

    void InstantiateTarget()
    {
        // Choose random position (within the boundary)
        vectorX = Random.Range(-spawnWidth, spawnWidth);
        vectorY = Random.Range(-spawnHeight, spawnHeight);
        targetPosition = new Vector2(vectorX, vectorY);

        // Create object as a child
        targetClone = Instantiate(target, targetPosition, Quaternion.identity);
        targetClone.transform.parent = gameObject.transform;
    }

    void InstantiatePS(Vector2 position)
    {
        particle = particleColours[keyColour];

        particleClone = Instantiate(particle, targetPosition, Quaternion.identity);

        particleClone.transform.position = position;
        particleClone.transform.parent = particleParent.transform;
    }

    void AnimateTarget()
    {
        // Set up animator on new target
        targetAnimator = targetClone.GetComponent<Animator>();
        targetAnimator.enabled = true;

        // Play random animation
        randomTargetAnimation = Random.Range(2, 16);
        targetAnimator.SetTrigger("CircleTrigger" + randomTargetAnimation);

        // Set random animation speed
        targetAnimationSpeed = Random.Range(minAnimationSpeed, maxAnimationSpeed);
        targetAnimator.speed = targetAnimationSpeed;
    }
}
