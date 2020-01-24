using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData{

    public int OpenableMapIndex;
    public bool gameFinished;
    public bool firstGameOpen;

    public int player_total_money;

    public int player_heal;
    public int player_sharpening;
    public int player_shieldHeal;
    public int player_weaponEmber;

    public int player_health;
    public int player_shield_health;

    public int player_haircolor_index;
    public int player_haircut;
    public int player_beard;
    public int player_moustache;

    public int player_helmet;
    public int player_tozluk;
    public int player_breastPlate;

    public int player_level;

    public int player_vigor;
    public int player_power;
    public int player_agility;
    public int player_defence;

    public int skillpoints_left;

    public int player_sword = -1;
    public int player_mace = -1;
    public int player_axe = -1;

    public int player_shield = -1;

    public bool playerEmber = false;
    public bool hotParticles = false;


    public string[] category = new string[GameController.Instance.itemList.Count];
    public int[] index = new int[GameController.Instance.itemList.Count];
    public int[] totalSeconds = new int[GameController.Instance.itemList.Count];
    


    public PlayerData()
    {
        OpenableMapIndex = GameController.Instance.OpenableMapIndex;
        gameFinished = GameController.Instance.gameFinished;
        firstGameOpen = GameController.Instance.firstGameOpen;
        player_total_money = GameController.Instance.player_total_money;
        player_heal = GameController.Instance.player_heal;
        player_sharpening = GameController.Instance.player_sharpening;
        player_shieldHeal = GameController.Instance.player_shieldHeal;
        player_weaponEmber = GameController.Instance.player_weaponEmber;
        player_health = GameController.Instance.player_health;
        player_shield_health = GameController.Instance.player_shield_health;
        player_haircolor_index = GameController.Instance.player_haircolor_index;
        player_haircut = GameController.Instance.player_haircut;
        player_beard = GameController.Instance.player_beard;
        player_moustache = GameController.Instance.player_moustache;
        player_breastPlate = GameController.Instance.player_breastPlate;
        player_helmet = GameController.Instance.player_helmet;
        player_tozluk = GameController.Instance.player_tozluk;
        player_level = GameController.Instance.player_level;
        player_vigor = GameController.Instance.player_vigor;
        player_power = GameController.Instance.player_power;
        player_agility = GameController.Instance.player_agility;
        player_defence = GameController.Instance.player_defence;
        skillpoints_left = GameController.Instance.skillpoints_left;
        player_sword = GameController.Instance.player_sword;
        player_mace = GameController.Instance.player_mace;
        player_axe = GameController.Instance.player_axe;
        player_shield = GameController.Instance.player_shield;
        playerEmber = GameController.Instance.playerEmber;
        hotParticles = GameController.Instance.hotParticles;

        for(int i = 0; i < category.Length; i++)
        {
            category[i] = GameController.Instance.itemList[i].GetCategory();
        }

        for (int i = 0; i < index.Length; i++)
        {
            index[i] = GameController.Instance.itemList[i].GetIndex();
        }

        for (int i = 0; i < totalSeconds.Length; i++)
        {
            totalSeconds[i] = GameController.Instance.itemList[i].GetTotalSeconds();
        }


    }

    
}
