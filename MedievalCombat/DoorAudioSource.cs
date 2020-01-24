using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudioSource : MonoBehaviour {

    public AudioSource doorAudioSource;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    public void PlaySingle(AudioClip clip)
    {
        doorAudioSource.clip = clip;
        doorAudioSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        doorAudioSource.pitch = randomPitch;

        doorAudioSource.clip = clips[randomIndex];

        doorAudioSource.Play();
    }
}
