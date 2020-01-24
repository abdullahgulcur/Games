using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ArcherWeapon : MonoBehaviour {
    
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

    public ParticleSystem blood_0;
    public ParticleSystem shieldParticles;
    public ParticleSystem hotParticles;
    public ParticleSystem destroyParticles;

    public GameObject enemy;

    ArcherController arCtrl;
    Player playerScript;
    PlayerHealth ph;
    EnemySkill es;
    ArcherArmour aa;

    bool damaged;
    bool damaged_shield;

    HitAudioSource has;

    private bool weaponActive = false;

    void Start () {

        has = enemy.GetComponent<HitAudioSource>();
        es = enemy.GetComponent<EnemySkill>();
        aa = enemy.GetComponent<ArcherArmour>();
        damaged_shield = false;
        damaged = false;
        arCtrl = enemy.GetComponent<ArcherController>();
    }

    void OnTriggerEnter2D(Collider2D other) // GetWeaponCategoryAndIndex()
    {

        if ((other.gameObject.CompareTag("AchillesBody")))//
        {

            playerScript = other.transform.parent.GetComponent<Player>();
            ph = other.transform.parent.GetComponent<PlayerHealth>();


            if (!playerScript.GetAnim().GetCurrentAnimatorStateInfo(1).IsName("Defense"))
            {
                if (arCtrl.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
                {
                    playerScript.SetIsAttacked(true);
                    playerScript.BackDirection(Utility.EnemyDirection(other.transform.parent.transform, enemy.transform));
                    if (ph.GetPlayerHealth() > 0 && !damaged)
                    {
                        CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                        has.RandomizeSfx(swing0, swing1, swing2);
                        ph.ReceiveDamage(Utility.GetWeaponDamageAmount(aa.GetWeaponCategoryAndIndex(), es.GetEnemyPower()));
                        damaged = true;
                        BloodEffect(other.transform.parent.transform);

                        if (aa.GetEmber())
                            HotParticles(other.transform.parent.transform);
                    }
                }

                if (arCtrl.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack"))
                {
                    playerScript.SetIsAttacked(true);
                    playerScript.BackDirection(Utility.EnemyDirection(other.transform.parent.transform, enemy.transform));
                    if (ph.GetPlayerHealth() > 0 && !damaged)
                    {
                        CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                        has.RandomizeSfx(stab0, stab1, stab2, stab3);
                        ph.ReceiveDamage(Utility.GetWeaponDamageAmount(aa.GetWeaponCategoryAndIndex(), es.GetEnemyPower()));
                        damaged = true;
                        BloodEffect(other.transform.parent.transform);

                        if (aa.GetEmber())
                            HotParticles(other.transform.parent.transform);
                    }
                }

            }


        }

        if (other.gameObject.CompareTag("AchillesShield"))
        {
            playerScript = other.transform.parent.parent.parent.parent.parent.GetComponent<Player>();
            ph = other.transform.parent.parent.parent.parent.parent.GetComponent<PlayerHealth>();

            if (playerScript.GetAnim().GetCurrentAnimatorStateInfo(1).IsName("Defense") &&
                (arCtrl.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                arCtrl.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                arCtrl.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") ||
                arCtrl.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack")))
            {
                if (ph.GetPlayerShieldHealth() > 0 && !damaged_shield)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsSmall);
                    ph.ReceiveShieldDamage(Utility.GetWeaponDamageAmount(aa.GetWeaponCategoryAndIndex(), es.GetEnemyPower()));
                    damaged_shield = true;

                    if (aa.GetEmber())
                        HotParticlesForShield(other.transform);
                    else
                        DestroyParticles(other.transform);

                    if (other.transform.parent.parent.parent.parent.parent.GetComponent<PlayerState>().GetShield() > 2)
                        has.RandomizeSfxShieldIron(shieldhit_iron_0, shieldhit_iron_1, shieldhit_iron_2);
                    else
                        has.RandomizeSfxShieldWood(shieldhit_wood_0, shieldhit_wood_1, shieldhit_wood_2);
                }
            }
        }
        
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

    void HotParticlesForShield(Transform pos)
    {
        Instantiate(hotParticles, pos.position, Quaternion.identity);
    }

    void DestroyParticles(Transform pos)
    {
        Instantiate(destroyParticles, pos.position, Quaternion.identity);
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

    public void SetWeaponActive()
    {
        weaponActive = true;
    }

    public void SetWeaponInactive()
    {
        weaponActive = false;
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
}
