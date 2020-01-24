using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedPower : MonoBehaviour {

    // Cursed ozellikleri levele gore duzenlenir. Herhangi bir random deger gozetilmez.

    public float damageAmount = 2;
    public float healthAmount = 20;

    float startDamage = 30;
    float startHealth = 100;

    private float givenDamage;
    private float totalHealth;

    private float startTotalHealth;

    CursedController cc;

    public GameObject healthLoad;
    public GameObject healthBack;

    float startScaleX;
    float currentScaleX;

    private bool ember;

    public GameObject hot;

    void Start () {

        cc = GetComponent<CursedController>();
        givenDamage = startDamage + GameController.Instance.player_level * damageAmount;
        totalHealth = startTotalHealth = startHealth + GameController.Instance.player_level * healthAmount;
        startScaleX = healthLoad.transform.localScale.x;

        SetEmber();
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

    void SetEmber()
    {
        if (Utility.GetRandomNumber(0, 2) == 0)
        {
            ember = true;
            hot.SetActive(true);
        }
            

    }

    public bool GetEmber()
    {
        return ember;
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
