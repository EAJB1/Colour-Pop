using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCircle : MonoBehaviour
{
    Animator animator;

    public float animationSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void StartWaveCircle()
    {
        animator.enabled = true;
        animator.SetTrigger("CircleTrigger");
        animator.speed = animationSpeed;
        animator.Play(0);
    }
}
