using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class TransferDiamondManager : MonoBehaviour
{
    public GameObject transferDiamond;

    private async void Start()
    {
        transferDiamond.SetActive(false);
        await CheckIsLeader();
    }

    private async Task CheckIsLeader()
    {
        string playerName = PlayerInfoManager.GetPlayerName();
        string url = $"{PlayerInfoManager.Url}?action=checkIsLeader&name={playerName}";

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