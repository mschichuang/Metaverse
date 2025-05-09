using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Threading.Tasks;
using SpatialSys.UnitySDK;

public class CoinUIManager : MonoBehaviour
{
    public TMP_Text coinText;

    void Start()
    {
        string playerName = SpatialBridge.actorService.localActor.displayName.Split(' ')[1];
        _ = UpdateCoinDisplay(playerName);
    }

    private async Task UpdateCoinDisplay(string name)
    {
        string url = $"https://script.google.com/macros/s/AKfycbyQD56ArfGkOuYfa-RRqYFPbSDLbSdsU98UWw86XBcjPaQ4NJ9GhegNnocDrX5hdlfZ/exec?name={name}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SendWebRequest();
            while (!request.isDone)
                await Task.Yield();

            string json = request.downloadHandler.text;
            CoinResponse data = JsonUtility.FromJson<CoinResponse>(json);
            coinText.text = data.coins.ToString();
        }
    }

    [System.Serializable]
    private class CoinResponse
    {
        public int coins;
    }
}