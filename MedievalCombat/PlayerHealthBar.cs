using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {

    /*
     * Cogu zaman start metodunda scriptler gec baslatilmaktadir
     * Bundan dolayi degerler null olarak donebilmektedir
     * Gecici cozum olarak start coroutine ile gec baslatilma denenmis ve basarili olmustur
     * Asagida daha iyi bir cozum yontemi bulunmaktadir
     * 
     */

    public Text player_healthText;
    public Text player_shieldText;

    public Image playerHealthBar;
    public Image playershieldBar;

    public Image playerShield;

    PlayerHealth ph;

    int playerStartHealth;
    int playerStartShield;

    void Start () {
        
    }

    void Update()
    {
        if(ph == null) // script null olmayincaya kadar if statement ici execute edilir
        {
            ph = this.GetComponent<PlayerHealth>();
            playerStartHealth = ph.GetPlayerHealth();               // blok ici sadece bir defa execute edilir
            playerStartShield = ph.GetPlayerShieldHealth();
          //  Debug.Log(playerStartHealth.ToString());
        }else
        {
            try
            {
                player_healthText.text = ph.GetPlayerHealth().ToString();
                player_shieldText.text = ph.GetPlayerShieldHealth().ToString();
                playerHealthBar.fillAmount = (float)ph.GetPlayerHealth() / playerStartHealth;
                playershieldBar.fillAmount = (float)ph.GetPlayerShieldHealth() / playerStartShield;

                if (ph.GetPlayerShieldHealth() <= 0)
                {
                    playerShield.gameObject.SetActive(false);
                    player_shieldText.gameObject.SetActive(false);
                }
            }
            catch
            {
                Debug.Log("find me");

            }
            
        }

        
    }
}
