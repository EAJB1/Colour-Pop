using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class MoveUIPanels : MonoBehaviour
{
    [SerializeField] Transform left, right, foregroundL, foregroundR;
    [SerializeField] float speed = 2.5f;

    int padding = 440;
    Vector3 a, b;

    void Start()
    {
        AToB(left, foregroundL);
        AToB(right, foregroundR);
    }

    void Update()
    {
        BToA(left, foregroundL);
        BToA(right, foregroundR);
    }

    /// <summary>
    /// Set transform position to vector B.
    /// </summary>
    /// <param name="panel"></param>
    void AToB(Transform panel, Transform foreground)
    {
        Vector2 pos = panel.localPosition;
        a.x = pos.x;

        if (panel.CompareTag("Left"))
        {
            b.x = pos.x - padding;
            pos.x = Vector2.left.x + b.x;
        }
        else if (panel.CompareTag("Right"))
        {
            b.x = pos.x + padding;
            pos.x = Vector2.right.x + b.x;
        }

        panel.localPosition = pos;
        foreground.localPosition = pos;
    }

    /// <summary>
    /// Lerp transform from position B to A by speed.
    /// </summary>
    /// <param name="t"></param>
    void BToA(Transform panel, Transform foreground)
    {
        Vector2 pos = panel.localPosition;
        pos.x = Mathf.Lerp(panel.localPosition.x, a.x, speed * Time.deltaTime);
        panel.localPosition = pos;
        foreground.localPosition = pos;
    }
}