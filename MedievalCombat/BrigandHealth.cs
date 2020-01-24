using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrigandHealth : MonoBehaviour
{
    public int startHealth;
    public int breastPlatePower;
    public int helmetPower;
    public int tozlukPower;
    public int healthAmountPerSkill;

    int totalStartHealth;
    
    int total_health;

    BrigandArmour ea;
    EnemySkill es;

    public ParticleSystem shieldParticles;

    BrigandController ic;
    
    float startScaleX;
    float currentScaleX;

    float startScaleShieldX;

    public GameObject healthLoad;
    public GameObject healthBack;

    void Start()
    {

        ic = this.GetComponent<BrigandController>();
        es = this.GetComponent<EnemySkill>();
        ea = this.GetComponent<BrigandArmour>();
        

        total_health = totalStartHealth = startHealth + ea.GetBreastPlate() * breastPlatePower +
            ea.GetTozluk() * tozlukPower +
            ea.GetHelmet() * helmetPower +
            es.GetEnemyVigor() * healthAmountPerSkill;
        
        startScaleX = healthLoad.transform.localScale.x;
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

                ic.SetIsDead(true);
            }
        }

    }


}
