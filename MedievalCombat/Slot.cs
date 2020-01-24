using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {
    
    List<Item> itemList;

    int page = 0;

    void Start () {

        itemList = GameController.Instance.itemList;

        AddEnvanter(itemList);
    }

    public void AddEnvanter(List<Item> itemList)
    {
        ClearInventory();

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].GetCategory().Equals("food"))
                transform.GetChild(i).GetChild(76 + itemList[i].GetIndex()).gameObject.SetActive(true);

            if (itemList[i].GetCategory().Equals("breastplate"))
                transform.GetChild(i).GetChild(itemList[i].GetIndex()).gameObject.SetActive(true);

            if (itemList[i].GetCategory().Equals("helmet"))
                transform.GetChild(i).GetChild(12 + itemList[i].GetIndex()).gameObject.SetActive(true);

            if (itemList[i].GetCategory().Equals("tozluk"))
                transform.GetChild(i).GetChild(24 + itemList[i].GetIndex()).gameObject.SetActive(true);

            if (itemList[i].GetCategory().Equals("shield"))
                transform.GetChild(i).GetChild(36 + itemList[i].GetIndex()).gameObject.SetActive(true);

            if (itemList[i].GetCategory().Equals("sword"))
                transform.GetChild(i).GetChild(48 + itemList[i].GetIndex()).gameObject.SetActive(true);

            if (itemList[i].GetCategory().Equals("axe"))
                transform.GetChild(i).GetChild(60 + itemList[i].GetIndex()).gameObject.SetActive(true);

            if (itemList[i].GetCategory().Equals("mace"))
                transform.GetChild(i).GetChild(68 + itemList[i].GetIndex()).gameObject.SetActive(true);

            if (itemList[i].GetCategory().Equals("hardware"))
                transform.GetChild(i).GetChild(88 + itemList[i].GetIndex()).gameObject.SetActive(true);
            
        }
        

    }

    public void ChangePage(int x)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < 8; i++)
        {
            transform.GetChild(8 * x + i).gameObject.SetActive(true);
        }
        
    }

    void ClearInventory()
    {
        for (int i = 0; i < 24; i++) // 24 inventory size ile beraber degismeli
        {
            foreach (Transform child in transform.GetChild(i))
            {
                child.gameObject.SetActive(false);
            }

        }
    }
}
