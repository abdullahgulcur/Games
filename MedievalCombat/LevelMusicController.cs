using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicController : MonoBehaviour {

    public static LevelMusicController lmc;

	void Awake () {

        if (lmc == null)
        {
            DontDestroyOnLoad(gameObject);
            lmc = this;
        }
        else
            Destroy(gameObject);
	}
    /*
    void Start()
    {
        StartCoroutine(StartPLayingMusicLately(0.75f));
    }*/

    public void StartPLayingMusicLately(float duration)
    {
        StartCoroutine(PLayingMusic(duration));

    }

    IEnumerator PLayingMusic(float duration)
    {
        yield return new WaitForSeconds(duration);
        GetComponent<AudioSource>().Play();

    }

    public void StartPLayingMusic()
    {
        GetComponent<AudioSource>().Play();
    }

    public void StopPlayingMusic()
    {
        GetComponent<AudioSource>().Stop();
    }
}
