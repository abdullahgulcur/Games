using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : MonoBehaviour {

    public AudioClip openSound;

    DoorAudioSource das;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        das = GetComponent<DoorAudioSource>();
    }

    public void OpenDoor()
    {
        anim.SetInteger("State", 1);
        StartCoroutine(CallSoundEffect(openSound));
    }

    IEnumerator CallSoundEffect(AudioClip ac)
    {
        yield return new WaitForSeconds(0.2f);
        das.PlaySingle(ac);
    }

    public void ShowText()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void NotToShowText()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
