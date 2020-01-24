using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WeaponryCtrl : MonoBehaviour {

    public AudioClip coinShake;
    public AudioClip equip;
    public AudioClip changePage;
    public AudioClip emptyPage;
    public AudioClip selectEffect;
    public AudioClip locked;
    List<Item> itemList;

    GameObject envanter;

    public GameObject paper;

    //GameObject items;

    Slot slot;

    public Button[] allButtons;
    Vector3[] startScales;

    public Button sellBtn;
    public Button useBtn;
    public Button purchaseBtn;
    
    int lastInventoryTick = -1;

    public GameObject player;

    public GameObject[] Sword_Group;
    public GameObject[] Axe_Group;
    public GameObject[] Mace_Group;

    GameObject scrollBar;

    public Text damageMultiplier;/////////////////////
    public Text armorMultiplier;//////////////////
    public Text shieldMultiplier;//////////////////

    public Text price;///////////////////////
    public Text sellingPrice;//////////////////////////////
    public Text availableText;
    public Text message;

    public Text purchased_txt;

    public Text leftAmount;

    int swordPage;
    int axePage;
    int macePage;

    int categoryIndex;

    PlayerState ps;

    public GameObject ItemTexts;

    int invPage = 0;

    GameObject greenBoxParent;

    public Text yourMoney;///////////////////

    struct LastSelectedItem
    {
        public string category;
        public int index;
    };

    LastSelectedItem lsi;

    public GameObject[] scenes;

    public GameObject slots;
    public GameObject pattern_0;
    public GameObject pattern_1;

    GameObject diffSlots;

    public InterfaceSoundManager ism;
    
    public GameObject map0;
    public GameObject mapBtn0;

    public GameObject mapStuff;

    public GameObject fadeSprite;

    public GameObject levelController;

    public GameObject eventSystem;

    public GameObject characterMenuCtrl;

    public GameObject yourMoneyIcon;
    public GameObject sellingPriceIcon;
    public GameObject armourIcon;
    public GameObject damageIcon;
    public GameObject shieldIcon;

    int clickedSkipCount = 0;

    public GameObject hints;

    string currentState = "skip";

    void Start()
    {
        if (!GameController.Instance.firstGameOpen)
            hints.SetActive(false);

        SetLockersInWeaponry();
        InitStartScales();
        envanter = paper.transform.GetChild(6).gameObject;
        scrollBar = paper.transform.GetChild(8).gameObject;
        greenBoxParent = paper.transform.GetChild(5).gameObject;
        diffSlots = paper.transform.GetChild(4).gameObject;

        lsi = new LastSelectedItem();
        slot = envanter.GetComponent<Slot>();
        itemList = GameController.Instance.itemList;
        ps = player.GetComponent<PlayerState>();
        categoryIndex = 0;
        
    }

    public void SetHintsInactive()
    {
        if (!GameController.Instance.firstGameOpen)
            hints.SetActive(false);
    }

    public void SetLockersInWeaponry()
    {
        for (int i = 0; i < 12; i++)
        {
            if (GameController.Instance.player_level < Utility.RequiredLevelForTwelve(i))
            {
                if (i % 4 == 0)
                    Sword_Group[i / 4].transform.GetChild(3).gameObject.SetActive(true);
                else if(i % 4 == 1)
                    Sword_Group[i / 4].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                else if (i % 4 == 2)
                    Sword_Group[i / 4].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                else if (i % 4 == 3)
                    Sword_Group[i / 4].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                if (i % 4 == 0)
                    Sword_Group[i / 4].transform.GetChild(3).gameObject.SetActive(false);
                else if (i % 4 == 1)
                    Sword_Group[i / 4].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                else if (i % 4 == 2)
                    Sword_Group[i / 4].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                else if (i % 4 == 3)
                    Sword_Group[i / 4].transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < 8; i++)
        {
            if (GameController.Instance.player_level < Utility.RequiredLevelForEight(i))
            {
                if (i % 4 == 0)
                    Axe_Group[i / 4].transform.GetChild(3).gameObject.SetActive(true);
                else if (i % 4 == 1)
                    Axe_Group[i / 4].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                else if (i % 4 == 2)
                    Axe_Group[i / 4].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                else if (i % 4 == 3)
                    Axe_Group[i / 4].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                if (i % 4 == 0)
                    Axe_Group[i / 4].transform.GetChild(3).gameObject.SetActive(false);
                else if (i % 4 == 1)
                    Axe_Group[i / 4].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                else if (i % 4 == 2)
                    Axe_Group[i / 4].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                else if (i % 4 == 3)
                    Axe_Group[i / 4].transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < 8; i++)
        {
            if (GameController.Instance.player_level < Utility.RequiredLevelForEight(i))
            {
                if (i % 4 == 0)
                    Mace_Group[i / 4].transform.GetChild(3).gameObject.SetActive(true);
                else if (i % 4 == 1)
                    Mace_Group[i / 4].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                else if (i % 4 == 2)
                    Mace_Group[i / 4].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                else if (i % 4 == 3)
                    Mace_Group[i / 4].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                if (i % 4 == 0)
                    Mace_Group[i / 4].transform.GetChild(3).gameObject.SetActive(false);
                else if (i % 4 == 1)
                    Mace_Group[i / 4].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                else if (i % 4 == 2)
                    Mace_Group[i / 4].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                else if (i % 4 == 3)
                    Mace_Group[i / 4].transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            }
        }


    }

    public void Skip()
    {
        if (clickedSkipCount == 0)
        {
            currentState = "sword";
            hints.transform.GetChild(0).gameObject.SetActive(false);
            hints.transform.GetChild(1).gameObject.SetActive(true);
            hints.transform.GetChild(2).gameObject.SetActive(true); // triangle
            hints.transform.GetChild(4).gameObject.SetActive(false); // skip button
            hints.transform.GetChild(3).GetComponent<Text>().text = "That sword is not bad at all for beginning. When you click that sword, you will be able to see information of sword and the money you have.";
            hints.transform.localPosition = new Vector3(-124, -175, 0);
            ism.Select(selectEffect);
            clickedSkipCount++;
            return;
        }
    }

    public void PressReturnInterface()
    {
        SceneManager.LoadScene("Interface");
    }

    public void pressDownButton(int x)
    {
        if (GameController.Instance.firstGameOpen)
            return;

        TickEmpty(0);

        switch (categoryIndex)
        {
            case 0:

                if (x == -1)
                {
                    if (swordPage < 2)
                    {
                        swordPage++;
                        ism.ChangePage(changePage);
                    }
                    else
                        ism.PlaySingle(emptyPage);


                }
                else
                {
                    if (swordPage > 0)
                    {
                        swordPage--;
                        ism.ChangePage(changePage);
                    }
                    else
                        ism.PlaySingle(emptyPage);
                }


                for (int i = 0; i < Sword_Group.Length; i++)
                {
                    if (i == swordPage)
                        continue;
                    else
                        Sword_Group[i].SetActive(false);
                }
                Sword_Group[swordPage].SetActive(true);

                break;
            case 1:

                if (x == -1)
                {
                    if (axePage < 1)
                    {
                        axePage++;
                        ism.ChangePage(changePage);
                    }
                    else
                        ism.PlaySingle(emptyPage);
                }
                else
                {
                    if (axePage > 0)
                    {
                        axePage--;
                        ism.ChangePage(changePage);
                    }
                    else
                        ism.PlaySingle(emptyPage);
                }

                for (int i = 0; i < Axe_Group.Length; i++)
                {
                    if (i == axePage)
                        continue;
                    else
                        Axe_Group[i].SetActive(false);
                }
                Axe_Group[axePage].SetActive(true);

                break;
            case 2:

                if (x == -1)
                {
                    if (macePage < 1)
                    {
                        macePage++;
                        ism.ChangePage(changePage);
                    }
                    else
                        ism.PlaySingle(emptyPage);

                }
                else
                {
                    if (macePage > 0)
                    {
                        macePage--;
                        ism.ChangePage(changePage);
                    }
                    else
                        ism.PlaySingle(emptyPage);
                }

                for (int i = 0; i < Mace_Group.Length; i++)
                {
                    if (i == macePage)
                        continue;
                    else
                        Mace_Group[i].SetActive(false);
                }
                Mace_Group[macePage].SetActive(true);

                break;
        }
    }

    public void ChangeCategory(int x)
    {
        if (GameController.Instance.firstGameOpen)
            return;

        categoryIndex = x;
        swordPage = 0;
        axePage = 0;
        macePage = 0;

        for (int i = 0; i < Axe_Group.Length; i++)
        {
            Axe_Group[i].SetActive(false);
        }
        for (int i = 0; i < Sword_Group.Length; i++)
        {
            Sword_Group[i].SetActive(false);
        }
        for (int i = 0; i < Mace_Group.Length; i++)
        {
            Mace_Group[i].SetActive(false);
        }

        switch (categoryIndex)
        {
            case 0:
                Sword_Group[0].SetActive(true);
                break;
            case 1:
                Axe_Group[0].SetActive(true);
                break;
            case 2:
                Mace_Group[0].SetActive(true);
                break;

        }
        TickEmpty(0);

        ism.ChangePage(changePage);
    }

    public void LoadSceneWithIndex(int x)// baslangic scene 0
    {
        if (GameController.Instance.firstGameOpen)
            if (!(currentState.Equals("blacksmith") && x == 2))
                return;

        if (x != 4)
        {
            foreach (GameObject scene in scenes)
                scene.gameObject.SetActive(false);

            scenes[x].gameObject.SetActive(true);
        }
        

        ChangeCategory(0);

        TickEmpty(0);

        invPage = 0;
        slot.ChangePage(invPage);
        ConfigureScrollBarPos();

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
        else
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

        player.gameObject.SetActive(false);
        paper.gameObject.SetActive(false);
        scenes[1].gameObject.SetActive(false);
        scenes[4].gameObject.SetActive(true);
        map0.gameObject.SetActive(true);
        mapBtn0.gameObject.SetActive(true);
        mapStuff.gameObject.SetActive(true);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
        levelController.GetComponent<LevelController>().SetLevelBtnColor();
        yield return new WaitForSeconds(0.5f);
        eventSystem.SetActive(true);
    }

    void ShowItemText(int x) // x == 0 price ----- x == 1 selling price   burada selling price aktive et last inv de deaktive 
    {
        //get damage amount
        int damageAmount = Utility.GetDamageMultiplier(lsi.category, lsi.index);
        int armorAmount = Utility.GetArmorMultiplier(lsi.category, lsi.index);
        int shieldAmount = Utility.GetShieldMultiplier(lsi.index);

        UpdateYourMoney();

        damageMultiplier.text = damageAmount.ToString();
        armorMultiplier.text = armorAmount.ToString();
        shieldMultiplier.text = shieldAmount.ToString();

        ItemTexts.gameObject.SetActive(true);
        
        price.text = Price.GetPrice(lsi.category, lsi.index).ToString();
        sellingPrice.text = Price.GetSellingPrice(lsi.category, lsi.index).ToString();

        foreach (Transform child in ItemTexts.transform)
            child.gameObject.SetActive(false);

        yourMoney.gameObject.SetActive(true);
        yourMoneyIcon.SetActive(true);

        if (x == 0)
        {
            sellingPriceIcon.SetActive(true);
            price.gameObject.SetActive(true);
            sellingPrice.gameObject.SetActive(false);
        }
        else if (x == 1)
        {
            sellingPriceIcon.SetActive(true);
            price.gameObject.SetActive(false);
            sellingPrice.gameObject.SetActive(true);
        }

        
        if (lsi.category.Equals("sword"))
        {
            ItemTexts.transform.GetChild(48 + lsi.index).gameObject.SetActive(true);
            if(x == 0)
            {
                ItemTexts.transform.GetChild(48 + lsi.index).GetChild(2).gameObject.SetActive(true);
                ItemTexts.transform.GetChild(48 + lsi.index).GetChild(3).gameObject.SetActive(false);
            }
            else if(x == 1)
            {
                ItemTexts.transform.GetChild(48 + lsi.index).GetChild(2).gameObject.SetActive(false);
                ItemTexts.transform.GetChild(48 + lsi.index).GetChild(3).gameObject.SetActive(true);
            }
            
        }

        if (lsi.category.Equals("axe"))
        {
            ItemTexts.transform.GetChild(60 + lsi.index).gameObject.SetActive(true);
            if (x == 0)
            {
                ItemTexts.transform.GetChild(60 + lsi.index).GetChild(2).gameObject.SetActive(true);
                ItemTexts.transform.GetChild(60 + lsi.index).GetChild(3).gameObject.SetActive(false);
            }
            else if (x == 1)
            {
                ItemTexts.transform.GetChild(60 + lsi.index).GetChild(2).gameObject.SetActive(false);
                ItemTexts.transform.GetChild(60 + lsi.index).GetChild(3).gameObject.SetActive(true);
            }
            
        }

        if (lsi.category.Equals("mace"))
        {
            ItemTexts.transform.GetChild(68 + lsi.index).gameObject.SetActive(true);
            if (x == 0)
            {
                ItemTexts.transform.GetChild(68 + lsi.index).GetChild(2).gameObject.SetActive(true);
                ItemTexts.transform.GetChild(68 + lsi.index).GetChild(3).gameObject.SetActive(false);
            }
            else if (x == 1)
            {
                ItemTexts.transform.GetChild(68 + lsi.index).GetChild(2).gameObject.SetActive(false);
                ItemTexts.transform.GetChild(68 + lsi.index).GetChild(3).gameObject.SetActive(true);
            }
            
        }

        if (lsi.category.Equals("breastplate"))
        {
            ItemTexts.transform.GetChild(lsi.index).gameObject.SetActive(true);
            if (x == 0)
            {
                ItemTexts.transform.GetChild(lsi.index).GetChild(2).gameObject.SetActive(true);
                ItemTexts.transform.GetChild(lsi.index).GetChild(3).gameObject.SetActive(false);
            }
            else if (x == 1)
            {
                ItemTexts.transform.GetChild(lsi.index).GetChild(2).gameObject.SetActive(false);
                ItemTexts.transform.GetChild(lsi.index).GetChild(3).gameObject.SetActive(true);
            }
            
        }

        if (lsi.category.Equals("tozluk"))
        {
            ItemTexts.transform.GetChild(24 + lsi.index).gameObject.SetActive(true);
            if (x == 0)
            {
                ItemTexts.transform.GetChild(24 + lsi.index).GetChild(2).gameObject.SetActive(true);
                ItemTexts.transform.GetChild(24 + lsi.index).GetChild(3).gameObject.SetActive(false);
            }
            else if (x == 1)
            {
                ItemTexts.transform.GetChild(24 + lsi.index).GetChild(2).gameObject.SetActive(false);
                ItemTexts.transform.GetChild(24 + lsi.index).GetChild(3).gameObject.SetActive(true);
            }
            
        }

        if (lsi.category.Equals("shield"))
        {
            ItemTexts.transform.GetChild(36 + lsi.index).gameObject.SetActive(true);
            if (x == 0)
            {
                ItemTexts.transform.GetChild(36 + lsi.index).GetChild(2).gameObject.SetActive(true);
                ItemTexts.transform.GetChild(36 + lsi.index).GetChild(3).gameObject.SetActive(false);
            }
            else if (x == 1)
            {
                ItemTexts.transform.GetChild(36 + lsi.index).GetChild(2).gameObject.SetActive(false);
                ItemTexts.transform.GetChild(36 + lsi.index).GetChild(3).gameObject.SetActive(true);
            }
            
        }

        if (lsi.category.Equals("hardware"))
        {
            ItemTexts.transform.GetChild(88 + lsi.index).gameObject.SetActive(true);
            if (x == 0)
            {
                ItemTexts.transform.GetChild(88 + lsi.index).GetChild(1).gameObject.SetActive(true);
                ItemTexts.transform.GetChild(88 + lsi.index).GetChild(2).gameObject.SetActive(false);
            }
            else if (x == 1)
            {
                ItemTexts.transform.GetChild(88 + lsi.index).GetChild(1).gameObject.SetActive(false);
                ItemTexts.transform.GetChild(88 + lsi.index).GetChild(2).gameObject.SetActive(true);
            }
            
        }

        if (lsi.category.Equals("food"))
        {
            ItemTexts.transform.GetChild(76 + lsi.index).gameObject.SetActive(true);
            if (x == 0)
            {
                ItemTexts.transform.GetChild(76 + lsi.index).GetChild(2).gameObject.SetActive(true);
                ItemTexts.transform.GetChild(76 + lsi.index).GetChild(3).gameObject.SetActive(false);
            }
            else if (x == 1)
            {
                ItemTexts.transform.GetChild(76 + lsi.index).GetChild(2).gameObject.SetActive(false);
                ItemTexts.transform.GetChild(76 + lsi.index).GetChild(3).gameObject.SetActive(true);
            }
            
        }

        if (lsi.category.Equals("helmet"))
        {
            ItemTexts.transform.GetChild(12 + lsi.index).gameObject.SetActive(true);
            if (x == 0)
            {
                ItemTexts.transform.GetChild(12 + lsi.index).GetChild(2).gameObject.SetActive(true);
                ItemTexts.transform.GetChild(12 + lsi.index).GetChild(3).gameObject.SetActive(false);
            }
            else if (x == 1)
            {
                ItemTexts.transform.GetChild(12 + lsi.index).GetChild(2).gameObject.SetActive(false);
                ItemTexts.transform.GetChild(12 + lsi.index).GetChild(3).gameObject.SetActive(true);
            }
            
        }



    }

    void FoodSold(int index)
    {
        switch (index)
        {
            case 0:
                GameController.Instance.player_power -= 2;
                break;
            case 1:
                GameController.Instance.player_agility -= 2;
                break;
            case 2:
                GameController.Instance.player_vigor -= 2;
                break;
            case 3:
                GameController.Instance.player_power -= 4;
                break;
            case 4:
                GameController.Instance.player_agility -= 4;
                break;
            case 5:
                GameController.Instance.player_vigor -= 4;
                break;
            case 6:
                GameController.Instance.player_power -= 6;
                break;
            case 7:
                GameController.Instance.player_agility -= 6;
                break;
            case 8:
                GameController.Instance.player_vigor -= 6;
                break;
            case 9:
                GameController.Instance.player_power -= 8;
                break;
            case 10:
                GameController.Instance.player_agility -= 8;
                break;
            case 11:
                GameController.Instance.player_vigor -= 8;
                break;

        }

        characterMenuCtrl.GetComponent<CharacterMenuCtrl1>().UpdateSkills();
    }

    void HardwareSold(int index)
    {
        switch (index)
        {
            case 0:
                GameController.Instance.player_heal -= 10;
                break;
            case 1:
                GameController.Instance.player_sharpening -= 7;
                break;
            case 2:
                GameController.Instance.player_shieldHeal -= 10;
                break;
            case 3:
                GameController.Instance.playerEmber = false;
                GameController.Instance.player_weaponEmber -= 15;
                ps.UpdatePlayer();
                break;
            case 4:
                GameController.Instance.player_heal -= 25;
                break;
            case 5:
                GameController.Instance.player_sharpening -= 15;
                break;
            case 6:
                GameController.Instance.player_shieldHeal -= 25;
                break;
            case 7:
                GameController.Instance.playerEmber = false;
                GameController.Instance.player_weaponEmber -= 25;
                ps.UpdatePlayer();
                break;
        }

        characterMenuCtrl.GetComponent<CharacterMenuCtrl1>().UpdateSkills();
    }

    public void WearSword(int index)
    {
        if (GameController.Instance.firstGameOpen)
        {
            if (!(currentState.Equals("sword") && index == 0))
                return;

            currentState = "purchase";
            hints.transform.GetChild(2).gameObject.SetActive(false); // triangle
            hints.transform.GetChild(8).gameObject.SetActive(true); // triangle
            hints.transform.GetChild(3).GetComponent<Text>().text = "Now it is time to purchase that sword. You can do this by clicking purchase button. But please do not forget that when you give back any item that you just bought, sellers do not take them same price. They accept with lower price.";
            hints.transform.localPosition = new Vector3(-320, -261, 0);
        }

        lsi.category = "sword";
        lsi.index = index;
        CheckPurchased(lsi.category, lsi.index);
        ShowItemText(0);
        damageMultiplier.gameObject.SetActive(true);
        damageIcon.SetActive(true);

        if (GameController.Instance.player_level < Utility.RequiredLevelForTwelve(index))
        {
            availableText.text = "This item is avaliable after level " + Utility.RequiredLevelForTwelve(index).ToString();
            availableText.gameObject.SetActive(true);
            ism.Select(locked);
            purchaseBtn.gameObject.SetActive(false);
        }
        else
        {
            ism.Select(selectEffect);
        }
    }

    public void WearAxe(int index)
    {
        if (GameController.Instance.firstGameOpen)
            return;

        lsi.category = "axe";
        lsi.index = index;
        CheckPurchased(lsi.category, lsi.index);
        ShowItemText(0);
        damageMultiplier.gameObject.SetActive(true);
        damageIcon.SetActive(true);

        if (GameController.Instance.player_level < Utility.RequiredLevelForEight(index))
        {
            availableText.text = "This item is avaliable after level " + Utility.RequiredLevelForEight(index).ToString();
            availableText.gameObject.SetActive(true);
            ism.Select(locked);
            purchaseBtn.gameObject.SetActive(false);
        }
        else
        {
            ism.Select(selectEffect);
        }
    }

    public void WearMace(int index)
    {
        if (GameController.Instance.firstGameOpen)
            return;

        lsi.category = "mace";
        lsi.index = index;
        CheckPurchased(lsi.category, lsi.index);
        ShowItemText(0);
        damageMultiplier.gameObject.SetActive(true);
        damageIcon.SetActive(true);

        if (GameController.Instance.player_level < Utility.RequiredLevelForEight(index))
        {
            availableText.text = "This item is avaliable after level " + Utility.RequiredLevelForEight(index).ToString();
            availableText.gameObject.SetActive(true);
            ism.Select(locked);
            purchaseBtn.gameObject.SetActive(false);
        }
        else
        {
            ism.Select(selectEffect);
        }
    }

    public void Purchase()
    {
        if (itemList.Count < 24)
        {
            if (Price.GetPrice(lsi.category, lsi.index) <= GameController.Instance.player_total_money)
            {
                if (GameController.Instance.firstGameOpen)
                {
                    if (!currentState.Equals("purchase"))
                        return;

                    currentState = "choose";
                    hints.transform.GetChild(8).gameObject.SetActive(false); // triangle
                    hints.transform.GetChild(9).gameObject.SetActive(true); // triangle
                    hints.transform.GetChild(3).GetComponent<Text>().text = "Well done ! You have just bought a simple sword. Hope it would help you to deal with enemies. Here is your envanter that you are going to keep your item in your entire game. To use this, just click the item you want to use.";
                    hints.transform.localPosition = new Vector3(-376, -7, 0);
                }

                GameController.Instance.player_total_money -= Price.GetPrice(lsi.category, lsi.index);
                UpdateYourMoney();

                ism.PurchaseOrSell(coinShake);

                itemList.Add(new Item(lsi.category, lsi.index, 0));
                slot.AddEnvanter(itemList);

                purchaseBtn.gameObject.SetActive(false);
                purchased_txt.gameObject.SetActive(true);
            }
            else
            {
                message.text = "You do not have enough money for this item !";
                message.gameObject.SetActive(true);
                ism.PlaySingle(emptyPage);
            }
        }
        else
        {
            message.text = "There is no enough space !";
            message.gameObject.SetActive(true);
            ism.PlaySingle(emptyPage);
        }

        SaveSystem.SavePlayer();
        //characterMenuCtrl.GetComponent<CharacterMenuCtrl1>().SaveToCloud();
    }

    public void LastInventoryTick(int x) // bos oldugunda bug oluyo
    {
        if (GameController.Instance.firstGameOpen)
        {
            if(!(currentState.Equals("choose") && x == 0))
                return;

            currentState = "use";
            hints.transform.GetChild(3).GetComponent<Text>().text = "Now it is enough to click use button.";
            hints.transform.localPosition = new Vector3(-395, -230, 0);
        }

        ResetIcons();

        lastInventoryTick = invPage * 8 + x;

        try
        {
            itemList[lastInventoryTick].GetCategory();
        }
        catch(Exception ex)
        {
            TickEmpty(0);
            return;
        }

        lsi.category = itemList[lastInventoryTick].GetCategory();
        lsi.index = itemList[lastInventoryTick].GetIndex();

        ShowItemText(1);

        sellBtn.gameObject.SetActive(true);
        useBtn.gameObject.SetActive(true);

        purchaseBtn.gameObject.SetActive(false);
        purchased_txt.gameObject.SetActive(false);

        foreach (Transform child in greenBoxParent.transform)
            child.gameObject.SetActive(false);

        greenBoxParent.transform.GetChild(x).gameObject.SetActive(true);

        if (lsi.category.Equals("sword") || lsi.category.Equals("mace") || lsi.category.Equals("axe"))
        {
            damageIcon.SetActive(true);
            damageMultiplier.gameObject.SetActive(true);
        }

        if (lsi.category.Equals("breastplate") || lsi.category.Equals("helmet") || lsi.category.Equals("tozluk"))
        {
            armorMultiplier.gameObject.SetActive(true);
            armourIcon.SetActive(true);
        }

        if (lsi.category.Equals("shield"))
        {
            shieldMultiplier.gameObject.SetActive(true);
            shieldIcon.SetActive(true);
        }

        if (lsi.category.Equals("food") || lsi.category.Equals("hardware"))
        {
            float percentage = 1f - (float)itemList[lastInventoryTick].GetTotalSeconds() / 900;
            leftAmount.text = "Left Amount : " + (int)(percentage * 100) + " %";

            leftAmount.gameObject.SetActive(true);
            useBtn.gameObject.SetActive(false);
        }

        ism.Select(selectEffect);
    }

    public void Sell()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        ResetIcons();

        if (lsi.category.Equals("sword") && lsi.index == 0 || lsi.category.Equals("breastplate") && lsi.index == 0 || lsi.category.Equals("shield") && lsi.index == 0)
        {
            leftAmount.text = "This item cannot be sold, you have to keep it ";
            leftAmount.gameObject.SetActive(true);
            ism.PlaySingle(emptyPage);
            return;
        }

        if (lsi.category.Equals("sword") && GameController.Instance.player_sword == itemList[lastInventoryTick].GetIndex())
            ps.ResetAllWeapons();

        if (lsi.category.Equals("axe") && GameController.Instance.player_axe == itemList[lastInventoryTick].GetIndex())
            ps.ResetAllWeapons();

        if (lsi.category.Equals("mace") && GameController.Instance.player_mace == itemList[lastInventoryTick].GetIndex())
            ps.ResetAllWeapons();

        if (lsi.category.Equals("breastplate") && GameController.Instance.player_breastPlate - 1 == itemList[lastInventoryTick].GetIndex())
            ps.ResetBreastplate();

        if (lsi.category.Equals("tozluk") && GameController.Instance.player_tozluk - 1 == itemList[lastInventoryTick].GetIndex())
            ps.ResetTozluk();

        if (lsi.category.Equals("helmet") && GameController.Instance.player_helmet - 1 == itemList[lastInventoryTick].GetIndex())
        {
            ps.ResetHelmet();
            ps.SetHair();
        }

        if (lsi.category.Equals("shield") && GameController.Instance.player_shield == itemList[lastInventoryTick].GetIndex())
            ps.ResetShield();

        if (lsi.category.Equals("food"))
            FoodSold(lsi.index);

        if (lsi.category.Equals("hardware"))
            HardwareSold(lsi.index);

        itemList.RemoveAt(lastInventoryTick);
        slot.AddEnvanter(itemList);

        GameController.Instance.player_total_money += Price.GetSellingPrice(lsi.category, lsi.index);
        UpdateYourMoney();

        ItemTexts.gameObject.SetActive(false);

        sellBtn.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(false);

        

        foreach (Transform child in greenBoxParent.transform)
            child.gameObject.SetActive(false);

        ism.PurchaseOrSell(coinShake);

        SaveSystem.SavePlayer();
        //characterMenuCtrl.GetComponent<CharacterMenuCtrl1>().SaveToCloud();
    }

    public void InventoryUp()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        if (invPage > 0)
        {
            invPage--;
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        ConfigureScrollBarPos();

        slot.ChangePage(invPage);

        sellBtn.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(false);

        foreach (Transform child in greenBoxParent.transform)
            child.gameObject.SetActive(false);

        TickEmpty(0);
    }
    public void InventoryDown()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        if (invPage < 2)
        {
            invPage++;
            ism.ChangePage(changePage);
        }
        else
            ism.PlaySingle(emptyPage);

        ConfigureScrollBarPos();

        slot.ChangePage(invPage);

        sellBtn.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(false);

        foreach (Transform child in greenBoxParent.transform)
            child.gameObject.SetActive(false);

        TickEmpty(0);
    }

    void CheckPurchased(string category, int index)
    {
        lastInventoryTick = -1;

        foreach (Transform child in greenBoxParent.transform)
            child.gameObject.SetActive(false);

        sellBtn.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(false);

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].GetCategory().Equals(category) && itemList[i].GetIndex() == index)
            {
                purchased_txt.gameObject.SetActive(true);
                purchaseBtn.gameObject.SetActive(false);
                return;
            }
        }
        purchaseBtn.gameObject.SetActive(true);
        purchased_txt.gameObject.SetActive(false);

    }

    public void Use()
    {
        if (GameController.Instance.firstGameOpen)
        {
            if (!currentState.Equals("use"))
                return;

            currentState = "blacksmith";
            hints.transform.GetChild(9).gameObject.SetActive(false);
            hints.transform.GetChild(7).gameObject.SetActive(true);
            hints.transform.GetChild(3).GetComponent<Text>().text = "Yes, now you are able to give the thing that enemies deserve. Pain... But we are not done yet. As you see our character is naked, he needs to have some armour. To do this, let's go to blacksmith.";
            hints.transform.localPosition = new Vector3(-515, -68, 0);
        }

        if (lsi.category.Equals("sword") || lsi.category.Equals("mace") || lsi.category.Equals("axe"))
            ps.ResetAllWeapons();

        if (itemList[lastInventoryTick].GetCategory().Equals("sword"))
        {
            GameController.Instance.player_sword = itemList[lastInventoryTick].GetIndex();
            ism.PlaySingle(equip);
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("axe"))
        {
            GameController.Instance.player_axe = itemList[lastInventoryTick].GetIndex();
            ism.PlaySingle(equip);
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("mace"))
        {
            GameController.Instance.player_mace = itemList[lastInventoryTick].GetIndex();
            ism.PlaySingle(equip);
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("breastplate"))
        {
            GameController.Instance.player_breastPlate = itemList[lastInventoryTick].GetIndex() + 1;
            ism.PlaySingle(equip);
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("helmet"))
        {
            GameController.Instance.player_helmet = itemList[lastInventoryTick].GetIndex() + 1;
            ism.PlaySingle(equip);
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("tozluk"))
        {
            GameController.Instance.player_tozluk = itemList[lastInventoryTick].GetIndex() + 1;
            ism.PlaySingle(equip);
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("shield"))
        {
            GameController.Instance.player_shield = itemList[lastInventoryTick].GetIndex();
            ism.PlaySingle(equip);
            ps.UpdatePlayer();
        }

        foreach (Transform child in greenBoxParent.transform)
            child.gameObject.SetActive(false);

        sellBtn.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(false);

        SaveSystem.SavePlayer();
        //characterMenuCtrl.GetComponent<CharacterMenuCtrl1>().SaveToCloud();
    }

    void UpdateYourMoney()
    {
        int moneyAmount = GameController.Instance.player_total_money;
        yourMoney.text = "Your Money: " + moneyAmount.ToString();
    }

    void ResetIcons()
    {
        yourMoneyIcon.SetActive(false);
        sellingPriceIcon.SetActive(false);
        damageIcon.SetActive(false);
        shieldIcon.SetActive(false);
        armourIcon.SetActive(false);
    }

    public void TickEmpty(int x)
    {
        if(x == 1)
            if (GameController.Instance.firstGameOpen)
                return;

        yourMoneyIcon.SetActive(false);
        sellingPriceIcon.SetActive(false);
        damageIcon.SetActive(false);
        shieldIcon.SetActive(false);
        armourIcon.SetActive(false);

        leftAmount.gameObject.SetActive(false);
        message.gameObject.SetActive(false);
        yourMoney.gameObject.SetActive(false);
        sellBtn.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(false);
        ItemTexts.gameObject.SetActive(false);
        damageMultiplier.gameObject.SetActive(false);
        armorMultiplier.gameObject.SetActive(false);
        shieldMultiplier.gameObject.SetActive(false);
        purchased_txt.gameObject.SetActive(false);
        purchaseBtn.gameObject.SetActive(false);

        foreach (Transform child in greenBoxParent.transform)
            child.gameObject.SetActive(false);
    }

    void ConfigureScrollBarPos()
    {
        switch (invPage)
        {
            case 0:
                scrollBar.transform.GetChild(0).gameObject.SetActive(true);
                scrollBar.transform.GetChild(1).gameObject.SetActive(false);
                scrollBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                scrollBar.transform.GetChild(1).gameObject.SetActive(true);
                scrollBar.transform.GetChild(0).gameObject.SetActive(false);
                scrollBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 2:
                scrollBar.transform.GetChild(2).gameObject.SetActive(true);
                scrollBar.transform.GetChild(1).gameObject.SetActive(false);
                scrollBar.transform.GetChild(0).gameObject.SetActive(false);
                break;

        }
    }

    public void IncreaseSizeOfButton(int index)
    {

        allButtons[index].transform.localScale = startScales[index] * 1.2f;
    }

    public void DecreaseSizeOfButton(int index)
    {
        allButtons[index].transform.localScale = startScales[index];
    }

    void InitStartScales()
    {
        startScales = new Vector3[allButtons.Length];

        for (int i = 0; i < allButtons.Length; i++)
        {
            startScales[i] = allButtons[i].transform.localScale;
        }
    }

}



