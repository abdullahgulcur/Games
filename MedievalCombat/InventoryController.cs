using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    int lastInventoryTick;
    List<Item> itemList;
    public GameObject ItemTexts;
    public GameObject player;
    public GameObject arm;
    public GameObject arm1;

    PlayerArm armScript;
    PlayerArm armScript1;

    PlayerState ps;

    struct LastSelectedItem
    {
        public string category;
        public int index;
    };

    LastSelectedItem lsi;

    void Start()
    {
        ps = player.GetComponent<PlayerState>();
        lsi = new LastSelectedItem();
        itemList = GameController.Instance.itemList;
    }

    public void LastInventoryTick(int x)
    {
        lastInventoryTick = x;

        lsi.category = itemList[lastInventoryTick].GetCategory();
        lsi.index = itemList[lastInventoryTick].GetIndex();

        ShowItemText();
    }

    void ShowItemText()
    {
        foreach (Transform child in ItemTexts.transform)
            child.gameObject.SetActive(false);

        if (lsi.category.Equals("sword"))
        {
            ItemTexts.transform.GetChild(48 + lsi.index).gameObject.SetActive(true);
        }

        if (lsi.category.Equals("axe"))
        {
            ItemTexts.transform.GetChild(60 + lsi.index).gameObject.SetActive(true);
        }

        if (lsi.category.Equals("mace"))
        {
            ItemTexts.transform.GetChild(68 + lsi.index).gameObject.SetActive(true);
        }

        if (lsi.category.Equals("breastplate"))
        {
            ItemTexts.transform.GetChild(lsi.index).gameObject.SetActive(true);
        }

        if (lsi.category.Equals("tozluk"))
        {
            ItemTexts.transform.GetChild(24 + lsi.index).gameObject.SetActive(true);
        }

        if (lsi.category.Equals("shield"))
        {
            ItemTexts.transform.GetChild(36 + lsi.index).gameObject.SetActive(true);
        }

        if (lsi.category.Equals("hardware"))
        {
            ItemTexts.transform.GetChild(88 + lsi.index).gameObject.SetActive(true);
        }

        if (lsi.category.Equals("food"))
        {
            ItemTexts.transform.GetChild(76 + lsi.index).gameObject.SetActive(true);
        }

        if (lsi.category.Equals("helmet"))
        {
            ItemTexts.transform.GetChild(12 + lsi.index).gameObject.SetActive(true);
        }
        
    }

    public void Use()
    {
        ps.ResetAllWeapons();

        if (itemList[lastInventoryTick].GetCategory().Equals("sword"))
        {
            GameController.Instance.player_sword = itemList[lastInventoryTick].GetIndex();
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("axe"))
        {
            GameController.Instance.player_axe = itemList[lastInventoryTick].GetIndex();
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("mace"))
        {
            GameController.Instance.player_mace = itemList[lastInventoryTick].GetIndex();
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("breastplate"))
        {
            GameController.Instance.player_breastPlate = itemList[lastInventoryTick].GetIndex() + 1;

            armScript.ChangeArmSprite(itemList[lastInventoryTick].GetIndex() + 1);
            armScript1.ChangeArmSprite(itemList[lastInventoryTick].GetIndex() + 1);

            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("helmet"))
        {
            GameController.Instance.player_helmet = itemList[lastInventoryTick].GetIndex() + 1;
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("tozluk"))
        {
            GameController.Instance.player_tozluk = itemList[lastInventoryTick].GetIndex() + 1;
            ps.UpdatePlayer();
        }

        if (itemList[lastInventoryTick].GetCategory().Equals("shield"))
        {
            GameController.Instance.player_shield = itemList[lastInventoryTick].GetIndex();
            ps.UpdatePlayer();
        }
    }






}
