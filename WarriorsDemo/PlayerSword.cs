using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class PlayerSword : MonoBehaviour
{
    private bool interactable = true;

    public AudioClip[] hitBodyClips;
    public AudioClip[] hitSwordClips;


    public GameObject player;
    Invector.CharacterController.vThirdPersonMotor motor;

    public ParticleSystem blood_0;
    public GameObject hotParticles;

    PaladinController pc;
    HitAudioSource has;

    void Start()
    {
        has = GetComponent<HitAudioSource>();
        motor = player.GetComponent<Invector.CharacterController.vThirdPersonMotor>();
    }

    void OnTriggerEnter(Collider other)
    {
        // for paladin
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (motor.isAttacking)
            {
                pc = other.transform.GetComponent<PaladinController>();

                if (interactable)//&& !motor.isAttacked
                {
                    CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);

                    if (pc.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Block"))
                    {
                        // hot particles and iron hit sound effect
                        HotParticleEffect(other.transform);
                        has.RandomizeSfx(hitSwordClips);

                    }

                    else if (!pc.isAttacked)
                    {
                        pc.isAttacked = true;
                        StartCoroutine(SetFixedForPaladin());
                        //BloodEffect(other.transform);
                        HotParticleEffect(other.transform);
                        has.RandomizeSfx(hitBodyClips);

                        // slash sound effect
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

    IEnumerator SetFixedForPaladin()
    {
        yield return new WaitForSeconds(0.35f); // lenght of animation
        //yield return new WaitForSeconds(1.5f); // lenght of animation

        pc.isAttacked = false;
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
