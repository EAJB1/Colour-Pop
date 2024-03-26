using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> clips;

    AudioClip RandomPop(List<AudioClip> clips)
    {
        return clips[Random.Range(0, clips.Count)];
    }

    public void PlayPopAudio()
    {
        audioSource.clip = RandomPop(clips);
        audioSource.Play();
    }
}
