using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour {

    public static Utility Instance;

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

    public static int GetRandomNumber(int min, int max)
    {
        return (int)(Random.Range(min, max + 1));
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public static Vector3 getVectorBetween(Transform player, Transform enemy)
    {
        Vector3 heading = player.transform.position - enemy.transform.position;
        return heading;
    }

    public static float GetDistanceBetween(Transform player, Transform enemy)
    {
        float distance = getVectorBetween(player, enemy).x;
        return distance;
    }

    public static bool AtSameYPosition(Transform player, Transform enemy)
    {
        float distance = getVectorBetween(player, enemy).x;

        if (Mathf.Abs((player.transform.position - enemy.transform.position).y) < 10f)
            return true;
        else
            return false;
    }

    public static int EnemyDirection(Transform player, Transform enemy)
    {
        if (GetDistanceBetween(player, enemy) > 0)
            return 1;
        else
            return -1;
    }

    public static int GetArrowDamageAmount(int index, int ember) // 0 not embered 1 embered
    {
        float qualityEffect = 1 + 0.4f * index;
        float emberEffect = 1 + ember / 10f;
        float arrowMultiplier = 20f;

        int arrowDamageAmount = (int)(arrowMultiplier * emberEffect * qualityEffect);
        return arrowDamageAmount;
    }

    public static int GetWeaponDamageAmount(int[] categoryAndIndex, int power)
    {
        int weaponDamageAmount = 0;

        int swordMultiplier = 30;
        int axeMultiplier = 40;
        int maceMultiplier = 45;

        float powerEffect = 1 + power * 0.05f;
        float qualityEffect = 1 + 0.4f * categoryAndIndex[1];

        switch (categoryAndIndex[0])
        {
            case 0:

                weaponDamageAmount = (int)(swordMultiplier * powerEffect * qualityEffect); // sword
                break;
            case 1:
                weaponDamageAmount = (int)(maceMultiplier * powerEffect * qualityEffect); // mace
                break;
            case 2:
                weaponDamageAmount = (int)(axeMultiplier * powerEffect * qualityEffect); // axe
                break;
        }
        return weaponDamageAmount;
    }

    public static int GetWeaponDamageAmount(int[] categoryAndIndex, int power, int emberPercentage)
    {
        int weaponDamageAmount = 0;

        int swordMultiplier = 30;
        int axeMultiplier = 40;
        int maceMultiplier = 45;
        float emberEffect = 1f + emberPercentage / 100f;

        float powerEffect = 1 + power * 0.05f;
        float qualityEffect = 1 + 0.4f * categoryAndIndex[1];

        switch (categoryAndIndex[0])
        {
            case 0:

                weaponDamageAmount = (int)(swordMultiplier * powerEffect * qualityEffect * emberEffect); // sword
                break;
            case 1:
                weaponDamageAmount = (int)(maceMultiplier * powerEffect * qualityEffect * emberEffect); // mace
                break;
            case 2:
                weaponDamageAmount = (int)(axeMultiplier * powerEffect * qualityEffect * emberEffect); // axe
                break;
        }
        return weaponDamageAmount;
    }

    public static int GetWeaponDamageAmountPlayer(int [] categoryAndIndex, int power, int sharpeningPercentage, int emberPercentage)
    {
        int weaponDamageAmount = 0;

        int swordMultiplier = 30;
        int axeMultiplier = 40;
        int maceMultiplier = 45;

        float powerEffect = 1 + power * 0.05f;
        float qualityEffect = 1 + 0.4f * categoryAndIndex[1];
        float sharpeningEffect = 1f + sharpeningPercentage / 100f;
        float emberEffect = 1f + emberPercentage / 100f;
        
        switch (categoryAndIndex[0])
        {
            case 0:
                if (categoryAndIndex[1] > 3)
                    weaponDamageAmount = (int)(swordMultiplier * powerEffect * qualityEffect * sharpeningEffect * emberEffect); // sword
                else
                    weaponDamageAmount = (int)(swordMultiplier * powerEffect * qualityEffect * sharpeningEffect);
                break;
            case 1:
                if (categoryAndIndex[1] > 3)
                    weaponDamageAmount = (int)(maceMultiplier * powerEffect * qualityEffect * emberEffect); // mace
                else
                    weaponDamageAmount = (int)(maceMultiplier * powerEffect * qualityEffect);
                break;
            case 2:
                if (categoryAndIndex[1] > 3)
                    weaponDamageAmount = (int)(axeMultiplier * powerEffect * qualityEffect * emberEffect); // axe
                else
                    weaponDamageAmount = (int)(axeMultiplier * powerEffect * qualityEffect);
                break;
        }
        return weaponDamageAmount;
    }

    public static int CalculatePocketMoneyAmount(int level) // her kisiden ortalama 10 para almali
    {
        return GetRandomNumber(8, 12) + level / 5;
    }

    public static int CalculateTreasureAmount(int level)
    {
        return GetRandomNumber(8, 12) * 5 + level;
    }

    public static int GetSpearDamageAmount(int index, int power, int emberPercentage)
    {

        int spearMultiplier = 20;
        float powerEffect = 1 + power * 0.05f;
        float qualityEffect = 1 + 0.4f * index;
        float emberEffect = 1f + emberPercentage / 100f;
        int weaponDamageAmount = (int)(spearMultiplier * powerEffect * qualityEffect * emberEffect); // spear

        return weaponDamageAmount;
    }

    public static int GetDamageMultiplier(string category, int index)
    {
        int damageMultiplier = 0;

        int swordMultiplier = 30;
        int axeMultiplier = 40;
        int maceMultiplier = 45;

        float qualityEffect = 1 + 0.4f * index;

        if (category.Equals("sword"))
            damageMultiplier = (int)(swordMultiplier * qualityEffect);
        else if (category.Equals("axe"))
            damageMultiplier = (int)(axeMultiplier * qualityEffect);
        else if (category.Equals("mace"))
            damageMultiplier = (int)(maceMultiplier * qualityEffect);

        return damageMultiplier;
    }

    public static int GetArmorMultiplier(string category, int index)
    {
        int armor = 0;

        if (category.Equals("breastplate"))
            armor = 50 * (index + 1);
        else if (category.Equals("helmet"))
            armor = 30 * (index + 1);
        else if (category.Equals("tozluk"))
            armor = 15 * (index + 1);

        return armor;
    }

    public static int GetShieldHealth(int index, int power , int startHealth)
    {
        int startShield = 100;
        float qualityEffect = 1 + 0.3f * index;
        float powerEffect = 1 + power * 0.05f;

        int shieldHealth = startHealth + (int)(startShield * qualityEffect * powerEffect);

        return shieldHealth;
    }

    public static int GetShieldMultiplier(int index)
    {
        int startShield = 100;
        float qualityEffect = 1 + 0.3f * index;

        int shieldMultiplier = (int)(startShield * qualityEffect);

        return shieldMultiplier;
    }

    public static int GetHelmetLogic(GameObject[] helmet, int level, float range)
    {
        int helmetIndex = Utility.GetRandomNumber((int)(level * (range - 0.05f)), (int)(level * (range + 0.05f)));

        if (helmetIndex >= helmet.Length)
            helmetIndex = helmet.Length - 1;

        return helmetIndex;
    }

    public static int GetTozlukLogic(GameObject[] tozluk, int level, float range)
    {
        int tozlukIndex = Utility.GetRandomNumber((int)(level * (range - 0.05f)), (int)(level * (range + 0.05f)));

        if (tozlukIndex >= tozluk.Length)
            tozlukIndex = tozluk.Length - 1;

        return tozlukIndex;
    }

    public static int GetBowLogic(GameObject[] bows, int level)
    {
        int bow = GetRandomNumber((int)(level * 0.09f), (int)(level * 0.15f));

        if (bow >= bows.Length)
            bow = bows.Length - 1;

        return bow;
    }

    public static int GetBreastplateLogic(GameObject[] breastPlate, int level, float range)
    {
        int breastPlateIndex = Utility.GetRandomNumber((int)(level * (range - 0.05f)), (int)(level * (range + 0.05f)));

        if (breastPlateIndex >= breastPlate.Length)
            breastPlateIndex = breastPlate.Length - 1;

        return breastPlateIndex;
    }

    public static int GetSwordLogic(GameObject[] sword, int level, float range)
    {
        int swordIndex = GetRandomNumber((int)(level * (range - 0.08f)), (int)(level * (range)));

        if (swordIndex >= sword.Length)
            swordIndex = sword.Length - 1;

        return swordIndex;
    }

    public static int GetAxeLogic(GameObject[] axe, int level, float range)
    {
        range = 0.15f;

        int axeIndex = GetRandomNumber((int)(level * (range - 0.04f)), (int)(level * (range + 0.04f)));

        if (axeIndex >= axe.Length)
            axeIndex = axe.Length - 1;

        return axeIndex;
    }

    public static int GetMaceLogic(GameObject[] mace, int level, float range)
    {
        range = 0.15f;

        int maceIndex = GetRandomNumber((int)(level * (range - 0.04f)), (int)(level * (range + 0.04f)));

        if (maceIndex >= mace.Length)
            maceIndex = mace.Length - 1;

        return maceIndex;
    }

    public static int GetShieldLogic(GameObject[] shield, int level, float range)
    {
        int shieldIndex = Utility.GetRandomNumber((int)(level * (range - 0.05f)), (int)(level * (range + 0.05f)));

        if (shieldIndex >= shield.Length)
            shieldIndex = shield.Length - 1;

        return shieldIndex;
    }

    public static bool CheckAllDied()
    {
        //Debug.Log(GameController.Instance.totalEnemy.ToString());

        if (GameController.Instance.totalEnemy == 0)
            return true;
        else
            return false;
    }

    public static void StartTimers()
    {
        for (int i = 0; i < GameController.Instance.itemList.Count; i++)
        {
            GameController.Instance.itemList[i].StartTimer();
        }
    }

    public static void StopTimers()
    {
        for (int i = 0; i < GameController.Instance.itemList.Count; i++)
        {
            GameController.Instance.itemList[i].StopTimer();
        }
    }

    public static int RequiredLevelForTwelve(int index)
    {
        return 4 * index;
    }

    public static int RequiredLevelForEight(int index)
    {
        return 6 * index;
    }
}