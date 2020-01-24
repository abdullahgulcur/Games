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

    public void ShieldBrokeWood(AudioClip clip)
    {
        hitAudioSource.clip = clip;
        hitAudioSource.volume = 1f;
        hitAudioSource.Play();

    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        hitAudioSource.pitch = randomPitch;

        hitAudioSource.clip = clips[randomIndex];

        hitAudioSource.volume = 0.1f;

        hitAudioSource.Play();
    }

    public void RandomizeSfxWhip(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        hitAudioSource.pitch = randomPitch;

        hitAudioSource.clip = clips[randomIndex];

        hitAudioSource.volume = 0.1f;

        hitAudioSource.Play();
    }

    public void RandomizeSfxArrowHitShieldIron(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        hitAudioSource.pitch = randomPitch;

        hitAudioSource.clip = clips[randomIndex];

        hitAudioSource.volume = 0.4f;

        hitAudioSource.Play();
    }

    public void RandomizeSfxArrowHitBody(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        hitAudioSource.pitch = randomPitch;

        hitAudioSource.clip = clips[randomIndex];

        hitAudioSource.volume = 0.3f;

        hitAudioSource.Play();
    }

    public void RandomizeSfxShieldIron(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        hitAudioSource.pitch = randomPitch;

        hitAudioSource.clip = clips[randomIndex];

        hitAudioSource.volume = 0.1f;

        hitAudioSource.Play();
    }

    public void RandomizeSfxShieldWood(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        hitAudioSource.pitch = randomPitch;

        hitAudioSource.clip = clips[randomIndex];

        hitAudioSource.volume = 0.7f;

        hitAudioSource.Play();
    }

}
