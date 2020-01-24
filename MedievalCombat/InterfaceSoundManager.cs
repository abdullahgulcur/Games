using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceSoundManager : MonoBehaviour {

    public AudioSource efxSource;                   
    
    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.volume = 1f;
        efxSource.Play();
    }

    public void Select(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.volume = 0.45f;
        efxSource.Play();
    }

    public void Equip(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.volume = 1f;
        efxSource.Play();
    }

    public void PurchaseOrSell(AudioClip clip)
    {
        efxSource.clip = clip;

        efxSource.volume = 1f;

        efxSource.Play();
    }

    public void ChangePage(AudioClip clip)
    {
        efxSource.clip = clip;

        efxSource.volume = 0.5f;

        efxSource.Play();
    }

}
