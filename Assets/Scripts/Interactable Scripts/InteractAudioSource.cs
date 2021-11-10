using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAudioSource : MonoBehaviour
{
    // Start is called before the first frame update

    public static InteractAudioSource current;
    private AudioSource thisAudioSource;

    private void Start()
    {
        thisAudioSource = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        current = this;
    }

    public void PlayInteractClip(AudioClip clip, float volume)
    {
        thisAudioSource.PlayOneShot(clip, volume);
    }
}
