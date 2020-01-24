using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Price {

    static int[] breastplatePrices = new int[12];
    static int[] helmetPrices = new int[12];
    static int[] shinguardPrices = new int[12];
    static int[] shieldPrices = new int[12];
    
    static int[] swordPrices = new int[12];
    static int[] axePrices = new int[8];
    static int[] macePrices = new int[8];

    static int[] foodPrices = new int[12];
    static int[] hardwarePrices = new int[8];

    /*
     * A static constructor cannot be called 
     * directly and is only meant to 
     * be called by the common language 
     * runtime (CLR). It is invoked automatically.
     * 
     * https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors
     * 
     */
    static Price()
    {
        swordPrices[0] = 120;
        swordPrices[1] = 180;
        swordPrices[2] = 240;
        swordPrices[3] = 315;
        swordPrices[4] = 380;
        swordPrices[5] = 480;
        swordPrices[6] = 580;
        swordPrices[7] = 700;
        swordPrices[8] = 840;
        swordPrices[9] = 1000;
        swordPrices[10] = 1200;
        swordPrices[11] = 1450;

        axePrices[0] = 160;
        axePrices[1] = 230;
        axePrices[2] = 300;
        axePrices[3] = 400;
        axePrices[4] = 550;
        axePrices[5] = 750;
        axePrices[6] = 1000;
        axePrices[7] = 1300;

        macePrices[0] = 180;
        macePrices[1] = 260;
        macePrices[2] = 340;
        macePrices[3] = 440;
        macePrices[4] = 600;
        macePrices[5] = 800;
        macePrices[6] = 1100;
        macePrices[7] = 1500;

        breastplatePrices[0] = 150;
        breastplatePrices[1] = 300;
        breastplatePrices[2] = 450;
        breastplatePrices[3] = 625;
        breastplatePrices[4] = 800;
        breastplatePrices[5] = 1000;
        breastplatePrices[6] = 1200;
        breastplatePrices[7] = 1450;
        breastplatePrices[8] = 1700;
        breastplatePrices[9] = 2100;
        breastplatePrices[10] = 2500;
        breastplatePrices[11] = 3000;

        helmetPrices[0] = 100;
        helmetPrices[1] = 200;
        helmetPrices[2] = 300;
        helmetPrices[3] = 410;
        helmetPrices[4] = 520;
        helmetPrices[5] = 640;
        helmetPrices[6] = 780;
        helmetPrices[7] = 930;
        helmetPrices[8] = 1100;
        helmetPrices[9] = 1400;
        helmetPrices[10] = 1750;
        helmetPrices[11] = 2100;

        shinguardPrices[0] = 60;
        shinguardPrices[1] = 120;
        shinguardPrices[2] = 185;
        shinguardPrices[3] = 250;
        shinguardPrices[4] = 320;
        shinguardPrices[5] = 400;
        shinguardPrices[6] = 480;
        shinguardPrices[7] = 560;
        shinguardPrices[8] = 660;
        shinguardPrices[9] = 800;
        shinguardPrices[10] = 980;
        shinguardPrices[11] = 1300;

        shieldPrices[0] = 230;
        shieldPrices[1] = 300;
        shieldPrices[2] = 380;
        shieldPrices[3] = 470;
        shieldPrices[4] = 570;
        shieldPrices[5] = 680;
        shieldPrices[6] = 800;
        shieldPrices[7] = 940;
        shieldPrices[8] = 1000;
        shieldPrices[9] = 1200;
        shieldPrices[10] = 1500;
        shieldPrices[11] = 1850;

        foodPrices[0] = 30;
        foodPrices[1] = 40;
        foodPrices[2] = 50;
        foodPrices[3] = 80;
        foodPrices[4] = 90;
        foodPrices[5] = 100;
        foodPrices[6] = 130;
        foodPrices[7] = 140;
        foodPrices[8] = 150;
        foodPrices[9] = 180;
        foodPrices[10] = 190;
        foodPrices[11] = 200;

        hardwarePrices[0] = 80;
        hardwarePrices[1] = 90;
        hardwarePrices[2] = 100;
        hardwarePrices[3] = 130;
        hardwarePrices[4] = 140;
        hardwarePrices[5] = 150;
        hardwarePrices[6] = 190;
        hardwarePrices[7] = 200;
    }

    public static int GetPrice(string category, int index)
    {
        if (category.Equals("sword"))
            return swordPrices[index];

        if (category.Equals("breastplate"))
            return breastplatePrices[index];

        if (category.Equals("tozluk"))
            return shinguardPrices[index];

        if (category.Equals("helmet"))
            return helmetPrices[index];

        if (category.Equals("shield"))
            return shieldPrices[index];

        if (category.Equals("axe"))
            return axePrices[index];

        if (category.Equals("mace"))
            return macePrices[index];

        if (category.Equals("food"))
            return foodPrices[index];

        if (category.Equals("hardware"))
            return hardwarePrices[index];


        return 0;
    }

    public static int GetSellingPrice(string category, int index) // need to handle
    {
        if (category.Equals("sword"))
        {
            return swordPrices[index] / 2;
        }
        if (category.Equals("axe"))
        {
            return axePrices[index] / 2;
        }
        if (category.Equals("mace"))
        {
            return macePrices[index] / 2;
        }
        if (category.Equals("breastplate"))
        {
            return breastplatePrices[index] / 2;
        }
        if (category.Equals("helmet"))
        {
            return helmetPrices[index] / 2;
        }
        if (category.Equals("tozluk"))
        {
            return shinguardPrices[index] / 2;
        }
        if (category.Equals("shield"))
        {
            return shieldPrices[index] / 2;
        }
        if (category.Equals("hardware"))
        {
            return hardwarePrices[index] / 2;
        }
        if (category.Equals("food"))
        {
            return foodPrices[index] / 2;
        }

        return 0;
    }

}
