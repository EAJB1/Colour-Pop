using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore;

public class Colours : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] Indicator indicator;
    WaveManager waveManager;
    AudioManager audioManager;

    [Header("Circle Properties")]
    [SerializeField] float spawnWidth = 4.7f;
    [SerializeField] float spawnHeight = 4.7f;
    [SerializeField] TMP_Text colour1, colour2, colour3, colour4;
    public bool red, green, blue, yellow;
    public int totalCirclePopped, currentCircleCount, currentColourCount,
                redCount, greenCount, blueCount, yellowCount;
    float vectorX, vectorY;

    [Header("Points")]
    [SerializeField] TMP_Text pointsNumberTxt;
    public float currentPoints, pointsChange, multiplier;
    [SerializeField] float basePointsChange = 100f;

    [Header("Target Properties")]
    int randomTargetAnimation, keyColour;
    GameObject target, targetClone;
    public List<GameObject> clones = new List<GameObject>();
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
    [SerializeField] int minAnim = 2, maxAnim = 16;
    int[] layerOrder = new int[0];
    float targetAnimationSpeed;
    Animator targetAnimator;
    Vector2 targetPosition;

    void Start()
    {
        waveManager = GetComponent<WaveManager>();
        audioManager = GetComponent<AudioManager>();

        Controls.Init();
        Controls.DisableUIControls();
        Controls.EnablePlayerControls();
        Controls.DisableCursor();
        Controls.playerControls.Player.Colour1.performed += ColourKey;
        Controls.playerControls.Player.Colour2.performed += ColourKey;
        Controls.playerControls.Player.Colour3.performed += ColourKey;
        Controls.playerControls.Player.Colour4.performed += ColourKey;

        // Reverse order in layers array to assign correct object layer.
        ReverseArray(minAnim, maxAnim);

        totalCirclePopped = 0;
        pointsChange = basePointsChange;
    }

    void FixedUpdate()
    {
        CurrentColours(); // Find a way to not call on update
    }

    /// <summary>
    /// Reverse the order of an array. For example, 0 to 9 becomes 9 to 0.
    /// </summary>
    public void ReverseArray(int minLength, int maxLength)
    {
        layerOrder = new int[maxLength - (minLength - 1)];

        for (int i = 0; i < layerOrder.Length; i++)
        {
            layerOrder[i] = maxLength - i;
        }
    }

    /// <summary>
    /// Return number of children under this gameobject.
    /// </summary>
    public int ChildTotal()
    {
        return gameObject.transform.childCount;
    }

    /// <summary>
    /// Keep count of all the colours popped and which colours are currently active.
    /// </summary>
    void CurrentColours()
    {
        currentColourCount = 0; redCount = 0; greenCount = 0; blueCount = 0; yellowCount = 0;

        foreach (Transform t in transform)
        {
            UnityEngine.Color temp = t.GetComponent<SpriteRenderer>().color;

            if (temp == indicator.availableColours[0]) { redCount++; }
            else if (temp == indicator.availableColours[1]) { greenCount++; }
            else if (temp == indicator.availableColours[2]) { blueCount++; }
            else if (temp == indicator.availableColours[3]) { yellowCount++; }
        }

        currentCircleCount = redCount + greenCount + blueCount + yellowCount;

        if (redCount > 0) { currentColourCount++; red = true; } else { red = false; }
        if (greenCount > 0) { currentColourCount++; green = true; } else { green = false; }
        if (blueCount > 0) { currentColourCount++; blue = true; } else { blue = false; }
        if (yellowCount > 0) { currentColourCount++; yellow = true; } else { yellow = false; }
    }


    /// <summary>
    /// Return the last colour popped in the wave.
    /// </summary>
    public UnityEngine.Color ReturnLastColour()
    {
        if (red) { return indicator.availableColours[0]; }
        else if (green) { return indicator.availableColours[1]; }
        else if (blue) { return indicator.availableColours[2]; }
        else if (yellow) { return indicator.availableColours[3]; }
        return UnityEngine.Color.black;
    }

    /// <summary>
    /// The colour corresponds with the indicator colour.
    /// </summary>
    void Success(int index)
    {
        GameObject child = targetParent.transform.GetChild(index).gameObject;

        // Play sound effect.
        audioManager.PlayPopAudio();

        // Play particle effect.
        InstantiatePS(child.transform.position);
        particleSystemColours[keyColour].Play();

        // Remove object from clones list.
        clones.Remove(child);

        // Destroy colour game object.
        Destroy(child);
        particleSystemColours[keyColour].Stop();

        // Add points.
        currentPoints = PointsManager.AddPoints(currentPoints, pointsChange);

        // Update points UI.
        pointsNumberTxt.text = PointsManager.UpdatePointsText(pointsNumberTxt, currentPoints);

        // Increment total circles popped.
        totalCirclePopped++;

        // Update total circles popped UI.
        waveManager.totalColoursPopped.text = totalCirclePopped.ToString();

        // Update each colour popped UI.
        switch (keyColour)
        {
            case 0: colour1.text = (int.Parse(colour1.text) + 1).ToString(); break;
            case 1: colour2.text = (int.Parse(colour2.text) + 1).ToString(); break;
            case 2: colour3.text = (int.Parse(colour3.text) + 1).ToString(); break;
            case 3: colour4.text = (int.Parse(colour4.text) + 1).ToString(); break;
            default: break;
        }
    }

    /// <summary>
    /// The wrong colour was pressed.
    /// </summary>
    void Failure()
    {
        SpawnInputColour();
    }

    /// <summary>
    /// When a button is pressed, update key colour and check colour value.
    /// </summary>
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
    /// Check if the player pressed the correct colour.
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
    /// Spawn a single colour.
    /// </summary>
    public void SpawnColour()
    {
        AssignRandomColour();
        InstantiateTarget();
        AnimateTarget();
        UpdateColourList(clones, targetParent);
        UpdateOrderInLayer(randomTargetAnimation);
    }

    /// <summary>
    /// Spawn a single colour equal to the input colour.
    /// </summary>
    void SpawnInputColour()
    {
        AssignInputColour();
        InstantiateTarget();
        AnimateTarget();
        UpdateColourList(clones, targetParent);
        UpdateOrderInLayer(randomTargetAnimation);
    }

    /// <summary>
    /// Assign a random number within boundaries of target colours array.
    /// </summary>
    void AssignRandomColour()
    {
        target = targetColours[UnityEngine.Random.Range(0, targetColours.Length)];
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
        randomTargetAnimation = UnityEngine.Random.Range(minAnim, maxAnim);
        targetAnimator.SetTrigger("CircleTrigger" + randomTargetAnimation);

        // Set random animation speed
        targetAnimationSpeed = UnityEngine.Random.Range(minAnimationSpeed, maxAnimationSpeed);
        targetAnimator.speed = targetAnimationSpeed;
    }

    /// <summary>
    /// Update the list of child objects.
    /// </summary>
    void UpdateColourList(List<GameObject> objects, GameObject parent)
    {
        objects.Clear();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            objects.Add(parent.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Update the objects sprite order in layer.
    /// </summary>
    void UpdateOrderInLayer(int index)
    {
        targetClone.GetComponent<SpriteRenderer>().sortingOrder = layerOrder[index - 1];
    }
}