using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] Indicator indicator;
    [SerializeField] Colours colours;
    [SerializeField] WaveCircle waveCircle;
    [SerializeField] WaveManager waveManager;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Controls.Init();
        Controls.DisableUIControls();
        Controls.EnablePlayerControls();
        Controls.DisableCursor();
        colours.InitControls();

        indicator.Init();
        waveCircle.Init();
        colours.Init();
        waveManager.Init();

        // START GAME
        waveManager.CheckWaveState();
    }
}
