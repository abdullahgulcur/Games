using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public AudioClip shieldBrokeWood;
    public AudioClip shieldBrokeIron;

    public ParticleSystem shieldParticles;
    public GameObject[] shields;

    public Button defenseBtn;

    public int startHealth;
    public int breastPlatePower;
    public int helmetPower;
    public int tozlukPower;
    public int healthAmountPerSkill;

    public int startShield;

    int total_health;
    int shield_health;

    int start_health;
    int start_shieldHealth;

    Player ps;

    float startHealInTimes = 0.3f;
    float startShieldHealInTimes = 0.25f;

    void Start () {
        
        InvokeRepeating("IncreaseHealth", 1, GetHeal()); // InvokeRepeating("functionName", startTime, inEverySeconds);
        InvokeRepeating("IncreaseShieldHealth", 1, GetShieldHeal());

        ps = GetComponent<Player>();

        total_health = start_health = startHealth + GameController.Instance.player_breastPlate * breastPlatePower +
            GameController.Instance.player_tozluk * tozlukPower + 
            GameController.Instance.player_helmet * helmetPower + 
            GameController.Instance.player_vigor * healthAmountPerSkill;




        if (GameController.Instance.player_shield != -1)//startShield * System.Math.Pow(1.2f, GameController.Instance.player_shield + 1)
            shield_health = start_shieldHealth = Utility.GetShieldHealth(GameController.Instance.player_shield, GameController.Instance.player_power, startShield);
        else
            shield_health = 0;

    }

    float GetHeal()
    {
        //return startHealInTimes - startHealInTimes * (GameController.Instance.player_heal / 100f);

        return (startHealInTimes - startHealInTimes * (GameController.Instance.player_heal / 100f)) * (1 - GameController.Instance.player_level / 60f);
    }

    float GetShieldHeal()
    {
        //return startShieldHealInTimes - startShieldHealInTimes * (GameController.Instance.player_shieldHeal / 100f);

        return (startShieldHealInTimes - startShieldHealInTimes * (GameController.Instance.player_shieldHeal / 100f)) * (1 - GameController.Instance.player_level / 60f);
    }

    void IncreaseHealth()
    {
        if (total_health < start_health && !GameController.Instance.playerDead)
        {
            total_health += 1;
        }
    }

    void IncreaseShieldHealth()
    {
        if (shield_health < start_shieldHealth && !GameController.Instance.playerDead)
        {
            shield_health += 1;
        }
    }

    void ShieldParticles(Transform pos)
    {
        Instantiate(shieldParticles, pos.position, Quaternion.identity);
    }
    
    public int GetPlayerHealth()
    {
        return total_health;
    }

    public void ReceiveDamage(int x)
    {
        if (total_health > 0)
        {
            if (total_health > x)
                total_health -= x;
            else
            {
                total_health = 0;
                ps.SetIsDead(true);
            }
                
        }

    }

    public int GetPlayerShieldHealth()
    {
        return shield_health;
    }

    public void ReceiveShieldDamage(int x)
    {
        if (shield_health > 0)
        {
            if (shield_health > x)
                shield_health -= x;
            else
            {
                shield_health = 0;

                if(GetComponent<PlayerState>().GetShield() < 3)
                    GetComponent<HitAudioSource>().ShieldBrokeWood(shieldBrokeWood);
                else
                    GetComponent<HitAudioSource>().ShieldBrokeWood(shieldBrokeIron);

                ShieldParticles(shields[GameController.Instance.player_shield].transform);
                shields[GameController.Instance.player_shield].SetActive(false);
                defenseBtn.interactable = false;
                ps.Mobile_DefenseUnClicked();
                ps.GetAnim().SetLayerWeight(ps.GetAnim().GetLayerIndex("Layer1"), 0f);

            }

        }

    }


}
