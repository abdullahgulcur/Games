using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class EnemySword : MonoBehaviour
{
    private bool interactable = true;

    public AudioClip[] hitBodyClips;
    public AudioClip[] hitSwordClips;

    public GameObject enemy;
    PaladinController pc;

    public ParticleSystem blood_0;
    public GameObject hotParticles;

    Invector.CharacterController.vThirdPersonController playerCtrl;
    HitAudioSource has;

    void Start()
    {
        has = GetComponent<HitAudioSource>();
        pc = enemy.GetComponent<PaladinController>();
    }


    void OnTriggerEnter(Collider other)
    {

        // for paladin
        if (other.gameObject.CompareTag("Player"))
        {
            if (pc.isAttacking)
            {
                playerCtrl = other.transform.GetComponent<Invector.CharacterController.vThirdPersonController>();


                if (interactable && !pc.isAttacked)
                {
                    CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);

                    if (!playerCtrl.isAttacked && !playerCtrl.isBlocking) // player receives attack with weapon
                    {
                        playerCtrl.isAttacked = true;
                        playerCtrl.isKicked = true;
                        //BloodEffect(other.transform);
                        HotParticleEffect(other.transform);
                        has.RandomizeSfx(hitBodyClips);

                        StartCoroutine(SetFixedForPlayer());


                        // slash sound effect
                    }
                    else if (!playerCtrl.isAttacked && playerCtrl.isBlocking)
                    {
                        has.RandomizeSfx(hitSwordClips);
                        HotParticleEffect(other.transform);
                    }
                    interactable = false;
                    StartCoroutine(SetFixed());

                }

            }

        }


    }

    IEnumerator SetFixed()
    {
        yield return new WaitForSeconds(1f); // lenght of animation
        interactable = true;
    }

    IEnumerator SetFixedForPlayer()
    {
        yield return new WaitForSeconds(0.35f); // lenght of animation
        //yield return new WaitForSeconds(1.5f); // lenght of animation

        playerCtrl.isAttacked = false;
        playerCtrl.isKicked = false;
    }

    void BloodEffect(Transform pos)
    {
        Vector3 pos_0 = new Vector3(pos.position.x, pos.position.y + 1.15f, pos.position.z);

        Quaternion angle = Quaternion.AngleAxis(-90, Vector3.right);

        Instantiate(blood_0, pos_0, angle);
    }

    void HotParticleEffect(Transform pos)
    {
        Vector3 pos_0 = new Vector3(pos.position.x, pos.position.y + 1.15f, pos.position.z);

        Quaternion angle = Quaternion.AngleAxis(0, Vector3.right);

        Instantiate(hotParticles, pos_0, angle);
    }
}
