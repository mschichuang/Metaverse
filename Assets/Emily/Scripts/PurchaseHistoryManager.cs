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

    /// <summary>
    /// 回傳組裝等級資料字串
    /// 格式範例: CPU:3;GPU:2;RAM:1;SSD:0;
    /// 金=3, 銀=2, 銅=1, 未購買=0
    /// </summary>
    public string GetAssemblyDataString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var kvp in purchasedTiers)
        {
            // 將中文類別轉換為英文 (如果有對照的話)
            string englishKey = categoryToEnglish.ContainsKey(kvp.Key) ? categoryToEnglish[kvp.Key] : kvp.Key;
            sb.Append($"{englishKey}:{kvp.Value};");
        }
        return sb.ToString();
    }
}
