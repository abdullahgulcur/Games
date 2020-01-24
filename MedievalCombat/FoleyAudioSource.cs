using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoleyAudioSource : MonoBehaviour {

    public AudioSource foleyAudioSource;
    
    public void CoinTake(AudioClip ac)
    {
        foleyAudioSource.clip = ac;
        foleyAudioSource.volume = 1f;
        foleyAudioSource.Play();
    }

    internal void DoorClosed(AudioClip ac)
    {
        foleyAudioSource.clip = ac;
        foleyAudioSource.volume = 0.25f;
        foleyAudioSource.Play();
    }
}
