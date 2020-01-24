using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerWeapon : MonoBehaviour {

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


    public GameObject player;

    public ParticleSystem blood_0;
    public ParticleSystem hotParticles;
    public ParticleSystem destroyParticles;

    Player ps;
    PlayerState pState;

    LastBossCtrl lbc;
    LastBossPower lbp;

    SpearBossCtrl sbc;
    SpearBossPower sbp;

    InfantryController ic;
    EnemyHealth eh;

    ArcherController ac;
    ArcherHealth ah;

    CursedController cc;
    CursedPower cp;

    CursedBossCtrl cbc;
    CursedBossPower cbp;

    BrigandController bc;
    BrigandHealth bh;

    SpearmanController sc;
    SpearmanHealth sh;

    CyclopsController coc;
    CyclopsHealth coh;

    HeavyHeadController hhc;
    HeavyHeadHealth hhh;

    bool damaged;
    bool damaged_shield;

    HitAudioSource has;

    private bool weaponActive = false;

    void Start () {
        
       // pos = this.gameObject.transform.GetChild
        has = player.GetComponent<HitAudioSource>();
        pState = player.GetComponent<PlayerState>();
        damaged_shield = false;
        damaged = false;
        ps = player.GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        // for infantry
        if (other.gameObject.CompareTag("EnemiesBody"))
        {
            
            eh = other.transform.parent.gameObject.GetComponent<EnemyHealth>();
            ic = other.transform.parent.gameObject.GetComponent<InfantryController>();

            if ((ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack")) &&  
                !ic.GetDefenseStatus())
            {
                
                ic.SetIsAttacked(true);
                
                if (eh.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(stab0, stab1, stab2, stab3);
                    eh.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

            if ((ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack")) &&
                !ic.GetDefenseStatus())
            {

                ic.SetIsAttacked(true);

                if (eh.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(swing0, swing1, swing2);
                    eh.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

        }


        if (other.gameObject.CompareTag("EnemyShield"))
        {
            ic = other.transform.parent.parent.parent.parent.parent.GetComponent<InfantryController>();
            eh = other.transform.parent.parent.parent.parent.parent.gameObject.GetComponent<EnemyHealth>();
            
            if (ic.GetDefenseStatus())
            {
                if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                    ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") ||
                    ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                    ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
                {

                    if (eh.GetEnemyShieldHealth() > 0 && !damaged_shield)
                    {
                        CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsSmall);
                        
                        eh.ReceiveShieldDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                        damaged_shield = true;

                        if(other.transform.parent.parent.parent.parent.parent.GetComponent<EnemyArmour>().GetShield() > 2)
                            has.RandomizeSfxShieldIron(shieldhit_iron_0, shieldhit_iron_1, shieldhit_iron_2);
                        else
                            has.RandomizeSfxShieldWood(shieldhit_wood_0, shieldhit_wood_1, shieldhit_wood_2);


                        if (GameController.Instance.hotParticles)
                            HotParticlesForShield(other.transform);
                        else
                            DestroyParticles(other.transform);
                    }

                }

            }

        }


        // for archer

        if (other.gameObject.CompareTag("ArcherBody"))
        {
            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
            {
                ah = other.transform.parent.gameObject.GetComponent<ArcherHealth>();
                ac = other.transform.parent.gameObject.GetComponent<ArcherController>();
                ac.SetIsAttacked(true);

                if (ah.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(swing0, swing1, swing2);
                    ah.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if(GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
            {
                ah = other.transform.parent.gameObject.GetComponent<ArcherHealth>();
                ac = other.transform.parent.gameObject.GetComponent<ArcherController>();
                ac.SetIsAttacked(true);

                if (ah.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(stab0, stab1, stab2, stab3);
                    ah.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }
            }


        }

        // For Cursed

        if (other.gameObject.CompareTag("CursedBody"))
        {
            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
            {
                cp = other.transform.parent.gameObject.GetComponent<CursedPower>();
                cc = other.transform.parent.gameObject.GetComponent<CursedController>();

                cc.SetIsAttacked(true);

                if (cp.GetTotalHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfxShieldWood(shieldhit_wood_0, shieldhit_wood_1, shieldhit_wood_2);
                    cp.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;

                    DestroyParticles(other.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
            {
                cp = other.transform.parent.gameObject.GetComponent<CursedPower>();
                cc = other.transform.parent.gameObject.GetComponent<CursedController>();

                cc.SetIsAttacked(true);

                if (cp.GetTotalHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfxShieldWood(shieldhit_wood_0, shieldhit_wood_1, shieldhit_wood_2);
                    cp.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;

                    DestroyParticles(other.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

        }
        // For CursedBoss

        if (other.gameObject.CompareTag("CursedBossBody"))
        {
            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
            {
                cbp = other.transform.parent.gameObject.GetComponent<CursedBossPower>();
                cbc = other.transform.parent.gameObject.GetComponent<CursedBossCtrl>();

                cbc.SetIsAttacked(true);

                if (cbp.GetTotalHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfxShieldWood(shieldhit_wood_0, shieldhit_wood_1, shieldhit_wood_2);
                    cbp.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;

                    DestroyParticles(other.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
            {
                cbp = other.transform.parent.gameObject.GetComponent<CursedBossPower>();
                cbc = other.transform.parent.gameObject.GetComponent<CursedBossCtrl>();

                cbc.SetIsAttacked(true);

                if (cbp.GetTotalHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfxShieldWood(shieldhit_wood_0, shieldhit_wood_1, shieldhit_wood_2);
                    cbp.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;

                    DestroyParticles(other.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

        }

        // For Brigand

        if (other.gameObject.CompareTag("BrigandBody"))
        {
            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
            {
                bh = other.transform.parent.gameObject.GetComponent<BrigandHealth>();
                bc = other.transform.parent.gameObject.GetComponent<BrigandController>();

                bc.SetIsAttacked(true);

                if (bh.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(swing0, swing1, swing2);
                    bh.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
            {
                bh = other.transform.parent.gameObject.GetComponent<BrigandHealth>();
                bc = other.transform.parent.gameObject.GetComponent<BrigandController>();

                bc.SetIsAttacked(true);

                if (bh.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(stab0, stab1, stab2, stab3);
                    bh.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

        }

        // For Spearman

        if (other.gameObject.CompareTag("SpearmanBody"))
        {
            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
            {
                sh = other.transform.parent.gameObject.GetComponent<SpearmanHealth>();
                sc = other.transform.parent.gameObject.GetComponent<SpearmanController>();

                sc.SetIsAttacked(true);

                if (sh.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(swing0, swing1, swing2);
                    sh.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
            {
                sh = other.transform.parent.gameObject.GetComponent<SpearmanHealth>();
                sc = other.transform.parent.gameObject.GetComponent<SpearmanController>();

                sc.SetIsAttacked(true);

                if (sh.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(stab0, stab1, stab2, stab3);
                    sh.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

        }

        // Cyclops

        if (other.gameObject.CompareTag("CyclopsBody"))
        {
            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
            {
                coh = other.transform.parent.gameObject.GetComponent<CyclopsHealth>();
                coc = other.transform.parent.gameObject.GetComponent<CyclopsController>();

                coc.SetIsAttacked(true);

                if (coh.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(swing0, swing1, swing2);
                    coh.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
            {
                coh = other.transform.parent.gameObject.GetComponent<CyclopsHealth>();
                coc = other.transform.parent.gameObject.GetComponent<CyclopsController>();

                coc.SetIsAttacked(true);

                if (coh.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(stab0, stab1, stab2, stab3);
                    coh.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

        }

        // HeavyHead

        if (other.gameObject.CompareTag("HeavyHeadBody"))
        {
            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
            {
                hhh = other.transform.parent.parent.gameObject.GetComponent<HeavyHeadHealth>();
                hhc = other.transform.parent.parent.gameObject.GetComponent<HeavyHeadController>();

                hhc.SetIsAttacked(true);

                if (hhh.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(swing0, swing1, swing2);
                    hhh.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

            if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack"))
            {
                hhh = other.transform.parent.parent.gameObject.GetComponent<HeavyHeadHealth>();
                hhc = other.transform.parent.parent.gameObject.GetComponent<HeavyHeadController>();

                hhc.SetIsAttacked(true);

                if (hhh.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(stab0, stab1, stab2, stab3);
                    hhh.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

        }

        // Last Boss

        if (other.gameObject.CompareTag("BossBody"))
        {

            lbp = other.transform.parent.gameObject.GetComponent<LastBossPower>();
            lbc = other.transform.parent.gameObject.GetComponent<LastBossCtrl>();

            if ((ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack")) &&
                !lbc.GetDefenseStatus())
            {

                lbc.SetIsAttacked(true);

                if (lbp.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(stab0, stab1, stab2, stab3);
                    lbp.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

            if ((ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack")) &&
                !lbc.GetDefenseStatus())
            {

                lbc.SetIsAttacked(true);

                if (lbp.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(swing0, swing1, swing2);
                    lbp.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

        }


        if (other.gameObject.CompareTag("BossShield"))
        {
            lbc = other.transform.parent.parent.parent.parent.parent.GetComponent<LastBossCtrl>();
            lbp = other.transform.parent.parent.parent.parent.parent.gameObject.GetComponent<LastBossPower>();

            if (lbc.GetDefenseStatus())
            {
                if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                    ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") ||
                    ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                    ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
                {

                    if (lbp.GetEnemyShieldHealth() > 0 && !damaged_shield)
                    {
                        CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsSmall);

                        lbp.ReceiveShieldDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                        damaged_shield = true;

                        has.RandomizeSfxShieldIron(shieldhit_iron_0, shieldhit_iron_1, shieldhit_iron_2);

                        if (GameController.Instance.hotParticles)
                            HotParticlesForShield(other.transform);
                        else
                            DestroyParticles(other.transform);
                    }

                }

            }

        }

        //  For SpearBoss

        if (other.gameObject.CompareTag("SpearBossBody"))
        {

            sbp = other.transform.parent.gameObject.GetComponent<SpearBossPower>();
            sbc = other.transform.parent.gameObject.GetComponent<SpearBossCtrl>();

            if ((ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack")) &&
                !sbc.GetDefenseStatus())
            {

                sbc.SetIsAttacked(true);

                if (sbp.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(stab0, stab1, stab2, stab3);
                    sbp.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

            if ((ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack")) &&
                !sbc.GetDefenseStatus())
            {

                sbc.SetIsAttacked(true);

                if (sbp.GetEnemyHealth() > 0 && !damaged)
                {
                    CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsBig);
                    has.RandomizeSfx(swing0, swing1, swing2);
                    sbp.ReceiveDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                    damaged = true;
                    BloodEffect(other.transform.parent.transform);

                    if (GameController.Instance.hotParticles)
                        HotParticles(other.transform.parent.transform);
                }

            }

        }


        if (other.gameObject.CompareTag("SpearBossShield"))
        {
            sbc = other.transform.parent.parent.parent.parent.parent.GetComponent<SpearBossCtrl>();
            sbp = other.transform.parent.parent.parent.parent.parent.gameObject.GetComponent<SpearBossPower>();

            if (sbc.GetDefenseStatus())
            {
                if (ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("QuickAttack") ||
                    ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") ||
                    ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("WalkingAttack") ||
                    ps.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
                {

                    if (sbp.GetEnemyShieldHealth() > 0 && !damaged_shield)
                    {
                        CameraShaker.Instance.ShakeOnce(GameController.Instance.magnitudeValsSmall);

                        sbp.ReceiveShieldDamage(Utility.GetWeaponDamageAmountPlayer(pState.GetWeaponCategoryAndIndex(), GameController.Instance.player_power, GameController.Instance.player_sharpening, GameController.Instance.player_weaponEmber));
                        damaged_shield = true;

                        has.RandomizeSfxShieldIron(shieldhit_iron_0, shieldhit_iron_1, shieldhit_iron_2);

                        if (GameController.Instance.hotParticles)
                            HotParticlesForShield(other.transform);
                        else
                            DestroyParticles(other.transform);
                    }

                }

            }

        }


        ///////

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

    public void SetWeaponActive()
    {
        weaponActive = true;
    }

    public void SetWeaponInactive()
    {
        weaponActive = false;
    }

    IEnumerator SetFixedForShield()
    {
        yield return new WaitForSeconds(0.2f);
        damaged_shield = false;
    }

    IEnumerator SetFixed()
    {
        yield return new WaitForSeconds(0.2f);
        damaged = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("EnemyShield"))
        {
            StartCoroutine(SetFixedForShield());
        }
        if (other.gameObject.CompareTag("EnemiesBody"))
        {
            StartCoroutine(SetIsAttactedForInfantry());
            StartCoroutine(SetFixed());
        }
        if (other.gameObject.CompareTag("ArcherBody"))
        {
            StartCoroutine(SetIsAttactedForArcher());
            StartCoroutine(SetFixed());
        }
        if (other.gameObject.CompareTag("CursedBody"))
        {
            StartCoroutine(SetIsAttactedForCursed());
            StartCoroutine(SetFixed());
        }

        if (other.gameObject.CompareTag("CursedBossBody"))
        {
            StartCoroutine(SetIsAttactedForCursedBoss());
            StartCoroutine(SetFixed());
        }

        if (other.gameObject.CompareTag("BrigandBody"))
        {
            StartCoroutine(SetIsAttactedForBrigand());
            StartCoroutine(SetFixed());
        }

        if (other.gameObject.CompareTag("SpearmanBody"))
        {
            StartCoroutine(SetIsAttactedForSpearman());
            StartCoroutine(SetFixed());
        }

        if (other.gameObject.CompareTag("CyclopsBody"))
        {
            StartCoroutine(SetIsAttactedForCyclops());
            StartCoroutine(SetFixed());
        }

        if (other.gameObject.CompareTag("HeavyHeadBody"))
        {
            StartCoroutine(SetIsAttactedForHeavyhead());
            StartCoroutine(SetFixed());
        }

        if (other.gameObject.CompareTag("BossBody"))
        {
            StartCoroutine(SetIsAttactedForLastBoss());
            StartCoroutine(SetFixed());
        }

        if (other.gameObject.CompareTag("SpearBossBody"))
        {
            StartCoroutine(SetIsAttactedForSpearBoss());
            StartCoroutine(SetFixed());
        }

        if (other.gameObject.CompareTag("BossShield"))
        {
            StartCoroutine(SetFixedForShield());
        }

        if (other.gameObject.CompareTag("SpearBossShield"))
        {
            StartCoroutine(SetFixedForShield());
        }
    }

    IEnumerator SetIsAttactedForInfantry()
    {
        yield return new WaitForSeconds(0.2f);

        if(ic != null)
            ic.SetIsAttacked(false);
    }

    IEnumerator SetIsAttactedForArcher()
    {
        yield return new WaitForSeconds(0.2f);

        if (ac != null)
            ac.SetIsAttacked(false);
    }

    IEnumerator SetIsAttactedForBrigand()
    {
        yield return new WaitForSeconds(0.2f);

        if (bc != null)
            bc.SetIsAttacked(false);
    }

    IEnumerator SetIsAttactedForSpearman()
    {
        yield return new WaitForSeconds(0.2f);

        if (sc != null)
            sc.SetIsAttacked(false);
    }

    IEnumerator SetIsAttactedForCyclops()
    {
        yield return new WaitForSeconds(0.2f);

        if (coc != null)
            coc.SetIsAttacked(false);
    }

    IEnumerator SetIsAttactedForCursed()
    {
        yield return new WaitForSeconds(0.2f);

        if (cc != null)
            cc.SetIsAttacked(false);
    }

    IEnumerator SetIsAttactedForCursedBoss()
    {
        yield return new WaitForSeconds(0.2f);

        if (cbc != null)
            cbc.SetIsAttacked(false);
    }

    IEnumerator SetIsAttactedForHeavyhead()
    {
        yield return new WaitForSeconds(0.2f);

        if (hhc != null)
            hhc.SetIsAttacked(false);
    }

    IEnumerator SetIsAttactedForLastBoss()
    {
        yield return new WaitForSeconds(0.2f);

        if (lbc != null)
            lbc.SetIsAttacked(false);
    }

    IEnumerator SetIsAttactedForSpearBoss()
    {
        yield return new WaitForSeconds(0.2f);

        if (sbc != null)
            sbc.SetIsAttacked(false);
    }
}
