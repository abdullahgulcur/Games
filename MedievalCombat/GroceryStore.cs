using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GroceryStore : MonoBehaviour {

    public AudioClip coinShake;
    public AudioClip equip;
    public AudioClip changePage;
    public AudioClip emptyPage;
    public AudioClip selectEffect;
    public AudioClip locked;

    public GameObject[] Food_Group;
    public GameObject[] Hardware_Group;

    public Button sellBtn;
    public Button useBtn;
    public Button purchaseBtn;

    List<Item> itemList;

    public GameObject player;
    PlayerState ps;

    int foodPage;
    int hardwarePage;

    int categoryIndex = 0;

    public GameObject paper;

    GameObject envanter;
    Slot slot;

    int lastInventoryTick = - 1;

    int invPage = 0;

    GameObject scrollBar;

    struct LastSelectedItem
    {
        public string category;
        public int index;
    };

    LastSelectedItem lsi;

    public GameObject ItemTexts;

    GameObject greenBoxParent;

    public Text damageMultiplier;
    public Text armorMultiplier;
    public Text shieldMultiplier;

    public Text price;
    public Text sellingPrice;

    public Text yourMoney;

    public Text purchased_txt;

    public Text message;
    public Text availableText;
    public Text leftAmount;

    public GameObject[] scenes;

    public Button[] allButtons;
    Vector3[] startScales;

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

        SetLockersInGrocery();
        InitStartScales();
        envanter = paper.transform.GetChild(6).gameObject;
        scrollBar = paper.transform.GetChild(8).gameObject;
        greenBoxParent = paper.transform.GetChild(5).gameObject;
        diffSlots = paper.transform.GetChild(4).gameObject;

        ps = player.GetComponent<PlayerState>();
        slot = envanter.GetComponent<Slot>();
        itemList = GameController.Instance.itemList;

        
    }

    public void SetHintsInactive()
    {
        if (!GameController.Instance.firstGameOpen)
            hints.SetActive(false);
    }

    public void SetLockersInGrocery()
    {
        for (int i = 0; i < 12; i++)
        {
            if (GameController.Instance.player_level < Utility.RequiredLevelForTwelve(i))
            {
                if (i % 4 == 0)
                    Food_Group[i / 4].transform.GetChild(3).gameObject.SetActive(true);
                else if (i % 4 == 1)
                    Food_Group[i / 4].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                else if (i % 4 == 2)
                    Food_Group[i / 4].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                else if (i % 4 == 3)
                    Food_Group[i / 4].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                if (i % 4 == 0)
                    Food_Group[i / 4].transform.GetChild(3).gameObject.SetActive(false);
                else if (i % 4 == 1)
                    Food_Group[i / 4].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                else if (i % 4 == 2)
                    Food_Group[i / 4].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                else if (i % 4 == 3)
                    Food_Group[i / 4].transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < 8; i++)
        {
            if (GameController.Instance.player_level < Utility.RequiredLevelForEight(i))
            {
                if (i % 4 == 0)
                    Hardware_Group[i / 4].transform.GetChild(3).gameObject.SetActive(true);
                else if (i % 4 == 1)
                    Hardware_Group[i / 4].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                else if (i % 4 == 2)
                    Hardware_Group[i / 4].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                else if (i % 4 == 3)
                    Hardware_Group[i / 4].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                if (i % 4 == 0)
                    Hardware_Group[i / 4].transform.GetChild(3).gameObject.SetActive(false);
                else if (i % 4 == 1)
                    Hardware_Group[i / 4].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                else if (i % 4 == 2)
                    Hardware_Group[i / 4].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                else if (i % 4 == 3)
                    Hardware_Group[i / 4].transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void LoadSceneWithIndex(int x)// baslangic scene 0
    {
        if (GameController.Instance.firstGameOpen)
            if (!(currentState.Equals("map") && x == 4))
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
            slots.gameObject.SetActive(false);
            pattern_0.SetActive(true);
            pattern_1.SetActive(false);

            envanter.gameObject.SetActive(true);
            scrollBar.gameObject.SetActive(true);
            greenBoxParent.gameObject.SetActive(true);
            diffSlots.gameObject.SetActive(true);
        }

        if(x == 4)
        {
            if (GameController.Instance.firstGameOpen)
                hints.SetActive(false);

            fadeSprite.GetComponent<Animator>().SetInteger("State", 1);
            eventSystem.SetActive(false);
            StartCoroutine(ResetFade());

            if (GameController.Instance.firstGameOpen)
                hints.SetActive(false);
        }

        ism.ChangePage(changePage);
    }

    IEnumerator ResetFade()
    {
        yield return new WaitForSeconds(0.5f);

        player.gameObject.SetActive(false);
        paper.gameObject.SetActive(false);
        scenes[3].gameObject.SetActive(false);
        scenes[4].gameObject.SetActive(true);
        map0.gameObject.SetActive(true);
        mapBtn0.gameObject.SetActive(true);
        mapStuff.gameObject.SetActive(true);
        fadeSprite.GetComponent<Animator>().SetInteger("State", 0);
        levelController.GetComponent<LevelController>().SetLevelBtnColor();
        yield return new WaitForSeconds(0.5f);
        eventSystem.SetActive(true);
    }

    public void Skip()
    {
        if (clickedSkipCount == 0)
        {
            hints.transform.GetChild(3).GetComponent<Text>().text = "Next time you come here, do not forget to purchase some food and bandage. Bandages increase healing speed of wounds.";
            clickedSkipCount++;
            ism.Select(selectEffect);
            return;
        }

        if (clickedSkipCount == 1)
        {
            currentState = "map";
            hints.transform.GetChild(0).gameObject.SetActive(false);
            hints.transform.GetChild(1).gameObject.SetActive(true);
            hints.transform.GetChild(3).GetComponent<Text>().text = "I think we are done here, too. Let's visit island of Apotolia which you will take over from enemies...";
            hints.transform.localPosition = new Vector3(108, -51, 0);
            hints.transform.GetChild(7).gameObject.SetActive(true); // triangle
            hints.transform.GetChild(4).gameObject.SetActive(false); // skip button
            ism.Select(selectEffect);
            clickedSkipCount++;
            return;
        }
    }

    void ShowItemText(int x) // x == 0 price ----- x == 1 selling price
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
            if (x == 0)
            {
                ItemTexts.transform.GetChild(48 + lsi.index).GetChild(2).gameObject.SetActive(true);
                ItemTexts.transform.GetChild(48 + lsi.index).GetChild(3).gameObject.SetActive(false);
            }
            else if (x == 1)
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

    void HardwarePurcashed()
    {


        if (lsi.category.Equals("hardware"))
        {
            switch (lsi.index)
            {
                case 0:
                    GameController.Instance.player_heal += 10;
                    break;
                case 1:
                    GameController.Instance.player_sharpening += 7;
                    break;
                case 2:
                    GameController.Instance.player_shieldHeal += 10;
                    break;
                case 3:
                    GameController.Instance.playerEmber = true;
                    GameController.Instance.player_weaponEmber += 15;
                    ps.UpdatePlayer();
                    break;
                case 4:
                    GameController.Instance.player_heal += 25;
                    break;
                case 5:
                    GameController.Instance.player_sharpening += 15;
                    break;
                case 6:
                    GameController.Instance.player_shieldHeal += 25;
                    break;
                case 7:
                    GameController.Instance.playerEmber = true;
                    GameController.Instance.player_weaponEmber += 25;
                    ps.UpdatePlayer();
                    break;
            }
        }

        purchaseBtn.gameObject.SetActive(false);
        purchased_txt.gameObject.SetActive(true);

        characterMenuCtrl.GetComponent<CharacterMenuCtrl1>().UpdateSkills();
    }

    void FoodPurcashed()
    {


        if (lsi.category.Equals("food"))
        {
            switch (lsi.index)
            {
                case 0:
                    GameController.Instance.player_power += 2;
                    break;
                case 1:
                    GameController.Instance.player_agility += 2;
                    break;
                case 2:
                    GameController.Instance.player_vigor += 2;
                    break;
                case 3:
                    GameController.Instance.player_power += 4;
                    break;
                case 4:
                    GameController.Instance.player_agility += 4;
                    break;
                case 5:
                    GameController.Instance.player_vigor += 4;
                    break;
                case 6:
                    GameController.Instance.player_power += 6;
                    break;
                case 7:
                    GameController.Instance.player_agility += 6;
                    break;
                case 8:
                    GameController.Instance.player_vigor += 6;
                    break;
                case 9:
                    GameController.Instance.player_power += 8;
                    break;
                case 10:
                    GameController.Instance.player_agility += 8;
                    break;
                case 11:
                    GameController.Instance.player_vigor += 8;
                    break;

            }

            purchaseBtn.gameObject.SetActive(false);
            purchased_txt.gameObject.SetActive(true);

            characterMenuCtrl.GetComponent<CharacterMenuCtrl1>().UpdateSkills();

        }
    }

    public void Purchase()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        if (itemList.Count < 24)
        {
            if (Price.GetPrice(lsi.category, lsi.index) <= GameController.Instance.player_total_money)
            {
                GameController.Instance.player_total_money -= Price.GetPrice(lsi.category, lsi.index);
                UpdateYourMoney();

                ism.PlaySingle(coinShake);

                itemList.Add(new Item(lsi.category, lsi.index, 0));
                slot.AddEnvanter(itemList);

                FoodPurcashed(); // food almissa eger
                HardwarePurcashed();
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

    void UpdateYourMoney()
    {
        int moneyAmount = GameController.Instance.player_total_money;
        yourMoney.text = "Your Money: " + moneyAmount.ToString();
    }

    public void BuyHardware(int index)
    {
        if (GameController.Instance.firstGameOpen)
            return;

        ResetIcons();

        lsi.category = "hardware";
        lsi.index = index;
        CheckPurchased(lsi.category, lsi.index);
        ShowItemText(0);

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

    public void BuyFood(int index)
    {
        if (GameController.Instance.firstGameOpen)
            return;

        ResetIcons();

        lsi.category = "food";
        lsi.index = index;
        CheckPurchased(lsi.category, lsi.index);
        ShowItemText(0);

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

    public void LastInventoryTick(int x)
    {
        if (GameController.Instance.firstGameOpen)
            return;

        ResetIcons();

        lastInventoryTick = invPage * 8 + x;

        try
        {
            itemList[lastInventoryTick].GetCategory();
        }
        catch (Exception ex)
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

    public void RemoveAtIndex(string category, int index, int inventoryIndex)
    {
        if (category.Equals("food"))
            FoodSold(index);

        if (category.Equals("hardware"))
            HardwareSold(index);


        itemList.RemoveAt(inventoryIndex);

        SaveSystem.SavePlayer();
        //characterMenuCtrl.GetComponent<CharacterMenuCtrl1>().SaveToCloud();
        //  slot.AddEnvanter();

        //ItemTexts.gameObject.SetActive(false);

        // sellBtn.gameObject.SetActive(false);
        //useBtn.gameObject.SetActive(false);

        //foreach (Transform child in greenBoxParent.transform)
        //  child.gameObject.SetActive(false);
    }

    public void InventoryUp()
    {
        if (GameController.Instance.firstGameOpen)
            return;

        if (invPage > 0)
        {
            invPage--;
            ism.ChangePage(changePage);
        }else
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
        }else
            ism.PlaySingle(emptyPage);


        ConfigureScrollBarPos();

        slot.ChangePage(invPage);

        sellBtn.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(false);

        foreach (Transform child in greenBoxParent.transform)
            child.gameObject.SetActive(false);

        TickEmpty(0);
    }

    public void Use()
    {
        if (GameController.Instance.firstGameOpen)
            return;

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
        if (x == 1)
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
                    if (foodPage < 2)
                    {
                        foodPage++;
                        ism.ChangePage(changePage);
                    }
                    else
                        ism.PlaySingle(emptyPage);

                }
                else
                {
                    if (foodPage > 0)
                    {
                        foodPage--;
                        ism.ChangePage(changePage);
                    }
                    else
                        ism.PlaySingle(emptyPage);
                }

                for (int i = 0; i < Food_Group.Length; i++)
                {
                    if (i == foodPage)
                        continue;
                    else
                        Food_Group[i].SetActive(false);
                }
                Food_Group[foodPage].SetActive(true);
                break;

            case 1:
                if (x == -1)
                {
                    if (hardwarePage < 1)
                    {
                        hardwarePage++;
                        ism.ChangePage(changePage);
                    }
                    else
                        ism.PlaySingle(emptyPage);
                }
                else
                {
                    if (hardwarePage > 0)
                    {
                        hardwarePage--;
                        ism.ChangePage(changePage);
                    }
                    else
                        ism.PlaySingle(emptyPage);
                }

                for (int i = 0; i < Hardware_Group.Length; i++)
                {
                    if (i == hardwarePage)
                        continue;
                    else
                        Hardware_Group[i].SetActive(false);
                }
                Hardware_Group[hardwarePage].SetActive(true);
                break;
        }

        
        
    }

    public void ChangeCategory(int x)
    {
        if (GameController.Instance.firstGameOpen)
            return;

        categoryIndex = x;
        foodPage = 0;
        hardwarePage = 0;

        for (int i = 0; i < Food_Group.Length; i++)
        {
            Food_Group[i].SetActive(false);
        }
        for (int i = 0; i < Hardware_Group.Length; i++)
        {
            Hardware_Group[i].SetActive(false);
        }

        switch (categoryIndex)
        {
            case 0:
                Food_Group[0].SetActive(true);
                break;
            case 1:
                Hardware_Group[0].SetActive(true);
                break;

        }

        TickEmpty(0);

        ism.ChangePage(changePage);
    }

    public void PressReturnInterface()
    {
        SceneManager.LoadScene("Interface");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
