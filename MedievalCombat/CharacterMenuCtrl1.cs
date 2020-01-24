using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterMenuCtrl1 : MonoBehaviour {

    public AudioClip changePage;
    public AudioClip emptyPage;
    public AudioClip selectEffect;

    public AudioClip firsSceneMusic;
    public AudioClip lastSceneMusic;
    public GameObject InterfaceMusic;

    public Text powerTxt;
    public Text agilityTxt;
    public Text vigorTxt;
    public Text defenceTxt;
    public Text bargainingTxt;

    public Text skillpoints_left;

    public Text lvlTxt;

    int moneyIndex = 0;

    int hair_index;
    int moustache_index;
    int beard_index;
    int color_index;

    int hairCutLen = 10;
    int beardLen = 6;
    int moustacheLen = 6;

    public GameObject Player;
    
    int totalLevel = 60;
    int currentLevel;

    PlayerState ps;

    public Button[] allButtons;
    Vector3[] startScales;

    public GameObject[] scenes;

    public GameObject slots;
    public GameObject pattern_0;
    public GameObject pattern_1;

    public GameObject paper;

    GameObject envanter;
    GameObject scrollBar;
    GameObject greenBoxParent;
    GameObject diffSlots;

    public InterfaceSoundManager ism;

    public GameObject map0;
    public GameObject mapBtn0;
    public GameObject mapStuff;

    public GameObject levelController;

    public GameObject fadeSprite;

    public GameObject eventSystem;

    public GameObject GroceryStore;
    public GameObject BlackSmith;
    public GameObject Weaponry;

    public GameObject SplashScreen;
    public GameObject LoadingSystem;

    Slot slot;

    //public GameObject AndroidLogin;
    //public GameObject SaveManagement;
    //public GameObject PlayCloudDataManager;

    public Text debugText;

    //public GameObject problemPopUp;

    public Text moneyText;
    public Text USDollarMoneyText;

   // public GameObject textSheet;
    public Text scenarioText;

    public GameObject firstScene;
    public GameObject hints;

    public GameObject adManager;

    int clickedSkipCount = 0;

    string currentState = "skip";

    void Start()
    {
        StartFade();
        Utility.StopTimers();
        InitStartScales();
        GameController.Instance.playerDead = false;
        GameController.Instance.atBossRoom = false;
        envanter = paper.transform.GetChild(6).gameObject;
        scrollBar = paper.transform.GetChild(8).gameObject;
        greenBoxParent = paper.transform.GetChild(5).gameObject;
        diffSlots = paper.transform.GetChild(4).gameObject;
        slot = envanter.GetComponent<Slot>();
        ps = Player.GetComponent<PlayerState>();
        currentLevel = GameController.Instance.player_level;
        hair_index = GameController.Instance.player_haircut;
        moustache_index = GameController.Instance.player_moustache;
        beard_index = GameController.Instance.player_beard;
        color_index = GameController.Instance.player_haircolor_index;
        powerTxt.text = GameController.Instance.player_power.ToString();
        agilityTxt.text = GameController.Instance.player_agility.ToString();
        vigorTxt.text = GameController.Instance.player_vigor.ToString();
        defenceTxt.text = GameController.Instance.player_defence.ToString();
        skillpoints_left.text = "Total  " + GameController.Instance.skillpoints_left.ToString();
        
        if (GameController.Instance.firstOpen) // oyunun cihazda acilmasi
        {
            LoadingSystem.SetActive(true);
            SplashScreen.SetActive(true);
            eventSystem.SetActive(false);
            StartCoroutine(FadeSplashScreenBeforeLoad());
            GameController.Instance.firstOpen = false;
        }
        else
        {

            Destroy(LoadingSystem);
            Destroy(SplashScreen);

            if (GameController.Instance.levelPassed)
                StartCoroutine(LevelAnimation());
            else
                lvlTxt.text = "Level   " + GameController.Instance.player_level.ToString();

            if (GameController.Instance.castlePassed)
                StartCoroutine(SkillPointsAnimation());
            else
                skillpoints_left.text = "Total  " + GameController.Instance.skillpoints_left;

            if (!GameController.Instance.lastScenarioDemonstrable)
                InterfaceMusic.GetComponent<MusicLoop>().StartLoopingMusic();
        }


        CheckAmounts();

        if (GameController.Instance.lastScenarioDemonstrable)
        {
            StartCoroutine(ScenerioGameFinished());
            GameController.Instance.lastScenarioDemonstrable = false;
        }

        if (!GameController.Instance.firstGameOpen)
        {
            BlackSmith.GetComponent<BlackSmithMenuCtrl>().SetHintsInactive();
            Weaponry.GetComponent<WeaponryCtrl>().SetHintsInactive();
            GroceryStore.GetComponent<GroceryStore>().SetHintsInactive();
            levelController.GetComponent<LevelController>().SetHintsInactive();
        }

    }

    void StartFade()
    {
        fadeSprite.GetComponent<Animator>().SetInteger("State", 3);
        StartCoroutine(ResetFadeAtFirst());
    }

    IEnumerator FadeSplashScreenBeforeLoad()
    {
        yield return new WaitForSeconds(0.5f);
        LoadingSystem.GetComponent<Animator>().SetInteger("State", 0);
        yield return new WaitForSeconds(10f);// should be up to load

        if (SaveSystem.LoadPlayer() != null)
        {
            if (!SaveSystem.LoadPlayer().firstGameOpen)
                LoadLocal();
            else
                StartCoroutine(FadeSplashScreenAfterLoad());
        }
        else
        {
            StartCoroutine(FadeSplashScreenAfterLoad());
        }

        
    }

    IEnumerator LevelAnimation()
    {
        Text bottomLevelText = lvlTxt.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        lvlTxt.text = "Level   " + (GameController.Instance.player_level - 1).ToString();
        bottomLevelText.text = "Level   " + GameController.Instance.player_level.ToString();
        yield return new WaitForSeconds(1.5f);
        lvlTxt.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        GameController.Instance.levelPassed = false;
    }

    IEnumerator SkillPointsAnimation()
    {
        Text bottomTotalText = skillpoints_left.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        skillpoints_left.text = "Total  " + (GameController.Instance.skillpoints_left - 12).ToString();
        bottomTotalText.text = "Total  " + GameController.Instance.skillpoints_left.ToString();
        yield return new WaitForSeconds(1.5f);
        skillpoints_left.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        GameController.Instance.castlePassed = false;
    }

    IEnumerator ScenerioGameFinished()
    {
        ism.PlaySingle(lastSceneMusic);
        firstScene.SetActive(true);
        scenarioText.text = "After struggling enemies of Erithorn and finally killing Erithorn, people of Apotolia regained their lands.";
        yield return new WaitForSeconds(0.5f);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
        yield return new WaitForSeconds(5f);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
        yield return new WaitForSeconds(0.5f);
        scenarioText.text = "They have built civilization, brought well-being, happiness and hope.";
        yield return new WaitForSeconds(0.5f);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
        yield return new WaitForSeconds(3f);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
        yield return new WaitForSeconds(0.5f);
        scenarioText.text = "After all of this, Kholos remained his life being pleased the fact that completed his will and mission.";
        yield return new WaitForSeconds(0.5f);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
        yield return new WaitForSeconds(5f);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
        yield return new WaitForSeconds(0.5f);
        firstScene.SetActive(false);
        InterfaceMusic.GetComponent<MusicLoop>().StartLoopingMusic();
    }

    public void Skip()
    {
        

        if(clickedSkipCount == 0)
        {
            currentState = "haircut";
            hints.transform.GetChild(1).gameObject.SetActive(true); // triangle
            hints.transform.GetChild(3).gameObject.SetActive(false); // skip button
            hints.transform.GetChild(2).GetComponent<Text>().text = "The first thing that makes you look better is haircut. You can change your style by clicking left and right buttons.";
            hints.transform.localPosition = new Vector3(-73, 17, 0);
            ism.Select(selectEffect);
            clickedSkipCount++;
            return;
        }

        if (clickedSkipCount == 1)
        {
            currentState = "color";
            hints.transform.GetChild(3).gameObject.SetActive(false); // skip button
            hints.transform.GetChild(2).GetComponent<Text>().text = "You can also change your hair color. It depends on your choice... You can change your hair color by clicking left and right buttons.";
            hints.transform.localPosition = new Vector3(282, 17, 0);
            ism.Select(selectEffect);
            clickedSkipCount++;
            return;
        }

        if (clickedSkipCount == 2)
        {
            currentState = "moustache";
            hints.transform.GetChild(3).gameObject.SetActive(false); // skip button
            hints.transform.GetChild(2).GetComponent<Text>().text = "Do not forget to add some moustache! That makes you look tough. Enemies would need that... You can change your moustache by clicking left and right buttons.";
            hints.transform.localPosition = new Vector3(106, -80, 0);
            ism.Select(selectEffect);
            clickedSkipCount++;
            return;
        }

        if (clickedSkipCount == 3)
        {
            currentState = "beard";
            hints.transform.GetChild(3).gameObject.SetActive(false); // skip button
            hints.transform.GetChild(2).GetComponent<Text>().text = "Beard is the other significant factor that makes you look stronger. You can change your beard by clicking left and right buttons.";
            hints.transform.localPosition = new Vector3(106, -183, 0);
            ism.Select(selectEffect);
            clickedSkipCount++;
            return;
        }

        if (clickedSkipCount == 4)
        {
            currentState = "power";
            hints.transform.GetChild(1).gameObject.SetActive(false); // triangle 0
            hints.transform.GetChild(4).gameObject.SetActive(true); // triangle 1
            hints.transform.GetChild(3).gameObject.SetActive(false); // skip button
            hints.transform.GetChild(2).GetComponent<Text>().text = "Player has different skills. Power allows you to give more damage to enemies (Increases 5% in every step). It also increases size of player a little bit. You do not want to look small in next levels...";
            hints.transform.localPosition = new Vector3(78, 240, 0);
            ism.Select(selectEffect);
            clickedSkipCount++;
            return;
        }

    }

    IEnumerator FadeSplashScreenAfterLoad()
    {
        Destroy(LoadingSystem);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
        yield return new WaitForSeconds(0.5f);
        Destroy(SplashScreen);


        ///// bu iki alan arasi sadece oyun cihazda ilk acildigi zaman gozukmeli

        if (GameController.Instance.firstGameOpen)
        {
            ism.PlaySingle(firsSceneMusic);
            firstScene.SetActive(true);
            scenarioText.text = "After decades of warfare Erithorn, king of Slaven, has forced the kingdoms into a loose alliance.";
            yield return new WaitForSeconds(0.5f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
            yield return new WaitForSeconds(3.7f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
            yield return new WaitForSeconds(0.5f);
            scenarioText.text = "Only Apotolia remains unconquered on his way.";
            yield return new WaitForSeconds(0.5f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
            yield return new WaitForSeconds(2.7f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
            yield return new WaitForSeconds(0.5f);
            scenarioText.text = "Apotolia was in hunger and misery, Erithorn took advantage of this situation and invaded islands of Apotolia.";
            yield return new WaitForSeconds(0.5f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
            yield return new WaitForSeconds(3.6f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
            yield return new WaitForSeconds(0.5f);
            scenarioText.text = "Every corner of Islands of Apotolia was in danger; Erithorn deployed most of his army to those lands.";
            yield return new WaitForSeconds(0.5f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
            yield return new WaitForSeconds(3.6f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
            yield return new WaitForSeconds(0.5f);
            scenarioText.text = "However, there was one person that is not considered by Erithorn.";
            yield return new WaitForSeconds(0.5f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
            yield return new WaitForSeconds(2.7f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
            yield return new WaitForSeconds(0.5f);
            scenarioText.text = "Kholos.";
            yield return new WaitForSeconds(0.5f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
            yield return new WaitForSeconds(2.2f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
            yield return new WaitForSeconds(0.5f);
            scenarioText.text = "Kholos was the son of Apotolia and his will would have been the only thing that makes people of Apotolia regain their lands.";
            yield return new WaitForSeconds(0.5f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
            yield return new WaitForSeconds(5f);
            fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
            yield return new WaitForSeconds(0.5f);
            firstScene.SetActive(false);
        }
        
        ///// bu iki alan arasi sadece oyun cihazda ilk acildigi zaman gozukmeli

        yield return new WaitForSeconds(1f); // ne kadar surede hint ekrani cikacak
        fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
        eventSystem.SetActive(true);

        InterfaceMusic.GetComponent<MusicLoop>().StartLoopingMusic();

        if (GameController.Instance.firstGameOpen)
            hints.SetActive(true);
        
    }

    void InitStartScales()
    {
        startScales = new Vector3[allButtons.Length];

        for(int i = 0; i < allButtons.Length; i++) 
        {
            startScales[i] = allButtons[i].transform.localScale;
        }
    }

    public void LoadSceneWithIndex(int x)// baslangic scene 0
    {
        if (GameController.Instance.firstGameOpen)
            if (!(currentState.Equals("weaponry") && x == 1))
                return;

        if(x != 4)
        {
            foreach (GameObject scene in scenes)
                scene.gameObject.SetActive(false);

            scenes[x].gameObject.SetActive(true);

            if(x == 1)
            {
                
            }
            //if (!GameController.Instance.firstGameOpen)
                //hints.SetActive(false);
        }
        

        if (x == 0)
        {
            slots.gameObject.SetActive(true);
            pattern_0.SetActive(false);
            pattern_1.SetActive(true);

            envanter.gameObject.SetActive(false);
            scrollBar.gameObject.SetActive(false);
            greenBoxParent.gameObject.SetActive(false);
            diffSlots.gameObject.SetActive(false);
        }
        else if(x != 4)
        {
            if (GameController.Instance.firstGameOpen)
                hints.SetActive(false);

            slots.gameObject.SetActive(false);
            pattern_0.SetActive(true);
            pattern_1.SetActive(false);

            envanter.gameObject.SetActive(true);
            scrollBar.gameObject.SetActive(true);
            greenBoxParent.gameObject.SetActive(true);
            diffSlots.gameObject.SetActive(true);
            slot.AddEnvanter(GameController.Instance.itemList);

            
        }

        if (x == 4)
        {
            fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
            eventSystem.SetActive(false);
            StartCoroutine(ResetFade());
        }

        ism.ChangePage(changePage);
    }

    IEnumerator ResetFade()
    {
        yield return new WaitForSeconds(0.5f);
        AdManager.Instance.ShowInterstitial();
        Player.gameObject.SetActive(false);
        paper.gameObject.SetActive(false);
        scenes[0].gameObject.SetActive(false);
        scenes[4].gameObject.SetActive(true);
        map0.gameObject.SetActive(true);
        mapBtn0.gameObject.SetActive(true);
        mapStuff.gameObject.SetActive(true);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
        levelController.GetComponent<LevelController>().SetLevelBtnColor();
        yield return new WaitForSeconds(0.5f);
        eventSystem.SetActive(true);
    }

    IEnumerator ResetFadeAtFirst()
    {
        yield return new WaitForSeconds(1.5f);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
    }

    public void MoneyRightButton()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        if (moneyIndex < 3)
        {
            ism.ChangePage(changePage);
            moneyIndex++;
        }
        else
            ism.PlaySingle(emptyPage);

        SetMoneyText(moneyIndex);
        SetDollarMoneyText(moneyIndex);

    }
    public void MoneyLeftButton()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        if (moneyIndex > 0)
        {
            ism.ChangePage(changePage);
            moneyIndex--;
            
        }
        else
            ism.PlaySingle(emptyPage);

        SetMoneyText(moneyIndex);
        SetDollarMoneyText(moneyIndex);
    }

    void SetDollarMoneyText(int index)
    {
        switch (index)
        {
            case 0:
                USDollarMoneyText.text = "0.99 $";
                break;
            case 1:
                USDollarMoneyText.text = "1.69 $";
                break;
            case 2:
                USDollarMoneyText.text = "2.99 $";
                break;
            case 3:
                USDollarMoneyText.text = "4.99 $";
                break;
        }
    }

    void SetMoneyText(int index)
    {
        switch (index)
        {
            case 0:
                moneyText.text = "250";
                break;
            case 1:
                moneyText.text = "500";
                break;
            case 2:
                moneyText.text = "1000";
                break;
            case 3:
                moneyText.text = "2000";
                break;
        }
    }

    public void PurchaseMoney()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        switch (moneyIndex)
        {
            case 0:
                IAPManager.Instance.Buy_250_Money();
                break;
            case 1:
                IAPManager.Instance.Buy_500_Money();
                break;
            case 2:
                IAPManager.Instance.Buy_1000_Money();
                break;
            case 3:
                IAPManager.Instance.Buy_2000_Money();
                break;
        }
        
    }

    public void HairColorRightButton()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("color"))
                return;


        if (color_index < GameController.Instance.color.Length - 1)
        {
            ism.ChangePage(changePage);
            color_index++;
        }
        else
            ism.PlaySingle(emptyPage);

        SetColor();
        
    }
    public void HairColorLeftButton()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("color"))
                return;

        if (color_index > 0)
        {
            color_index--;
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        SetColor();
    }

    void SetColor()
    {
        GameController.Instance.player_haircolor_index = color_index;
        ps.UpdatePlayer();

        SaveSystem.SavePlayer();
        //SaveToCloud();

        if (GameController.Instance.firstGameOpen)
            hints.transform.GetChild(3).gameObject.SetActive(true); // skip button
    }
    

    public void HairStyleRightButton()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("haircut"))
                return;

        if (hair_index < hairCutLen - 1)
        {
            ism.ChangePage(changePage);
            hair_index++;
        }
        else
            ism.PlaySingle(emptyPage);

        SetHair();

    }

    public void HairStyleLeftButton()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("haircut"))
                return;

        if (hair_index > 0)
        {
            ism.ChangePage(changePage);
            hair_index--;
        }
        else
            ism.PlaySingle(emptyPage);

        SetHair();
    }

    void SetHair()
    {
        GameController.Instance.player_haircut = hair_index;
        ps.UpdatePlayer();

        SaveSystem.SavePlayer();
        //SaveToCloud();

        if (GameController.Instance.firstGameOpen)
            hints.transform.GetChild(3).gameObject.SetActive(true); // skip button
    }

    /// 
    public void MoustacheRightButton()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("moustache"))
                return;
        
        if (moustache_index < moustacheLen - 1)
        {
            moustache_index++;
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        SetMoustache();

    }

    public void MoustacheLeftButton()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("moustache"))
                return;

        if (moustache_index > 0)
        {
            moustache_index--;
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        SetMoustache();
    }

    public void IncreaseLevel()
    {
        if(currentLevel < totalLevel)
            currentLevel++;

        GameController.Instance.player_level = currentLevel;

        lvlTxt.text = "Level   " + currentLevel.ToString();

        BlackSmith.GetComponent<BlackSmithMenuCtrl>().SetLockersInBlackSmith();
        Weaponry.GetComponent<WeaponryCtrl>().SetLockersInWeaponry();
        GroceryStore.GetComponent<GroceryStore>().SetLockersInGrocery();

        SaveSystem.SavePlayer();
        //SaveToCloud();

    }

    public void DecreaseLevel()
    {
        if (currentLevel > 1)
            currentLevel--;

        GameController.Instance.player_level = currentLevel;

        lvlTxt.text = "Level   " + currentLevel.ToString();

        BlackSmith.GetComponent<BlackSmithMenuCtrl>().SetLockersInBlackSmith();
        Weaponry.GetComponent<WeaponryCtrl>().SetLockersInWeaponry();
        GroceryStore.GetComponent<GroceryStore>().SetLockersInGrocery();

        SaveSystem.SavePlayer();
        //SaveToCloud();
    }

    void SetMoustache()
    {
        GameController.Instance.player_moustache = moustache_index;
        ps.UpdatePlayer();

        SaveSystem.SavePlayer();
        //SaveToCloud();

        if (GameController.Instance.firstGameOpen)
            hints.transform.GetChild(3).gameObject.SetActive(true); // skip button
    }

    public void BeardRightButton()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("beard"))
                return;

        if (beard_index < beardLen - 1)
        {
            ism.ChangePage(changePage);
            beard_index++;
        }
        else
            ism.PlaySingle(emptyPage);

        SetBeard();

    }

    public void BeardLeftButton()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("beard"))
                return;

        if (beard_index > 0)
        {
            ism.ChangePage(changePage);
            beard_index--;
        }
        else
            ism.PlaySingle(emptyPage);

        SetBeard();
    }

    public void UpdateSkills()
    {
        powerTxt.text = GameController.Instance.player_power.ToString();
        agilityTxt.text = GameController.Instance.player_agility.ToString();
        vigorTxt.text = GameController.Instance.player_vigor.ToString();
        defenceTxt.text = GameController.Instance.player_defence.ToString();
    }

    void SetBeard()
    {
        GameController.Instance.player_beard = beard_index;
        ps.UpdatePlayer();

        SaveSystem.SavePlayer();
        //SaveToCloud();

        if (GameController.Instance.firstGameOpen)
            hints.transform.GetChild(3).gameObject.SetActive(true); // skip button
    }

    void CheckAmounts()
    {
        int time = 1350;

        List<Item> itemList = GameController.Instance.itemList;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].GetTotalSeconds() >= time && (itemList[i].GetCategory().Equals("food") || itemList[i].GetCategory().Equals("hardware")))
            {
                GroceryStore.GetComponent<GroceryStore>().RemoveAtIndex(itemList[i].GetCategory(), itemList[i].GetIndex(), i);
                i = 0;
            }
        }

        if(itemList.Count != 0 && itemList[0].GetTotalSeconds() >= time && (itemList[0].GetCategory().Equals("food") || itemList[0].GetCategory().Equals("hardware")))
        {
            GroceryStore.GetComponent<GroceryStore>().RemoveAtIndex(itemList[0].GetCategory(), itemList[0].GetIndex(), 0);
        }
    }
    /// 

    public void IncreasePower()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("power"))
                return;
            else
            {
                currentState = "agility";
                hints.transform.GetChild(3).gameObject.SetActive(false); // skip button
                hints.transform.GetChild(2).GetComponent<Text>().text = "Increasing agility skill is the reason behind running faster (Increases 10% in every step). It is a vital factor if you do not want to get in trouble with drifters.";
                hints.transform.localPosition = new Vector3(78, 150, 0);
                ism.Select(selectEffect);
            }

        if (GameController.Instance.skillpoints_left > 0 && GameController.Instance.player_power < 50)
        {
            GameController.Instance.skillpoints_left--;
            GameController.Instance.player_power++; // no limitation
            ps.UpdatePlayer();
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        skillpoints_left.text = "Total  " + GameController.Instance.skillpoints_left.ToString();
        skillpoints_left.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Total  " + GameController.Instance.skillpoints_left.ToString();
        powerTxt.text = GameController.Instance.player_power.ToString();

        SaveSystem.SavePlayer();
        //SaveToCloud();
    }

    public void IncreaseAgility()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("agility"))
                return;
            else
            {
                currentState = "vigor";
                hints.transform.GetChild(3).gameObject.SetActive(false); // skip button
                hints.transform.GetChild(2).GetComponent<Text>().text = "Do you want to be vigorous? If yes, just click the plus button, do not think much... That skill helps you to stay alive longer than before.";
                hints.transform.localPosition = new Vector3(78, 40, 0);
                ism.Select(selectEffect);
            }


        if (GameController.Instance.skillpoints_left > 0 && GameController.Instance.player_agility < 50)
        {
            GameController.Instance.skillpoints_left--;
            GameController.Instance.player_agility++; // no limitation
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        skillpoints_left.text = "Total  " + GameController.Instance.skillpoints_left.ToString();
        skillpoints_left.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Total  " + GameController.Instance.skillpoints_left.ToString();

        agilityTxt.text = GameController.Instance.player_agility.ToString();

        SaveSystem.SavePlayer();
        //SaveToCloud();
    }

    public void IncreaseVigor()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("vigor"))
                return;
            else
            {
                currentState = "defense";
                hints.transform.GetChild(3).gameObject.SetActive(false); // skip button
                hints.transform.GetChild(2).GetComponent<Text>().text = "Enemies hate shields which you are using for defending yourself. They want to broke your shield as soon as possible. If you want to decrease sturdiness of shield, you came right place.";
                hints.transform.localPosition = new Vector3(78, -44, 0);
                ism.Select(selectEffect);
            }

        if (GameController.Instance.skillpoints_left > 0 && GameController.Instance.player_vigor < 50)
        {
            GameController.Instance.skillpoints_left--;
            GameController.Instance.player_vigor++; // no limitation
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        skillpoints_left.text = "Total  " + GameController.Instance.skillpoints_left.ToString();
        skillpoints_left.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Total  " + GameController.Instance.skillpoints_left.ToString();

        vigorTxt.text = GameController.Instance.player_vigor.ToString();

        SaveSystem.SavePlayer();
        //SaveToCloud();
    }

    public void IncreaseDefence()
    {
        if (GameController.Instance.firstGameOpen)
            if (!currentState.Equals("defense"))
                return;
            else
            {
                currentState = "weaponry";
                hints.transform.GetChild(4).gameObject.SetActive(false); // triangle 1
                hints.transform.GetChild(5).gameObject.SetActive(true); // triangle 2
                hints.transform.GetChild(3).gameObject.SetActive(false); // skip button
                hints.transform.GetChild(2).GetComponent<Text>().text = "Yes, we are done here, maybe just for now... It is time to purchase a weapon for yourself. To do this, we need to visit weaponry... Let's go !";
                hints.transform.localPosition = new Vector3(39, -56, 0);
                ism.Select(selectEffect);
            }

        if (GameController.Instance.skillpoints_left > 0 && GameController.Instance.player_defence < 50)
        {
            GameController.Instance.skillpoints_left--;
            GameController.Instance.player_defence++; // no limitation
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        skillpoints_left.text = "Total  " + GameController.Instance.skillpoints_left.ToString();
        skillpoints_left.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Total  " + GameController.Instance.skillpoints_left.ToString();

        defenceTxt.text = GameController.Instance.player_defence.ToString();

        SaveSystem.SavePlayer();
        //SaveToCloud();
    }

    public void IncreaseSizeOfButton(int index)
    {
        allButtons[index].transform.localScale = startScales[index] * 1.2f;
    }

    public void DecreaseSizeOfButton(int index)
    {
        allButtons[index].transform.localScale = startScales[index];
    }

    public void SetOpenableMapIndex(int index)
    {
        GameController.Instance.OpenableMapIndex = index;
    }
    

    public void OnLoadAction(string str)
    {
        StartCoroutine(FadeSplashScreenAfterLoad());

        PlayerData data = JsonUtility.FromJson<PlayerData>(str);
        
        GameController.Instance.player_total_money = data.player_total_money;
        GameController.Instance.player_heal = data.player_heal;
        GameController.Instance.player_sharpening = data.player_sharpening;
        GameController.Instance.player_shieldHeal = data.player_shieldHeal;
        GameController.Instance.player_weaponEmber = data.player_weaponEmber;
        GameController.Instance.player_health = data.player_health;
        GameController.Instance.player_shield_health = data.player_shield_health;
        GameController.Instance.player_haircolor_index = color_index = data.player_haircolor_index;
        GameController.Instance.player_haircut = hair_index = data.player_haircut;
        GameController.Instance.player_beard = beard_index = data.player_beard;
        GameController.Instance.player_moustache = moustache_index = data.player_moustache;
        GameController.Instance.player_breastPlate = data.player_breastPlate;
        GameController.Instance.player_helmet = data.player_helmet;
        GameController.Instance.player_tozluk = data.player_tozluk;
        GameController.Instance.player_level = currentLevel = data.player_level;
        GameController.Instance.player_vigor = data.player_vigor;
        GameController.Instance.player_power = data.player_power;
        GameController.Instance.player_agility = data.player_agility;
        GameController.Instance.player_defence = data.player_defence;
        GameController.Instance.skillpoints_left = data.skillpoints_left;
        GameController.Instance.player_sword = data.player_sword;
        GameController.Instance.player_mace = data.player_mace;
        GameController.Instance.player_axe = data.player_axe;
        GameController.Instance.player_shield = data.player_shield;
        GameController.Instance.playerEmber = data.playerEmber;
        GameController.Instance.hotParticles = data.hotParticles;

        GameController.Instance.itemList.Clear();

        for (int i = 0; i < data.index.Length; i++)
        {
            GameController.Instance.itemList.Add(new Item(data.category[i], data.index[i], data.totalSeconds[i]));
        }


        //lvlTxt.text = "Level   " + currentLevel.ToString();
        skillpoints_left.text = "Total  " + data.skillpoints_left.ToString();
        Player.GetComponent<PlayerState>().UpdatePlayer();
        UpdateSkills();

        BlackSmith.GetComponent<BlackSmithMenuCtrl>().SetLockersInBlackSmith();
        Weaponry.GetComponent<WeaponryCtrl>().SetLockersInWeaponry();
        GroceryStore.GetComponent<GroceryStore>().SetLockersInGrocery();

        slot.AddEnvanter(GameController.Instance.itemList);
    }
    /*
    public void Authenticate()
    {
        PlayCloudDataManager.GetComponent<PlayCloudDataManager>().Login();
    }

    public void LoadFromCloud()
    {
        Action<string> loadAction = OnLoadAction;

        PlayCloudDataManager.GetComponent<PlayCloudDataManager>().LoadFromCloud(loadAction);
        
    }
    */
    public void SaveLocal()
    {
        SaveSystem.SavePlayer();
    }

    public void LoadLocal() // fact
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            GameController.Instance.OpenableMapIndex = data.OpenableMapIndex;
            GameController.Instance.gameFinished = data.gameFinished;
            GameController.Instance.firstGameOpen = data.firstGameOpen;
            GameController.Instance.player_total_money = data.player_total_money;
            GameController.Instance.player_heal = data.player_heal;
            GameController.Instance.player_sharpening = data.player_sharpening;
            GameController.Instance.player_shieldHeal = data.player_shieldHeal;
            GameController.Instance.player_weaponEmber = data.player_weaponEmber;
            GameController.Instance.player_health = data.player_health;
            GameController.Instance.player_shield_health = data.player_shield_health;
            GameController.Instance.player_haircolor_index = color_index = data.player_haircolor_index;
            GameController.Instance.player_haircut = hair_index = data.player_haircut;
            GameController.Instance.player_beard = beard_index = data.player_beard;
            GameController.Instance.player_moustache = moustache_index = data.player_moustache;
            GameController.Instance.player_breastPlate = data.player_breastPlate;
            GameController.Instance.player_helmet = data.player_helmet;
            GameController.Instance.player_tozluk = data.player_tozluk;
            GameController.Instance.player_level = currentLevel = data.player_level;
            GameController.Instance.player_vigor = data.player_vigor;
            GameController.Instance.player_power = data.player_power;
            GameController.Instance.player_agility = data.player_agility;
            GameController.Instance.player_defence = data.player_defence;
            GameController.Instance.skillpoints_left = data.skillpoints_left;
            GameController.Instance.player_sword = data.player_sword;
            GameController.Instance.player_mace = data.player_mace;
            GameController.Instance.player_axe = data.player_axe;
            GameController.Instance.player_shield = data.player_shield;
            GameController.Instance.playerEmber = data.playerEmber;
            GameController.Instance.hotParticles = data.hotParticles;

            GameController.Instance.itemList.Clear();

            for (int i = 0; i < data.index.Length; i++)
            {
                GameController.Instance.itemList.Add(new Item(data.category[i], data.index[i], data.totalSeconds[i]));
            }

            lvlTxt.text = "Level   " + currentLevel.ToString();
            skillpoints_left.text = "Total  " + data.skillpoints_left.ToString();
            Player.GetComponent<PlayerState>().UpdatePlayer();
            UpdateSkills();

            BlackSmith.GetComponent<BlackSmithMenuCtrl>().SetHintsInactive();
            Weaponry.GetComponent<WeaponryCtrl>().SetHintsInactive();
            GroceryStore.GetComponent<GroceryStore>().SetHintsInactive();
            levelController.GetComponent<LevelController>().SetHintsInactive();
            levelController.GetComponent<LevelController>().SetLevelBtnColor();

            slot.AddEnvanter(GameController.Instance.itemList);

            BlackSmith.GetComponent<BlackSmithMenuCtrl>().SetLockersInBlackSmith();
            Weaponry.GetComponent<WeaponryCtrl>().SetLockersInWeaponry();
            GroceryStore.GetComponent<GroceryStore>().SetLockersInGrocery();
            //StartCoroutine(LevelAnimation());
        }
        StartCoroutine(FadeSplashScreenAfterLoad());
    }

    public void ProblemWithLogin()
    {
        //problemPopUp.SetActive(true);
        StartCoroutine(QuitApp(2f));
        LoadingSystem.SetActive(false);
    }

    public IEnumerator QuitApp(float duration)
    {
        yield return new WaitForSeconds(duration);
        Application.Quit();
    }

    /*
public void OnLevelLoad(string str)
{

    debugText.text = str;

    PlayerData data = JsonUtility.FromJson<PlayerData>(str);

    GameController.Instance.player_total_money = data.player_total_money;
    GameController.Instance.player_heal = data.player_heal;
    GameController.Instance.player_sharpening = data.player_sharpening;
    GameController.Instance.player_shieldHeal = data.player_shieldHeal;
    GameController.Instance.player_weaponEmber = data.player_weaponEmber;
    GameController.Instance.player_health = data.player_health;
    GameController.Instance.player_shield_health = data.player_shield_health;
    GameController.Instance.player_haircolor_index = color_index = data.player_haircolor_index;
    GameController.Instance.player_haircut = hair_index = data.player_haircut;
    GameController.Instance.player_beard = beard_index = data.player_beard;
    GameController.Instance.player_moustache = moustache_index = data.player_moustache;
    GameController.Instance.player_breastPlate = data.player_breastPlate;
    GameController.Instance.player_helmet = data.player_helmet;
    GameController.Instance.player_tozluk = data.player_tozluk;
    GameController.Instance.player_level = currentLevel = data.player_level;
    GameController.Instance.player_vigor = data.player_vigor;
    GameController.Instance.player_power = data.player_power;
    GameController.Instance.player_agility = data.player_agility;
    GameController.Instance.player_defence = data.player_defence;
    GameController.Instance.skillpoints_left = data.skillpoints_left;
    GameController.Instance.player_sword = data.player_sword;
    GameController.Instance.player_mace = data.player_mace;
    GameController.Instance.player_axe = data.player_axe;
    GameController.Instance.player_shield = data.player_shield;
    GameController.Instance.playerEmber = data.playerEmber;
    GameController.Instance.hotParticles = data.hotParticles;

    GameController.Instance.itemList.Clear();

    for (int i = 0; i < data.index.Length; i++)
    {
        GameController.Instance.itemList.Add(new Item(data.category[i], data.index[i], data.totalSeconds[i]));
    }


    lvlTxt.text = currentLevel.ToString();
    skillpoints_left.text = data.skillpoints_left.ToString();
    Player.GetComponent<PlayerState>().UpdatePlayer();
    UpdateSkills();

    BlackSmith.GetComponent<BlackSmithMenuCtrl>().SetLockersInBlackSmith();
    Weaponry.GetComponent<WeaponryCtrl>().SetLockersInWeaponry();
    GroceryStore.GetComponent<GroceryStore>().SetLockersInGrocery();

    slot.AddEnvanter(GameController.Instance.itemList);//burasi sikinti cikariyor
}
*/
    /*
    public void SaveToCloud()
    {
        PlayCloudDataManager.GetComponent<PlayCloudDataManager>().SaveToCloud(JsonUtility.ToJson(new PlayerData()));
    }
    */


    /*
     public void SaveToCloud()
    {
        SaveManagement.GetComponent<SaveManagement>().SaveData();
    }

    public void OnLevelLoad(string str)
    {
        
        debugText.text = str;
        
        PlayerData data = JsonUtility.FromJson<PlayerData>(str);
        
        GameController.Instance.player_total_money = data.player_total_money;
        GameController.Instance.player_heal = data.player_heal;
        GameController.Instance.player_sharpening = data.player_sharpening;
        GameController.Instance.player_shieldHeal = data.player_shieldHeal;
        GameController.Instance.player_weaponEmber = data.player_weaponEmber;
        GameController.Instance.player_health = data.player_health;
        GameController.Instance.player_shield_health = data.player_shield_health;
        GameController.Instance.player_haircolor_index = color_index = data.player_haircolor_index;
        GameController.Instance.player_haircut = hair_index = data.player_haircut;
        GameController.Instance.player_beard = beard_index = data.player_beard;
        GameController.Instance.player_moustache = moustache_index = data.player_moustache;
        GameController.Instance.player_breastPlate = data.player_breastPlate;
        GameController.Instance.player_helmet = data.player_helmet;
        GameController.Instance.player_tozluk = data.player_tozluk;
        GameController.Instance.player_level = currentLevel = data.player_level;
        GameController.Instance.player_vigor = data.player_vigor;
        GameController.Instance.player_power = data.player_power;
        GameController.Instance.player_agility = data.player_agility;
        GameController.Instance.player_defence = data.player_defence;
        GameController.Instance.skillpoints_left = data.skillpoints_left;
        GameController.Instance.player_sword = data.player_sword;
        GameController.Instance.player_mace = data.player_mace;
        GameController.Instance.player_axe = data.player_axe;
        GameController.Instance.player_shield = data.player_shield;
        GameController.Instance.playerEmber = data.playerEmber;
        GameController.Instance.hotParticles = data.hotParticles;

        GameController.Instance.itemList.Clear();

        for (int i = 0; i < data.index.Length; i++)
        {
            GameController.Instance.itemList.Add(new Item(data.category[i], data.index[i], data.totalSeconds[i]));
        }
        

        lvlTxt.text = currentLevel.ToString();
        skillpoints_left.text = data.skillpoints_left.ToString();
        Player.GetComponent<PlayerState>().UpdatePlayer();
        UpdateSkills();

        BlackSmith.GetComponent<BlackSmithMenuCtrl>().SetLockersInBlackSmith();
        Weaponry.GetComponent<WeaponryCtrl>().SetLockersInWeaponry();
        GroceryStore.GetComponent<GroceryStore>().SetLockersInGrocery();
        
        //slot.AddEnvanter(GameController.Instance.itemList);//burasi sikinti cikariyor
    }

    public void LoadFromCloud()
    {
        SaveManagement.GetComponent<SaveManagement>().LoadData();
    }

    public void SaveLocal()
    {
        SaveSystem.SavePlayer();
    }

    public void LoadLocal()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        GameController.Instance.player_total_money = data.player_total_money;
        GameController.Instance.player_heal = data.player_heal;
        GameController.Instance.player_sharpening = data.player_sharpening;
        GameController.Instance.player_shieldHeal = data.player_shieldHeal;
        GameController.Instance.player_weaponEmber = data.player_weaponEmber;
        GameController.Instance.player_health = data.player_health;
        GameController.Instance.player_shield_health = data.player_shield_health;
        GameController.Instance.player_haircolor_index = color_index = data.player_haircolor_index;
        GameController.Instance.player_haircut = hair_index = data.player_haircut;
        GameController.Instance.player_beard = beard_index = data.player_beard;
        GameController.Instance.player_moustache = moustache_index = data.player_moustache;
        GameController.Instance.player_breastPlate = data.player_breastPlate;
        GameController.Instance.player_helmet = data.player_helmet;
        GameController.Instance.player_tozluk = data.player_tozluk;
        GameController.Instance.player_level = currentLevel = data.player_level;
        GameController.Instance.player_vigor = data.player_vigor;
        GameController.Instance.player_power = data.player_power;
        GameController.Instance.player_agility = data.player_agility;
        GameController.Instance.player_defence = data.player_defence;
        GameController.Instance.skillpoints_left = data.skillpoints_left;
        GameController.Instance.player_sword = data.player_sword;
        GameController.Instance.player_mace = data.player_mace;
        GameController.Instance.player_axe = data.player_axe;
        GameController.Instance.player_shield = data.player_shield;
        GameController.Instance.playerEmber = data.playerEmber;
        GameController.Instance.hotParticles = data.hotParticles;

        GameController.Instance.itemList.Clear();

        for (int i = 0; i < data.index.Length; i++)
        {
            GameController.Instance.itemList.Add(new Item(data.category[i], data.index[i], data.totalSeconds[i]));
        }

        lvlTxt.text = currentLevel.ToString();
        skillpoints_left.text = data.skillpoints_left.ToString();
        Player.GetComponent<PlayerState>().UpdatePlayer();
        UpdateSkills();

        BlackSmith.GetComponent<BlackSmithMenuCtrl>().SetLockersInBlackSmith();
        Weaponry.GetComponent<WeaponryCtrl>().SetLockersInWeaponry();
        GroceryStore.GetComponent<GroceryStore>().SetLockersInGrocery();

        //slot.AddEnvanter(GameController.Instance.itemList);//burasi sikinti cikariyor
    }
     */
}
