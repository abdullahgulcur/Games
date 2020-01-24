using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAudioSource : MonoBehaviour {

    public AudioSource swingAudioSource;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    public void PlaySingle(AudioClip clip)
    {
        swingAudioSource.clip = clip;
        swingAudioSource.Play();
    }

    public void RandomizeSfxWhip(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        swingAudioSource.pitch = randomPitch;
        swingAudioSource.volume = 0.3f;
        swingAudioSource.priority = 0;
        swingAudioSource.clip = clips[randomIndex];

        swingAudioSource.Play();
    }

}
