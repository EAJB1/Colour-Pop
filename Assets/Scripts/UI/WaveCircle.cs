using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCircle : MonoBehaviour
{
    Animator animator;

    public float animationSpeed;

    public void Init()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void PlayWaveCircle()
    {
        animator.enabled = true;
        animator.SetTrigger("CircleTrigger");
        animator.speed = animationSpeed;
        animator.Play(0);
    }
}
