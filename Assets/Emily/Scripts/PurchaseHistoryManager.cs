using System.Collections.Generic;
using UnityEngine;

public class PurchaseHistoryManager : MonoBehaviour
{
    private Dictionary<string, string> purchasedItems = new Dictionary<string, string>();

    public bool HasPurchasedCategory(string category)
    {
        return purchasedItems.ContainsKey(category);
    }

    public void AddPurchasedCategory(string category, string productName)
    {
        if (purchasedItems.ContainsKey(category))
        {
            purchasedItems[category] = productName;
        }
        else
        {
            purchasedItems.Add(category, productName);
        }
    }

    public void RemovePurchasedCategory(string category)
    {
        if (purchasedItems.ContainsKey(category))
        {
            purchasedItems.Remove(category);
        }
    }

    /// <summary>
    /// 回傳組裝資料字串，格式範例: CPU:i9-14900K;GPU:RTX4090;
    /// 使用分號分隔，避免 URL 編碼問題
    /// </summary>
    public string GetAssemblyDataString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var kvp in purchasedItems)
        {
            // 格式: "CPU:產品名稱;" (用分號分隔，不用換行)
            sb.Append($"{kvp.Key}:{kvp.Value};");
        }
        return sb.ToString();
    }
}
