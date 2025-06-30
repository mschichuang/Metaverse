using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using SpatialSys.UnitySDK;

public class TransferCoins : MonoBehaviour
{
    public GameObject transferDiamond;

    public void OnTransferTriggered()
    {
        transferDiamond.SetActive(false);
        string playerName = PlayerInfoManager.GetPlayerName();
        _ = CheckIsLeader(playerName);
    }

    private async Task CheckIsLeader(string name)
    {
        string url = $"{PlayerInfoManager.Url}?action=checkIsLeader&name={name}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            var op = request.SendWebRequest();
            while (!op.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                IsLeaderResponse data = JsonUtility.FromJson<IsLeaderResponse>(json);

                if (data.isLeader == "Y")
                {
                    Debug.Log("✅ 是組長，可以轉移金幣");
                    // 👉 在這裡加上 TransferCoinsToLeader() 的邏輯
                }
                else
                {
                    Debug.Log("❌ 不是組長，無法轉移");
                    ulong currentBalance = SpatialBridge.inventoryService.worldCurrencyBalance;
                    Debug.Log($"目前餘額：{currentBalance}");
                }
            }
            else
            {
                Debug.LogError($"錯誤：{request.error}");
            }
        }
    }

    [System.Serializable]
    private class IsLeaderResponse
    {
        public string isLeader;  // Y / N
    }
}