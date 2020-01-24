using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedBossPower : MonoBehaviour {

    public float damageAmount;
    public float healthAmount;

    private float givenDamage;
    private float totalHealth;

    private float startTotalHealth;

    CursedBossCtrl cc;

    public GameObject healthLoad;
    public GameObject healthBack;

    float startScaleX;
    float currentScaleX;

    void Start()
    {

        cc = GetComponent<CursedBossCtrl>();
        givenDamage = damageAmount;
        totalHealth = startTotalHealth = healthAmount;
        startScaleX = healthLoad.transform.localScale.x;
    }

    public void ReceiveDamage(int x)
    {
        if (totalHealth > 0)
        {
            if (totalHealth > x)
            {
                totalHealth -= x;
                healthLoad.transform.localScale = new Vector3(((float)GetTotalHealth() / startTotalHealth) * startScaleX, healthLoad.transform.localScale.y, healthLoad.transform.localScale.z);
            }
            else
            {
                totalHealth = 0;
                healthLoad.gameObject.SetActive(false);
                healthBack.gameObject.SetActive(false);

                cc.SetIsDead(true);
            }

        }

    }

    public float GetDamageAmount()
    {
        return givenDamage;
    }

    public float GetTotalHealth()
    {
        return totalHealth;
    }

}
