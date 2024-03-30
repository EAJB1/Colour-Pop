using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Theme", menuName = "ScriptableObjects/Theme", order = 1)]
[System.Serializable]
public class Theme : ScriptableObject
{
    public Color background;
    public Color middleground;
    public Color foreground;
}
