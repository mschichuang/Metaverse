using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class PurchaseHistoryManager : MonoBehaviour
{
    private HashSet<string> purchasedCategories = new HashSet<string>();

    public bool HasPurchasedCategory(string category)
    {
        return purchasedCategories.Contains(category);
    }

    public void AddPurchasedCategory(string category, string productName)
    {
        purchasedCategories.Add(category);
        RecordPurchase(category, productName);
    }

    public void RemovePurchasedCategory(string category)
    {
        purchasedCategories.Remove(category);
        ClearPurchase(category);
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
