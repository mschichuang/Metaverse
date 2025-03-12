using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoinManager
{
    private static int totalCoins = 0;

    public static int TotalCoins
    {
        get { return totalCoins; }
        set { totalCoins = value; }
    }

    public static void AddCoins(int amount)
    {
        totalCoins += amount;
    }

    public static void DeductCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
        }
        else
        {
            Debug.LogWarning("金幣不足！");
        }
    }
}
