using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CyclopsWeapon : MonoBehaviour
{

    public ParticleSystem blood_0;
    public ParticleSystem destroyParticles;

    public GameObject enemy;
    public ParticleSystem shieldParticles;
    public ParticleSystem hotParticles;

    public AudioClip stab0;
    public AudioClip stab1;
    public AudioClip stab2;
    public AudioClip stab3;

    public AudioClip swing0;
    public AudioClip swing1;
    public AudioClip swing2;

    public AudioClip shieldhit_iron_0;
    public AudioClip shieldhit_iron_1;
    public AudioClip shieldhit_iron_2;

    public AudioClip shieldhit_wood_0;
    public AudioClip shieldhit_wood_1;
    public AudioClip shieldhit_wood_2;

    CyclopsController cc;
    CyclopsHealth ch;
    Player playerScript;
    PlayerHealth ph;
    
    bool damaged;
    bool damaged_shield;

    HitAudioSource has;

    private bool weaponActive = false;

    void Start()
    {
        ch = enemy.GetComponent<CyclopsHealth>();
        has = enemy.GetComponent<HitAudioSource>();
        cc = enemy.GetComponent<CyclopsController>();
    }

    void OnTriggerEnter2D(Collider2D other) // attack0 strong olan
    {
        if ((other.gameObject.CompareTag("AchillesBody")))//
        {

            playerScript = other.transform.parent.GetComponent<Player>();
            ph = other.transform.parent.GetComponent<PlayerHealth>();


            if (!playerScript.GetAnim().GetCurrentAnimatorStateInfo(1).IsName("Defense"))
            {
                if (cc.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                {
                    playerScript.SetIsAttacked(true);
                    playerScript.BackDirection(Utility.EnemyDirection(other.transform.parent.transform, enemy.transform));
                    if (ph.GetPlayerHealth() > 0 && !damaged)
                    {
                        CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                        has.RandomizeSfx(swing0, swing1, swing2);
                        ph.ReceiveDamage(ch.GetDamageAmount());
                        damaged = true;
                        BloodEffect(other.transform.parent.transform);
                        
                    }
                }

                if (cc.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("Attack0"))
                {
                    playerScript.SetIsAttacked(true);
                    playerScript.BackDirection(Utility.EnemyDirection(other.transform.parent.transform, enemy.transform));
                    if (ph.GetPlayerHealth() > 0 && !damaged)
                    {
                        CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                        has.RandomizeSfx(stab0, stab1, stab2, stab3);
                        ph.ReceiveDamage(ch.GetDamageAmount());
                        damaged = true;
                        BloodEffect(other.transform.parent.transform);
                        
                    }
                }

            }

        }

        if (other.gameObject.CompareTag("AchillesShield"))
        {
            playerScript = other.transform.parent.parent.parent.parent.parent.GetComponent<Player>();
            ph = other.transform.parent.parent.parent.parent.parent.GetComponent<PlayerHealth>();

            if (playerScript.GetAnim().GetCurrentAnimatorStateInfo(1).IsName("Defense") &&
                (cc.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
                cc.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("Attack0")))
            {
                if (ph.GetPlayerShieldHealth() > 0 && !damaged_shield)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsSmall);
                    ph.ReceiveShieldDamage(ch.GetDamageAmount());
                    damaged_shield = true;

                    DestroyParticles(other.transform);

                    if (other.transform.parent.parent.parent.parent.parent.GetComponent<PlayerState>().GetShield() > 2)
                        has.RandomizeSfxShieldIron(shieldhit_iron_0, shieldhit_iron_1, shieldhit_iron_2);
                    else
                        has.RandomizeSfxShieldWood(shieldhit_wood_0, shieldhit_wood_1, shieldhit_wood_2);

                }
            }
        }

    }

    void DestroyParticles(Transform pos)
    {
        Instantiate(destroyParticles, pos.position, Quaternion.identity);
    }

    void BloodEffect(Transform pos)
    {
        Vector3 pos_0 = new Vector3(pos.position.x, pos.position.y + 1f, pos.position.z);

        Instantiate(blood_0, pos_0, Quaternion.identity);
    }

    void HotParticles(Transform pos)
    {
        Vector3 pos_0 = new Vector3(pos.position.x, pos.position.y + .5f, pos.position.z);

        Instantiate(hotParticles, pos_0, Quaternion.identity);
    }

    IEnumerator SetFixed()
    {
        yield return new WaitForSeconds(0.2f);
        damaged = false;
    }

    IEnumerator SetFixedForShield()
    {
        yield return new WaitForSeconds(0.2f);
        damaged_shield = false;
    }

    IEnumerator SetIsAttacked()
    {
        yield return new WaitForSeconds(0.2f);
        playerScript.SetIsAttacked(false);
    }

    public void SetWeaponActive()
    {
        weaponActive = true;
    }

    public void SetWeaponInactive()
    {
        weaponActive = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AchillesShield"))
        {
            StartCoroutine(SetFixedForShield());
        }
        if (other.gameObject.CompareTag("AchillesBody"))
        {
            StartCoroutine(SetIsAttacked());
            StartCoroutine(SetFixed());
        }
    }
}