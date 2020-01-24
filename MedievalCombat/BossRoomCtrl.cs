using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomCtrl : MonoBehaviour {

    public GameObject bosses;

	void Start () {
        SetCurrentBoss();
        GameController.Instance.atBossRoom = true;
    }
	
	void SetCurrentBoss()
    {
        int index = GameController.Instance.current_level;

        switch (index)
        {
            case 5:
                bosses.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 9:
                bosses.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 15:
                bosses.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 21:
                bosses.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case 26:
                bosses.transform.GetChild(4).gameObject.SetActive(true);
                break;
            case 30:
                bosses.transform.GetChild(5).gameObject.SetActive(true);
                break;
            case 35:
                bosses.transform.GetChild(6).gameObject.SetActive(true);
                break;
            case 41:
                bosses.transform.GetChild(7).gameObject.SetActive(true);
                break;
            case 45:
                bosses.transform.GetChild(8).gameObject.SetActive(true);
                break;
            case 50:
                bosses.transform.GetChild(9).gameObject.SetActive(true);
                break;

        }
    }

}
