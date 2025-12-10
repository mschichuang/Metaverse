using System.Collections.Generic;
using UnityEngine;

public class PurchaseHistoryManager : MonoBehaviour
{
    // 儲存: 類別 -> 等級分數 (金=3, 銀=2, 銅=1, 未購買=0)
    private Dictionary<string, int> purchasedTiers = new Dictionary<string, int>();
    
    // 儲存: 類別 -> 產品ID (用於區分同類別中哪個產品被購買)
    private Dictionary<string, string> purchasedProductIds = new Dictionary<string, string>();

    public bool HasPurchasedCategory(string category)
    {
        return purchasedTiers.ContainsKey(category) && purchasedTiers[category] > 0;
    }
    
    /// <summary>
    /// 檢查特定產品是否被購買
    /// </summary>
    public bool IsProductPurchased(string category, string productId)
    {
        return purchasedProductIds.ContainsKey(category) && purchasedProductIds[category] == productId;
    }
    
    /// <summary>
    /// 取得該類別購買的產品ID (如果有)
    /// </summary>
    public string GetPurchasedProductId(string category)
    {
        return purchasedProductIds.ContainsKey(category) ? purchasedProductIds[category] : null;
    }

    /// <summary>
    /// 記錄購買，儲存等級分數和產品ID
    /// </summary>
    public void AddPurchasedCategory(string category, int tier, string productId = null)
    {
        if (purchasedTiers.ContainsKey(category))
        {
            purchasedTiers[category] = tier;
        }
        else
        {
            purchasedTiers.Add(category, tier);
        }
        
        // 儲存產品ID
        if (!string.IsNullOrEmpty(productId))
        {
            if (purchasedProductIds.ContainsKey(category))
            {
                purchasedProductIds[category] = productId;
            }
            else
            {
                purchasedProductIds.Add(category, productId);
            }
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
        if (purchasedProductIds.ContainsKey(category))
        {
            purchasedProductIds.Remove(category);
        }
    }

    // 中文轉英文對照表 (確保 URL 安全)
    // 必須與 ProductData 資產中的 category 欄位完全一致
    private static readonly Dictionary<string, string> categoryToEnglish = new Dictionary<string, string>
    {
        {"機殼", "Case"},
        {"主機板", "MB"},
        {"中央處理器", "CPU"},
        {"散熱器", "Cooler"},
        {"記憶體", "RAM"},
        {"固態硬碟", "SSD"},
        {"顯示卡", "GPU"},
        {"電源供應器", "PSU"}
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
