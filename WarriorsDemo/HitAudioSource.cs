using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAudioSource : MonoBehaviour {

    public AudioSource hitAudioSource;            
    public float lowPitchRange = .95f; 
    public float highPitchRange = 1.05f;
    
    public void PlaySingle(AudioClip clip)
    {
        hitAudioSource.clip = clip;
        hitAudioSource.Play();
        hitAudioSource.volume = 0.1f;
    }

    public void RandomizeSfx(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        hitAudioSource.pitch = randomPitch;

        hitAudioSource.clip = clips[randomIndex];

        hitAudioSource.volume = .5f;

        hitAudioSource.Play();
    }

}
