using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Version : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI version;

    private void Start()
    {
        version.text = "VER " + Application.version;
    }
}
