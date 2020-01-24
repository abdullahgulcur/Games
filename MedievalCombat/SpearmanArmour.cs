using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanArmour : MonoBehaviour {

    public float range; // bu artarsa dusman daha guclu kusanir

    public Color[] skinColor = new Color[10];

    public GameObject[] breastPlate;
    public GameObject[] helmet;
    public GameObject[] tozluk_left;
    public GameObject[] tozluk_right;

    public GameObject[] haircuts;
    public GameObject[] beards;
    public GameObject[] moustaches;

    public GameObject[] armguard_0;
    public GameObject[] armguard_1;

    public GameObject[] spear;

    int[] weaponCategoryAndIndex = new int[2];

    public GameObject leftArm;
    public GameObject rightArm;

    private SpriteRenderer spriteR_left;
    private SpriteRenderer spriteR_right;
    public Sprite[] armSprites;

    private int playerLvl;

    private int _helmet;
    private int _breastPlate;
    private int tozluk;
    private int _spear;
    private int haircut;
    private int beard;
    private int moustache;
    private int color;

    public int Helmet
    {
        get
        {
            return _helmet;
        }
        set
        {
            _helmet = Utility.GetHelmetLogic(helmet, playerLvl, range);
        }
    }

    public int BreastPlate
    {
        get
        {
            return _breastPlate;
        }
        set
        {
            _breastPlate = Utility.GetBreastplateLogic(breastPlate, playerLvl, range);
        }
    }

    public int Tozluk
    {
        get
        {
            return tozluk;
        }
        set
        {
            tozluk = Utility.GetTozlukLogic(tozluk_left, playerLvl, range);
        }
    }


    private bool ember = false;
    private bool emberPrevious;

    public int emberPercentage = 15;

    int randomCategoryWeapon;

    void Start()
    {
        spriteR_left = leftArm.GetComponent<SpriteRenderer>();
        spriteR_right = rightArm.GetComponent<SpriteRenderer>();

        playerLvl = GameController.Instance.player_level;

        SetStyle();
        SetEmberLogic();
        
        _helmet = Utility.GetHelmetLogic(helmet, playerLvl, range);
        tozluk = Utility.GetTozlukLogic(tozluk_left, playerLvl, range);
        _breastPlate = Utility.GetBreastplateLogic(breastPlate, playerLvl, range);

        SetSpearLogic();
        
        WearEnemyBreastPlate();
        WearEnemyHelmet();
        WearEnemyTozluk();

        WearEnemySpear();

        if (_helmet == 0)
            SetHair();

        SetMoustache();
        SetBeard();
        SetHairColor();

        SetSkinColor();

    }

    void SetSkinColor()
    {
        Color color = skinColor[Utility.GetRandomNumber(0, 9)];

        transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;

        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = color;

        transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = color;

        transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = color;

        transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = color;

        transform.GetChild(0).GetChild(4).GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).GetChild(4).GetChild(36).GetComponent<SpriteRenderer>().color = color;
    }

    void SetEmberLogic()
    {
        int randomNumber = Utility.GetRandomNumber(0, 2); // burdaki deger kilicin sicak olma olasiligini degistiriyor
        if (randomNumber == 0)
        {
            emberPrevious = true;
        }
    }

    void SetSpearLogic()
    {
        _spear = Utility.GetRandomNumber((int)(playerLvl * 0.09f), (int)(playerLvl * 0.13f));

        if (_spear >= spear.Length)
            _spear = spear.Length - 1;
    }
    
    void WearEnemyBreastPlate()
    {
        breastPlate[_breastPlate].SetActive(true);
        WearEnemyArm();
    }

    void WearEnemyHelmet()
    {
        helmet[_helmet].SetActive(true);
    }

    void WearEnemyTozluk()
    {
        tozluk_left[tozluk].SetActive(true);
        tozluk_right[tozluk].SetActive(true);
    }

    void WearEnemySpear()
    {
        spear[_spear].SetActive(true);
        if (emberPrevious && _spear > 2)
        {
            ember = true;
            spear[_spear].transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.45f, 0f);
            spear[_spear].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    void SetStyle()
    {
        haircut = Utility.GetRandomNumber(0, 9);
        beard = Utility.GetRandomNumber(0, 5);
        moustache = Utility.GetRandomNumber(0, 5);
        color = Utility.GetRandomNumber(0, 9);
    }

    public void SetHair()
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
        if (haircut != 0)
            haircuts[haircut].GetComponent<SpriteRenderer>().color = GameController.Instance.color[color];
        if (beard != 0)
            beards[beard].GetComponent<SpriteRenderer>().color = GameController.Instance.color[color];
        if (moustache != 0)
            moustaches[moustache].GetComponent<SpriteRenderer>().color = GameController.Instance.color[color];
    }

    void WearEnemyArm()
    {
        for (int i = 0; i < armguard_0.Length; i++)
        {
            armguard_0[i].SetActive(false);
            armguard_1[i].SetActive(false);
        }

        if (_breastPlate == 1 || _breastPlate == 2 || _breastPlate == 3 || _breastPlate == 4 || _breastPlate == 5)
        {
            armguard_0[0].SetActive(true);
            armguard_1[0].SetActive(true);
        }
        if (_breastPlate == 8)
        {
            armguard_0[2].SetActive(true);
            armguard_1[2].SetActive(true);
        }

        if (_breastPlate == 7)
        {
            armguard_0[1].SetActive(true);
            armguard_1[1].SetActive(true);
        }

        if (_breastPlate == 9)
        {
            armguard_0[3].SetActive(true);
            armguard_1[3].SetActive(true);
        }
        if (_breastPlate == 10)
        {
            armguard_0[4].SetActive(true);
            armguard_1[4].SetActive(true);
        }
        if (_breastPlate == 11)
        {
            armguard_0[5].SetActive(true);
            armguard_1[5].SetActive(true);
        }
        if (_breastPlate == 12)
        {
            armguard_0[6].SetActive(true);
            armguard_1[6].SetActive(true);
        }
    }
    /*
    public int GetHelmet()
    {
        return this._helmet;
    }
    public int GetBreastPlate()
    {
        //Debug.Log(this._breastPlate.ToString());
        return this._breastPlate;
    }
    public int GetTozluk()
    {
        return this.tozluk;
    }
    */
    public int GetWeaponCategory()
    {
        return this.randomCategoryWeapon;
    }

    public int GetSpear()
    {
        return this._spear;
    }

    public int GetEmberPercentage()
    {
        return this.emberPercentage;
    }

    public bool GetEmber()
    {
        return this.ember;
    }
}
