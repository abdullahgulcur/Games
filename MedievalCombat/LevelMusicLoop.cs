using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicLoop : MonoBehaviour {

    public AudioSource as0;
    public AudioSource as1;

    void Start()
    {
        StartCoroutine(CallSoundLoopLately());
    }

    IEnumerator CallSoundLoopLately()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(CallSoundLoop());
    }

    IEnumerator CallSoundLoop()
    {
        if (as0.isPlaying)
            as1.Play();
        else
            as0.Play();

        yield return new WaitForSeconds(6.45f);
        StartCoroutine(CallSoundLoop());
    }
}
