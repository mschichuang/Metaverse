using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class CoinUIManager : MonoBehaviour
{
    public TMP_Text coinText;
    public int CurrentCoins { get; private set; }

    void Start()
    {
        _ = UpdateCoinUI();
    }

    public async Task UpdateCoinUI()
    {
        string name = PlayerInfoManager.GetPlayerName();
        string url = $"{PlayerInfoManager.Url}?action=getCoins&name={name}";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SendWebRequest();
            while (!request.isDone)
                await Task.Yield();

            string json = request.downloadHandler.text;
            CoinResponse data = JsonUtility.FromJson<CoinResponse>(json);
            SetCoins(data.coins);
        }
    }

    public void SetCoins(int amount)
    {
        CurrentCoins = amount;
        coinText.text = amount.ToString();
    }

    [System.Serializable]
    private class CoinResponse
    {
        public int coins;
    }
}