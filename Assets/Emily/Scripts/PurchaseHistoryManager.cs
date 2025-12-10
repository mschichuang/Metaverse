using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

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
        
        RecordPurchase(category, productName);
    }

    public void RemovePurchasedCategory(string category)
    {
        if (purchasedItems.ContainsKey(category))
        {
            purchasedItems.Remove(category);
        }
        ClearPurchase(category);
    }

    /// <summary>
    /// 回傳組裝資料字串，格式範例: [機殼:NV7銀][CPU:i9-14900K]
    /// </summary>
    public string GetAssemblyDataString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var kvp in purchasedItems)
        {
            sb.Append($"[{kvp.Key}:{kvp.Value}]");
        }
        return sb.ToString();
    }

    private async void RecordPurchase(string category, string productName)
    {
        string group = PlayerInfoManager.GetPlayerGroup();
        string json = $"{{\"group\":\"{group}\", \"category\":\"{category}\", \"productName\":\"{productName}\"}}";
        string url = "https://script.google.com/macros/s/AKfycbx1GbdP6H2bfVZXagnmacWCtytaw5Yi6WRZzPkWc_txCSK3jHrEcLwDWcPvdtaOzXyp/exec";

        Debug.Log($"[RecordPurchase] Sending JSON: {json}");

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.SendWebRequest();
            while (!request.isDone)
                await Task.Yield();
        }
    }

    private async void ClearPurchase(string category)
    {
        string group = PlayerInfoManager.GetPlayerGroup();
        string json = $"{{\"group\":\"{group}\", \"category\":\"{category}\", \"productName\":\"\"}}";
        string url = "https://script.google.com/macros/s/AKfycbx1GbdP6H2bfVZXagnmacWCtytaw5Yi6WRZzPkWc_txCSK3jHrEcLwDWcPvdtaOzXyp/exec";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.SendWebRequest();
            while (!request.isDone)
                await Task.Yield();
        }
    }
}
