using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class ShuffleAudioClips : MonoBehaviour
{
    public bool playOnAwake = true;
    public AudioClip[] audioClips;
    private AudioSource _audioSource;
        
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        _audioSource.clip = audioClips[Random.Range(0, audioClips.Length - 1)];
        _audioSource.Play();
    }
}