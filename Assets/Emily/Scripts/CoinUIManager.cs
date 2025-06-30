using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class CoinUIManager : MonoBehaviour
{
    public TMP_Text coinText;

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
            coinText.text = data.coins.ToString();
        }
    }

    [System.Serializable]
    private class CoinResponse
    {
        public int coins;
    }
}