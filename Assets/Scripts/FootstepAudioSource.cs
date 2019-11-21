using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Collider))]
public class FootstepAudioSource : MonoBehaviour
{
    public LayerMask triggerLayer;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(EnumUtilities.FlagUtilities.HasAll(other.gameObject.layer, triggerLayer))
        {
            _audioSource.Play();
        }
    }
}