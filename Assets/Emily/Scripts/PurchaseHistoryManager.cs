using System.Collections.Generic;
using UnityEngine;

public class PurchaseHistoryManager : MonoBehaviour
{
    // 儲存: 類別 -> 等級分數 (金=3, 銀=2, 銅=1, 未購買=0)
    private Dictionary<string, int> purchasedTiers = new Dictionary<string, int>();

    public bool HasPurchasedCategory(string category)
    {
        return purchasedTiers.ContainsKey(category) && purchasedTiers[category] > 0;
    }

    /// <summary>
    /// 記錄購買，儲存等級分數
    /// </summary>
    public void AddPurchasedCategory(string category, int tier)
    {
        if (purchasedTiers.ContainsKey(category))
        {
            purchasedTiers[category] = tier;
        }
        else
        {
            purchasedTiers.Add(category, tier);
        }
    }

    /// <summary>
    /// 移除購買 (設為 0)
    /// </summary>
    public void RemovePurchasedCategory(string category)
    {
        if (purchasedTiers.ContainsKey(category))
        {
            purchasedTiers[category] = 0;
        }
    }

    // 中文轉英文對照表 (確保 URL 安全)
    private static readonly Dictionary<string, string> categoryToEnglish = new Dictionary<string, string>
    {
        {"機殼", "Case"},
        {"主機板", "MB"},
        {"CPU", "CPU"},
        {"散熱器", "Cooler"},
        {"記憶體", "RAM"},
        {"硬碟", "SSD"},
        {"顯示卡", "GPU"},
        {"電源", "PSU"}
    };

    // 固定順序的類別清單 (用於輸出)
    private static readonly string[] categoryOrder = { "Case", "MB", "CPU", "Cooler", "RAM", "SSD", "GPU", "PSU" };

    /// <summary>
    /// 取得指定類別的等級分數
    /// </summary>
    /// <param name="englishCategory">英文類別名 (Case, MB, CPU, Cooler, RAM, SSD, GPU, PSU)</param>
    /// <returns>等級分數 (金=3, 銀=2, 銅=1, 未購買=0)</returns>
    public int GetTierByCategory(string englishCategory)
    {
        // 查找該類別的等級 (可能是中文或英文 key)
        foreach (var kvp in purchasedTiers)
        {
            string key = categoryToEnglish.ContainsKey(kvp.Key) ? categoryToEnglish[kvp.Key] : kvp.Key;
            if (key == englishCategory)
            {
                return kvp.Value;
            }
        }
        return 0; // 未購買
    }

    /// <summary>
    /// 取得所有類別的等級 (用於 URL 參數)
    /// </summary>
    public Dictionary<string, int> GetAllTiers()
    {
        var result = new Dictionary<string, int>();
        foreach (string category in categoryOrder)
        {
            result[category] = GetTierByCategory(category);
        }
        return result;
    }
}
