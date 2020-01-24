using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour {
    
    private int[] skill = new int[4];

    /*
     * Bu script tamamen basarili bir sekilde bitmistir
     * Dusman skilleri 50 yi asamiyor
     * Genel olarak player leveline paralel olarak skilleri artiyor
     * 
     */
    
    void Start () {
        
        SetSkill(GameController.Instance.player_level / 6);

    }

    void SetSkill(int level)
    {
        FillSkill(1 * level); // her levelde kategori basina ne kadar dolduracagini belirliyo
        RandomlyFill(1 * level); // yarisina kadar full obur yarisi randomlu doldurmasi icin

    }
    void FillSkill(int count)
    {
        for(int x = 0; x < count; x++)
        {
            for (int i = 0; i < skill.Length; i++)
            {
                skill[i]++;
            }
        }
    }

    void RandomlyFill(int count)
    {
        for (int x = 0; x < count; x++)
        {
            for (int i = 0; i < skill.Length; i++)
            {
                skill[Utility.GetRandomNumber(0, skill.Length - 1)]++;
            }
        }

        for(int i = 0; i < skill.Length; i++)
            if (skill[i] > 50)
                skill[i] = 50;
    }
    /*
    void Update()
    {
        for(int i = 0; i < skill.Length; i++)
        {
            Debug.Log("i:" + i + " " + skill[i].ToString() + " "); // "i:" + i + " " +
        }
       // Debug.Log("\n\n");
    }
    */
    public int GetEnemyPower()
    {
        return skill[0];
    }

    public int GetEnemyAgility()
    {
        return skill[1];
    }
    
    public int GetEnemyVigor()
    {
        return skill[2];
    }

    public int GetEnemyDefence()
    {
        return skill[3];
    }

}
