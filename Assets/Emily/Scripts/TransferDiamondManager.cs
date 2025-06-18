using UnityEngine;
using UnityEngine.Networking;
using SpatialSys.UnitySDK;
using System.Threading.Tasks;

public class TransferDiamondManager : MonoBehaviour
{
    public GameObject transferDiamond;
    private string playerName;

    private async void Start()
    {
        playerName = SpatialBridge.actorService.localActor.displayName.Split(' ')[1];
        transferDiamond.SetActive(false);

        await CheckIsLeader();
    }

    private async Task CheckIsLeader()
    {
        string url = $"https://script.google.com/macros/s/AKfycbyQD56ArfGkOuYfa-RRqYFPbSDLbSdsU98UWw86XBcjPaQ4NJ9GhegNnocDrX5hdlfZ/exec?action=checkIsLeader&name={playerName}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SendWebRequest();
            while (!request.isDone)
                await Task.Yield();

            string json = request.downloadHandler.text;
            LeaderResponse data = JsonUtility.FromJson<LeaderResponse>(json);

            transferDiamond.SetActive(data.isLeader == "Y");
        }
    }

    [System.Serializable]
    private class LeaderResponse
    {
        public string isLeader;
    }
}