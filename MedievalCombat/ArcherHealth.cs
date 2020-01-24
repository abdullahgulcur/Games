using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherHealth : MonoBehaviour {

    public int startHealth;
    public int breastPlatePower;
    public int helmetPower;
    public int tozlukPower;
    public int healthAmountPerSkill;

    int totalStartHealth;

    public GameObject healthLoad;
    public GameObject healthBack;

    int total_health;

    ArcherArmour aa;
    EnemySkill es;

    ArcherController ac;

    float startScaleX;
    float currentScaleX;

    void Start()
    {
        ac = this.GetComponent<ArcherController>();
        es = this.GetComponent<EnemySkill>();
        aa = this.GetComponent<ArcherArmour>();


        total_health = totalStartHealth = startHealth + aa.GetBreastPlate() * breastPlatePower +
            aa.GetTozluk() * tozlukPower +
            aa.GetHelmet() * helmetPower +
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

                ac.SetIsDead(true);
            }
        }

    }


}
