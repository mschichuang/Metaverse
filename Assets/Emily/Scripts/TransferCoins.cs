using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Threading.Tasks;
using SpatialSys.UnitySDK;

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

            coinUIManager.SetCoins(data.totalCoins);
        }
    }

    [System.Serializable]
    private class GroupCoinsResponse
    {
        public int totalCoins;
    }
}