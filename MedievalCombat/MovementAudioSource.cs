using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAudioSource : MonoBehaviour {

    public AudioSource movementAudioSource;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    public void PlaySingle(AudioClip clip)
    {
        movementAudioSource.clip = clip;
        movementAudioSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        movementAudioSource.pitch = randomPitch;

        movementAudioSource.clip = clips[randomIndex];

        movementAudioSource.Play();
    }

}
