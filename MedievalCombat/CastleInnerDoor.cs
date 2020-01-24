using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleInnerDoor : MonoBehaviour {

    public AudioClip openSound;
    public AudioClip closeSound;

    DoorAudioSource das;
    /*
    * Kapinin acilmasi ve kapanmasi icin gerekli animasyonlari iceriyo
    * 24 ve 32. satir aslinda ayni islevi goruyor. Birisi kapatip digeri aciyor
    * Ayni zamanda inverse animation icin guzel bir ornek
    * 
    */

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        das = GetComponent<DoorAudioSource>();
    }

	public void CloseDoor()
    {
        if (!transform.parent.GetComponents<Collider2D>()[1].enabled)
        {
            anim.SetInteger("State", 2);
            StartCoroutine(CallSoundEffect(closeSound));
            transform.parent.GetComponents<Collider2D>()[1].enabled = true;
        }
        
    }
    
    public void OpenDoor()
    {
        if (transform.parent.GetComponents<Collider2D>()[1].enabled)
        {
            anim.SetInteger("State", 1);
            StartCoroutine(CallSoundEffect(openSound));
            transform.parent.GetComponents<Collider2D>()[1].enabled = false;
        }
    }

    IEnumerator CallSoundEffect(AudioClip ac)
    {
        yield return new WaitForSeconds(0.2f);
        das.PlaySingle(ac);
    }

}
