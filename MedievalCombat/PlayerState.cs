using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    bool ember; // satin alinip alinmadigini belli eder sadece

    int breastPlate;
    int helmet;
    int tozluk;
    
    int haircut = 0;
    int beard = 0;
    int moustache = 0;
    int color = 0;

    int sword;
    int mace;
    int axe;

    int shield;

    int weaponCategory;
    int [] weaponCategoryAndIndex = new int[2];

    public float damageOffset = 1f;

    private int weaponDamageAmount;

    public int player_level;

    public int player_stamina;
    public int player_vigor;
    public int player_power;
    public float player_Agility;
    public int player_attack;
    public int player_defence;
    public int player_bargaining;
    
    public GameObject[] helmets;
    public GameObject[] tozluk_sol;
    public GameObject[] tozluk_sag;

    public GameObject[] swords;
    public GameObject[] maces;
    public GameObject[] axes;

    public GameObject[] shields;

    public GameObject [] eyeBrows;

    public GameObject[] haircuts;
    public GameObject[] beards;
    public GameObject[] moustaches;

    public GameObject[] breastplate;
    
    public GameObject leftArm;
    public GameObject rigthArm;

    GameObject leftHand;

    PlayerArm armScript0;
    PlayerArm armScript1;

    Player playerCtrl;

    Vector3 startPersonScale;

    void Start () {

        UpdatePlayer();
        startPersonScale = transform.localScale;
    }

    public void UpdatePlayer()
    {
        playerCtrl = GetComponent<Player>();

        armScript0 = leftArm.GetComponent<PlayerArm>();
        armScript1 = rigthArm.GetComponent<PlayerArm>();

        ember = GameController.Instance.playerEmber;
        breastPlate = GameController.Instance.player_breastPlate;
        helmet = GameController.Instance.player_helmet;
        tozluk = GameController.Instance.player_tozluk;
        haircut = GameController.Instance.player_haircut;
        beard = GameController.Instance.player_beard;
        moustache = GameController.Instance.player_moustache;
        color = GameController.Instance.player_haircolor_index;
        sword = GameController.Instance.player_sword;
        mace = GameController.Instance.player_mace;
        axe = GameController.Instance.player_axe;
        shield = GameController.Instance.player_shield;
        
        WearBreastPlate();
        WearHelmet();
        WearTozluk_R();
        WearTozluk_L();

        StartCoroutine(SetPersonSize());

        SetHair();
        SetMoustache();
        SetBeard();
        SetHairColor();

        if (shield != -1)
            WearShield();
        
        leftHand = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;


        if (sword != -1)
        {
            WearSword();
            weaponCategory = 0;
            weaponCategoryAndIndex[0] = 0;
            weaponCategoryAndIndex[1] = sword;
            GameController.Instance.player_mace = -1; // buralar eklendi
            GameController.Instance.player_axe = -1;  // buralar eklendi

        }

        if (mace != -1)
        {
            WearMace();
            weaponCategory = 1;
            weaponCategoryAndIndex[0] = 1;
            weaponCategoryAndIndex[1] = mace;
            GameController.Instance.player_sword = -1; // buralar eklendi
            GameController.Instance.player_axe = -1;   // buralar eklendi
        }

        if (axe != -1)
        {
            WearAxe();
            weaponCategory = 2;
            weaponCategoryAndIndex[0] = 2;
            weaponCategoryAndIndex[1] = axe;
            GameController.Instance.player_sword = -1;  // buralar eklendi
            GameController.Instance.player_mace = -1;   // buralar eklendi
        }

        if (ember && (sword > 3 || mace > 3 || axe > 3))
        {
            switch (weaponCategory)
            {
                case 0:
                    GameController.Instance.hotParticles = true;
                    leftHand.transform.GetChild(sword).GetChild(1).gameObject.SetActive(true);
                    leftHand.transform.GetChild(sword).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.45f, 0f);
                    break;
                case 1:
                    GameController.Instance.hotParticles = true;
                    leftHand.transform.GetChild(12 + mace).GetChild(1).gameObject.SetActive(true);
                    leftHand.transform.GetChild(12 + mace).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.45f, 0f);
                    break;
                case 2:
                    GameController.Instance.hotParticles = true;
                    leftHand.transform.GetChild(20 + axe).GetChild(1).gameObject.SetActive(true);
                    leftHand.transform.GetChild(20 + axe).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.45f, 0f);
                    break;
            }
        }
        else if(!ember && (sword > 3 || mace > 3 || axe > 3))
        {
            switch (weaponCategory)
            {
                case 0:
                    GameController.Instance.hotParticles = false;
                    leftHand.transform.GetChild(sword).GetChild(1).gameObject.SetActive(false);
                    leftHand.transform.GetChild(sword).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 1f, 1f);
                    break;
                case 1:
                    GameController.Instance.hotParticles = false;
                    leftHand.transform.GetChild(12 + mace).GetChild(1).gameObject.SetActive(false);
                    leftHand.transform.GetChild(12 + mace).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 1f, 1f);
                    break;
                case 2:
                    GameController.Instance.hotParticles = false;
                    leftHand.transform.GetChild(20 + axe).GetChild(1).gameObject.SetActive(false);
                    leftHand.transform.GetChild(20 + axe).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 1f, 1f);
                    break;
            }
        }

        //SaveSystem.SavePlayer();

    }
    
    IEnumerator SetPersonSize()
    {
        yield return new WaitForSeconds(0.001f);
        transform.localScale = startPersonScale + GameController.Instance.player_power * new Vector3(0.0005f, 0.0005f, 0f);
    }

    void WearBreastPlate()
    {
        for (int i = 0; i < breastplate.Length; i++)
        {
            if (i == breastPlate)
                continue;
            else
                breastplate[i].SetActive(false);
        }

        breastplate[breastPlate].SetActive(true);

        StartCoroutine(WearArms(breastPlate));
    }
    
    void WearHelmet()
    {
        for (int i = 0; i < helmets.Length; i++)
        {
            if (i == helmet)
                continue;
            else
                helmets[i].SetActive(false);
        }

        helmets[helmet].SetActive(true);
    }

    void WearTozluk_R()
    {
        for (int i = 0; i < tozluk_sag.Length; i++)
        {
            if (i == tozluk)
                continue;
            else
                tozluk_sag[i].SetActive(false);
        }

        tozluk_sag[tozluk].SetActive(true);
    }

    void WearTozluk_L()
    {
        for (int i = 0; i < tozluk_sol.Length; i++)
        {
            if (i == tozluk)
                continue;
            else
                tozluk_sol[i].SetActive(false);
        }

        tozluk_sol[tozluk].SetActive(true);
    }

    void WearShield()
    {
        for (int i = 0; i < shields.Length; i++)
        {
            if (i == shield)
                continue;
            else
                shields[i].SetActive(false);
        }

        shields[shield].SetActive(true);
    }

    public void WearSword()
    {

        for (int i = 0; i < swords.Length; i++)
        {
            if (i == sword)
                continue;
            else
                swords[i].SetActive(false);
        }

        swords[sword].SetActive(true);
    }

    public void WearAxe()
    {

        for (int i = 0; i < axes.Length; i++)
        {
            if (i == axe)
                continue;
            else
                axes[i].SetActive(false);
        }

        axes[axe].SetActive(true);
    }

    public void WearMace()
    {

        for (int i = 0; i < maces.Length; i++)
        {
            if (i == mace)
                continue;
            else
                maces[i].SetActive(false);
        }

        maces[mace].SetActive(true);
    }

    public GameObject GetWeapon()
    {
        GameObject hand = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        GameObject weapon;

        if (weaponCategoryAndIndex[0] == 0)
        {
            weapon = hand.transform.GetChild(weaponCategoryAndIndex[1]).gameObject;
        }
        else if(weaponCategoryAndIndex[0] == 1)
        {
            weapon = hand.transform.GetChild(weaponCategoryAndIndex[1] + 12).gameObject;
        }
        else
        {
            weapon = hand.transform.GetChild(weaponCategoryAndIndex[1] + 20).gameObject;
        }


        return weapon;
    }

    public void ResetAllWeapons()
    {
        GameController.Instance.player_sword = -1;
        GameController.Instance.player_mace = -1;
        GameController.Instance.player_axe = -1;

        for (int i = 0; i < swords.Length; i++)
            swords[i].SetActive(false);

        for (int i = 0; i < maces.Length; i++)
            maces[i].SetActive(false);

        for (int i = 0; i < axes.Length; i++)
            axes[i].SetActive(false);
    }

    public void ResetBreastplate()
    {
        for (int i = 0; i < breastplate.Length; i++)
            breastplate[i].SetActive(false);

        breastplate[0].SetActive(true);

        GameController.Instance.player_breastPlate = 0;
        armScript0.ChangeArmSpriteWithIndex(0); // hataliydi
        armScript1.ChangeArmSpriteWithIndex(0);
    }

    public void ResetHelmet()
    {
        for (int i = 0; i < helmets.Length; i++)
            helmets[i].SetActive(false);

        GameController.Instance.player_helmet = 0;
    }

    public void ResetTozluk()
    {
        for (int i = 0; i < tozluk_sag.Length; i++)
            tozluk_sag[i].SetActive(false);

        for (int i = 0; i < tozluk_sol.Length; i++)
            tozluk_sol[i].SetActive(false);

        GameController.Instance.player_tozluk = 0;
    }

    public void ResetShield()
    {
        for (int i = 0; i < shields.Length; i++)
            shields[i].SetActive(false);

        GameController.Instance.player_shield = -1;
    }

    public int GetWeaponCategory()
    {
        return weaponCategory;
    }

    public int [] GetWeaponCategoryAndIndex()
    {
        return weaponCategoryAndIndex;
    }

    public void SetHair()
    {
        if (GameController.Instance.player_helmet == 0)
        {
            for (int i = 0; i < haircuts.Length; i++)
            {
                if (i == haircut)
                    continue;
                else
                    haircuts[i].SetActive(false);
            }
            haircuts[haircut].SetActive(true);
        }
        else
            ResetHaircut();
        
    }

    void SetMoustache()
    {
        for (int i = 0; i < moustaches.Length; i++)
        {
            if (i == moustache)
                continue;
            else
                moustaches[i].SetActive(false);
        }
        moustaches[moustache].SetActive(true);
        
    }

    void SetBeard()
    {
        for (int i = 0; i < beards.Length; i++)
        {
            if (i == beard)
                continue;
            else
                beards[i].SetActive(false);
        }
        beards[beard].SetActive(true);
        
    }
    
    void SetHairColor()
    {
        if(haircut != 0)
            haircuts[haircut].GetComponent<SpriteRenderer>().color = GameController.Instance.color[color];
        if (beard != 0)
            beards[beard].GetComponent<SpriteRenderer>().color = GameController.Instance.color[color];
        if (moustache != 0)
            moustaches[moustache].GetComponent<SpriteRenderer>().color = GameController.Instance.color[color];

        eyeBrows[0].GetComponent<SpriteRenderer>().color = GameController.Instance.color[color];
        eyeBrows[1].GetComponent<SpriteRenderer>().color = GameController.Instance.color[color];
        
    }

    void ResetHaircut()
    {
        for (int i = 0; i < haircuts.Length; i++)
            haircuts[i].SetActive(false);
    }

    void ResetBeard()
    {
        for (int i = 0; i < beards.Length; i++)
            beards[i].SetActive(false);
    }

    void ResetMoustache()
    {
        for (int i = 0; i < moustaches.Length; i++)
            moustaches[i].SetActive(false);
    }

    IEnumerator WearArms(int index) // biraz gecikmeli baslamali
    {
        yield return new WaitForSeconds(0.001f);

        armScript0.ChangeArmSprite(index); // hataliydi
        armScript1.ChangeArmSprite(index);
    }

    public int GetShield()
    {
        return shield;
    }

}
