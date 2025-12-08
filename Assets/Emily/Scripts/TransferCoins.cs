using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Emily.Scripts;

public class TransferCoins : MonoBehaviour
{
    public CoinUIManager coinUIManager;

    public async void transferCoins()
    {
        string playerName = PlayerInfoManager.GetPlayerName();
        string url = $"{PlayerInfoManager.Url}?action=getGroupTotalCoins&name={playerName}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SendWebRequest();
            while (!request.isDone)
                await Task.Yield();

            string json = request.downloadHandler.text;
            GroupCoinsResponse data = JsonUtility.FromJson<GroupCoinsResponse>(json);

            // 先從 DataStore 讀取當前金幣
            int currentCoins = StudentData.Coins;
            int newCoins = data.totalCoins;
            
            // 計算差額
            int difference = newCoins - currentCoins;
            
            if (difference > 0)
            {
                // 增加金幣到 DataStore
                StudentData.AddCoins(difference, (result) => {
                    if (result)
                    {
                        coinUIManager.SetCoins(StudentData.Coins);
                    }
                });
            }
            else if (difference < 0)
            {
                // 扣除金幣 (理論上不應該發生,但保險起見)
                StudentData.SpendCoins(-difference);
                coinUIManager.SetCoins(StudentData.Coins);
            }
            else
            {
                // 金幣相同,只更新 UI
                coinUIManager.SetCoins(StudentData.Coins);
            }
            
            gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    private class GroupCoinsResponse
    {
        public int totalCoins;
    }
}