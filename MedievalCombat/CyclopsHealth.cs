using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsHealth : MonoBehaviour {

    public int startDamage;
    public int startHealth;
    int totalStartHealth;

    int total_health;
    int damage_amount;

    CyclopsController cc;

    float startScaleX;
    float currentScaleX;

    float startScaleShieldX;

    public GameObject healthLoad;
    public GameObject healthBack;

    void Start () {
        cc = GetComponent<CyclopsController>();
        damage_amount = startDamage;
        total_health = totalStartHealth = startHealth;
        startScaleX = healthLoad.transform.localScale.x;

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

                cc.SetIsDead(true);
            }
        }

    }

    public int GetEnemyHealth()
    {
        return total_health;
    }

    public int GetDamageAmount()
    {
        return damage_amount;
    }
}
