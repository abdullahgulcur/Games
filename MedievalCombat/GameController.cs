using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController Instance;

    public bool castlePassed = false;
    public bool levelPassed = false;

    public bool restartScene = false;
    public bool isInCastle = false;
    public bool gameFinished = false;
    public bool lastScenarioDemonstrable = false;
    public float[] magnitudeValsSmall;
    public float[] magnitudeValsBig;

    public int OpenableMapIndex = 0;

    public int totalEnemy = 0;

    public bool foundKey = false;
    public bool firstOpen = true;
    
    public List<Item> itemList = new List<Item>();

    public Color[] color = new Color[10];

    public bool firstGameOpen = true;
    
    public bool atBossRoom;
    public int current_level;
    public int player_total_money = 500;

    public int player_heal = 0;
    public int player_sharpening = 0;
    public int player_shieldHeal = 0;
    public int player_weaponEmber = 0;

    public int player_health;
    public int player_shield_health;

    public int player_haircolor_index;
    public int player_haircut;
    public int player_beard;
    public int player_moustache;
    public int player_helmet;
    public int player_tozluk;

    public int player_breastPlate;

    public int player_level = 1;
    
    public int player_vigor;
    public int player_power;
    public int player_agility;
    public int player_defence;

    public int skillpoints_left = 4;

    public int player_sword = -1;
    public int player_mace = -1;
    public int player_axe = -1;

    public int player_shield = -1;

    public bool playerDead;

    public bool playerEmber = false;
    public bool hotParticles = false;

    public int totalEnemyKilled = 0;

    public int tempMoney;

    public int timeInSeconds;
    float timer;

    void Start()
    {
        color[0] = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        color[1] = new Color(0.157f, 0.157f, 0.157f, 1.0f);
        color[2] = new Color(0.588f, 0.588f, 0.588f, 1.0f);
        color[3] = new Color(1f, 1f, 1f, 1.0f);
        color[4] = new Color(0.22f, 0.125f, 0.0f, 1.0f);
        color[5] = new Color(1f, 0.792f, 0.251f, 1.0f);
        color[6] = new Color(0.855f, 0.408f, 0f, 1.0f);
        color[7] = new Color(1f, 0.925f, 0.62f, 1.0f);
        color[8] = new Color(0.404f, 0.075f, 0f, 1.0f);
        color[9] = new Color(0.412f, 0.341f, 0.251f, 1.0f);

        magnitudeValsSmall = new float[] { 5f, 3f, 0.3f, 0.4f };
        magnitudeValsBig = new float[]{10f, 3f, 0.3f, 0.4f};
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

}
