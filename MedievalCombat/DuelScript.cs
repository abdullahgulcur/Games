using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class DuelScript : MonoBehaviour {

    public Image loadAmount;
    public Text percentageText;

    public Text enemy_armourText;

    public Text enemy_powerText;
    public Text enemy_agilityText;
    public Text enemy_staminaText;
    public Text enemy_defenceText;
    public Text enemy_attackText;
    public Text enemy_vigorText;

    public Text player_totalHealth;
    public Text player_powerText;
    public Text player_agilityText;
    public Text player_staminaText;
    public Text player_defenceText;
    public Text player_attackText;
    public Text player_vigorText;


    void Start()
    {
        loadAmount.fillAmount = (GameController.Instance.totalEnemyKilled % 4) / 4f;
        int percentage = (int)(((GameController.Instance.totalEnemyKilled % 4) / 4f) * 100);
        percentageText.text = "%" + percentage.ToString();

        SetPlayerStartHealth();
        SetPlayerShieldStartHealth();
      //  SetEnemySkills();
        SetPlayerSkills();
    }
    /*
    public void SetEnemyTotalArmour()
    {
        enemy_armourText.text = GameController.Instance.enemy_total_armour.ToString();
    }

    public void SetEnemySkills()
    {
        enemy_powerText.text = GameController.Instance.enemy_power.ToString();
        enemy_agilityText.text = GameController.Instance.enemy_agility.ToString();
        enemy_staminaText.text = GameController.Instance.enemy_stamina.ToString();
        enemy_defenceText.text = GameController.Instance.enemy_defence.ToString();
        enemy_attackText.text = GameController.Instance.enemy_attack.ToString();
        enemy_vigorText.text = GameController.Instance.enemy_vigor.ToString();
        
    }*/

    void SetPlayerSkills()
    {
        player_powerText.text = GameController.Instance.player_power.ToString();
        player_agilityText.text = GameController.Instance.player_agility.ToString();
        player_defenceText.text = GameController.Instance.player_defence.ToString();
        player_vigorText.text = GameController.Instance.player_vigor.ToString();
        player_totalHealth.text = GameController.Instance.player_health.ToString();
    }

    void SetPlayerStartHealth()
    {
        int healthStart = 50; // baslangic sagligi 50 olabilir
        int total = GameController.Instance.player_breastPlate * 50 +
            GameController.Instance.player_helmet * 30 + GameController.Instance.player_tozluk * 15+
            healthStart + GameController.Instance.player_vigor * 10;
        GameController.Instance.player_health = total;
    }

    void SetPlayerShieldStartHealth()
    {
        int shield_health = 50 + GameController.Instance.player_shield * 20;
        GameController.Instance.player_shield_health = shield_health;
    }

    public void PressArena()
    {
        SceneManager.LoadScene("Arena");
    }

    public void PressInterface()
    {
        SceneManager.LoadScene("Interface");
    }

    public void PressChange()
    {
        
    }
    

}
