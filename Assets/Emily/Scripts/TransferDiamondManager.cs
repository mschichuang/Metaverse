using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class TransferDiamondManager : MonoBehaviour
{
    public GameObject transferDiamond;

    private void Start()
    {
        transferDiamond.SetActive(false);
        _ = CheckIsLeader();
    }

    private async Task CheckIsLeader()
    {
        string playerName = PlayerInfoManager.GetPlayerName();
        string url = $"{PlayerInfoManager.Url}?action=checkIsLeader&name={UnityWebRequest.EscapeURL(playerName)}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SendWebRequest();
            while (!request.isDone)
                await Task.Yield();

            string json = request.downloadHandler.text;
            LeaderResponse data = JsonUtility.FromJson<LeaderResponse>(json);
            if (data.isLeader == "Y")
            {
                transferDiamond.SetActive(true);
            }
        }
    }

    [System.Serializable]
    private class LeaderResponse
    {
        public string isLeader;
    }
}