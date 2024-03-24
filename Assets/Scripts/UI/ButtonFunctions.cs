using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFunctions : MonoBehaviour
{
    public void Bigger()
    {
        transform.localScale = new Vector2(1.2f, 1.2f);
    }

    public void Smaller()
    {
        transform.localScale = new Vector2(1f, 1f);
    }

    public void SelectInputField()
    {
        Debug.Log(gameObject.name);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
