using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour {

    public AudioSource as0;
    public AudioSource as1;

    public void StartLoopingMusic()
    {
        StartCoroutine(CallSoundLoop());
    }

    IEnumerator CallSoundLoop()
    {
        if(as0.isPlaying)
            as1.Play();
        else
            as0.Play();

        yield return new WaitForSeconds(7.5f);
        StartCoroutine(CallSoundLoop());
    }

    public void StopPlayingMusic()
    {
        GetComponent<AudioSource>().Stop();
    }

}
