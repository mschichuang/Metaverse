using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Threading.Tasks;
using SpatialSys.UnitySDK;

public class TransferCoins : MonoBehaviour
{
    public TMP_Text coinText;

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

            // coinText, Spatial錢包擇一
            coinText.text = data.totalCoins.ToString();
            SpatialBridge.inventoryService.AwardWorldCurrency((ulong)data.totalCoins);
        }
    }

    [System.Serializable]
    private class GroupCoinsResponse
    {
        public int totalCoins;
    }
}