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
        string playerName = SpatialBridge.actorService.localActor.displayName.Split(' ')[1];
        CheckIsLeader(playerName);
    }

    private async void CheckIsLeader(string name)
    {
        string url = $"https://script.google.com/macros/s/AKfycbwVKSdMOP-b8GEt_v1DQCtNXMes86mWpVae0BndvF6KPo9CHg87b2sfkXA3YdkM_ZNZ/exec?name={name}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            var op = request.SendWebRequest();
            while (!op.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                bool isLeader = JsonUtility.FromJson<IsLeaderResponse>(json).isLeader;

                if (isLeader)
                {
                    Debug.Log("✅ 是組長");
                }
                else
                {
                    ulong currentBalance = SpatialBridge.inventoryService.worldCurrencyBalance;
                }
            }
        }
    }

    [System.Serializable]
    private class IsLeaderResponse
    {
        public bool isLeader;
    }
}