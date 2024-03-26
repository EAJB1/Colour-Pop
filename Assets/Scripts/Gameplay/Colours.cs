using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Colours : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] Indicator indicator;
    AudioManager audioManager;

    [Header("Circle Properties")]
    [SerializeField] float spawnWidth = 4.7f;
    [SerializeField] float spawnHeight = 4.7f;
    public bool red, green, blue, yellow;
    public int circleCount, currentColourCount, redCount, greenCount, blueCount, yellowCount;
    float vectorX, vectorY;

    [Header("Target Properties")]
    int indexOfTargets, randomTargetAnimation, keyColour;
    GameObject target, targetClone;
    public GameObject[] targetColours;
    public GameObject targetParent;

    [Header("Particle System Properties")]
    public ParticleSystem[] particleSystemColours;
    GameObject particle, particleClone;
    public GameObject[] particleColours;
    public GameObject particleParent;

    [Header("Animator Properties")]
    public float minAnimationSpeed = 0.25f;
    public float maxAnimationSpeed = 1.75f;
    float targetAnimationSpeed;
    Animator targetAnimator;
    Vector2 targetPosition;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();

        Controls.Init();
        Controls.DisableUIControls();
        Controls.EnablePlayerControls();
        Controls.DisableCursor();
        Controls.playerControls.Player.Colour1.performed += ColourKey;
        Controls.playerControls.Player.Colour2.performed += ColourKey;
        Controls.playerControls.Player.Colour3.performed += ColourKey;
        Controls.playerControls.Player.Colour4.performed += ColourKey;
    }

    void FixedUpdate()
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

        circleCount = redCount + greenCount + blueCount + yellowCount;

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

    /// <summary>
    /// The colour corresponds with the indicator colour.
    /// </summary>
    /// <param name="index"></param>
    void Success(int index)
    {
        // Play sound effect.
        audioManager.PlayPopAudio();

        // Play particle effect.
        InstantiatePS(targetParent.transform.GetChild(index).transform.position);
        particleSystemColours[keyColour].Play();

        // Destroy colour game object.
        Destroy(targetParent.transform.GetChild(index).gameObject);
        particleSystemColours[keyColour].Stop();
    }

    /// <summary>
    /// The wrong colour was pressed.
    /// </summary>
    void Failure()
    {
        SpawnColour();
    }

    /// <summary>
    /// When a button is pressed, update key colour and check colour value.
    /// </summary>
    /// <param name="context"></param>
    void ColourKey(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Colour1": keyColour = 0; break;
            case "Colour2": keyColour = 1; break;
            case "Colour3": keyColour = 2; break;
            case "Colour4": keyColour = 3; break;
            default: break;
        }

        CheckColour();
    }


    /// <summary>
    /// Success if the player pressed the correct colour, else fail.
    /// </summary>
    void CheckColour()
    {
        if (indicator.availableColours[keyColour] == indicator.currentIndicatorColour)
        {
            for (int i = 0; i < targetParent.transform.childCount; i++)
            {
                // Find first child with the same colour.
                if (targetParent.transform.GetChild(i).GetComponent<SpriteRenderer>().color == 
                    indicator.availableColours[keyColour])
                {
                    Success(i);
                    break;
                }
            }
        }
        else { Failure(); }
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

    void AssignRandomColour()
    {
        // Choose random coloured object
        indexOfTargets = UnityEngine.Random.Range(0, targetColours.Length);
        target = targetColours[indexOfTargets];
    }

    /// <summary>
    /// Assigns the current input colour to target.
    /// </summary>
    void AssignInputColour()
    {
        target = targetColours[keyColour];
    }

    /// <summary>
    /// Spawn circle in the world.
    /// </summary>
    void InstantiateTarget()
    {
        // Choose random position (within the boundary)
        vectorX = UnityEngine.Random.Range(-spawnWidth, spawnWidth);
        vectorY = UnityEngine.Random.Range(-spawnHeight, spawnHeight);
        targetPosition = new Vector2(vectorX, vectorY);

        // Create object as a child
        targetClone = Instantiate(target, targetPosition, Quaternion.identity);
        targetClone.transform.parent = gameObject.transform;
    }

    /// <summary>
    /// Spawn particle effect at the targets location.
    /// </summary>
    /// <param name="position"></param>
    void InstantiatePS(Vector2 position)
    {
        particle = particleColours[keyColour];

        particleClone = Instantiate(particle, targetPosition, Quaternion.identity);

        particleClone.transform.position = position;
        particleClone.transform.parent = particleParent.transform;
    }

    /// <summary>
    /// Start circle animation.
    /// </summary>
    void AnimateTarget()
    {
        // Set up animator on new target
        targetAnimator = targetClone.GetComponent<Animator>();
        targetAnimator.enabled = true;

        // Play random animation
        randomTargetAnimation = UnityEngine.Random.Range(2, 16);
        targetAnimator.SetTrigger("CircleTrigger" + randomTargetAnimation);

        // Set random animation speed
        targetAnimationSpeed = UnityEngine.Random.Range(minAnimationSpeed, maxAnimationSpeed);
        targetAnimator.speed = targetAnimationSpeed;
    }
}
