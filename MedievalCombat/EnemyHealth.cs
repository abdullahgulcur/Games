using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    public AudioClip shieldBrokeWood;
    public AudioClip shieldBrokeIron;

    public int startHealth;
    public int breastPlatePower;
    public int helmetPower;
    public int tozlukPower;
    public int healthAmountPerSkill;

    int totalStartHealth;
    int totalStartShieldHealth;

    public int startShield;

    public GameObject[] shields;

    int total_health;
    int shield_health;

    EnemyArmour ea;
    EnemySkill es;

    public ParticleSystem shieldParticles;

    InfantryController ic;

    bool shieldBusted;

    float startScaleX;
    float currentScaleX;

    float startScaleShieldX;
    float currentScaleShieldX;

    public GameObject healthLoad;
    public GameObject shieldLoad;

    public GameObject healthBack;
    public GameObject shieldBack;

    void Start () {

        ic = this.GetComponent<InfantryController>();
        es = this.GetComponent<EnemySkill>();
        ea = this.GetComponent<EnemyArmour>();

        shieldBusted = false;

        total_health = totalStartHealth = startHealth + ea.GetBreastPlate() * breastPlatePower +
            ea.GetTozluk() * tozlukPower +
            ea.GetHelmet() * helmetPower +
            es.GetEnemyVigor() * healthAmountPerSkill;
        
        shield_health = totalStartShieldHealth = Utility.GetShieldHealth(ea.GetShield(), es.GetEnemyDefence(), startShield) / 2;

        startScaleX = healthLoad.transform.localScale.x;
        startScaleShieldX = shieldLoad.transform.localScale.x;
    }

    public int GetEnemyHealth()
    {
        return total_health;
    }

    public void ReceiveDamage(int x)
    {
        if (total_health > 0)
        {
            if (total_health > x)
            {
                total_health -= x;
                healthLoad.transform.localScale = new Vector3(((float)GetEnemyHealth() / totalStartHealth) * startScaleX, healthLoad.transform.localScale.y, healthLoad.transform.localScale.z);
            } 
            else
            {
                total_health = 0;
                healthLoad.gameObject.SetActive(false);
                healthBack.gameObject.SetActive(false);
                shieldLoad.gameObject.SetActive(false);
                shieldBack.gameObject.SetActive(false);

                ic.SetIsDead(true);
                ic.GetAnim().SetLayerWeight(ic.GetAnim().GetLayerIndex("Layer1"), 0f);
            }
        }

    }

    public int GetEnemyShieldHealth()
    {
        return shield_health;
    }

    public void ReceiveShieldDamage(int x)
    {
        if (shield_health > 0)
        {
            if (shield_health > x)
            {
                shield_health -= x;
                shieldLoad.transform.localScale = new Vector3(((float)GetEnemyShieldHealth() / totalStartShieldHealth) * startScaleShieldX, shieldLoad.transform.localScale.y, shieldLoad.transform.localScale.z);
            }
            else
            {
                shield_health = 0;
                shieldLoad.gameObject.SetActive(false);
                shieldBack.gameObject.SetActive(false);

                if (GetComponent<EnemyArmour>().GetShield() < 3)
                    GetComponent<HitAudioSource>().ShieldBrokeWood(shieldBrokeWood);
                else
                    GetComponent<HitAudioSource>().ShieldBrokeWood(shieldBrokeIron);

                ShieldParticles(shields[ea.GetShield()].transform);
                shields[ea.GetShield()].SetActive(false);
                shieldBusted = true;
            }
                
        }

    }

    void ShieldParticles(Transform pos)
    {
        Instantiate(shieldParticles, pos.position, Quaternion.identity);
    }

}
