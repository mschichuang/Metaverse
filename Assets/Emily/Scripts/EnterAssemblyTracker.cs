using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Threading.Tasks;

public class EnterAssemblyTracker : MonoBehaviour
{
    public async void OnInteract()
    {
        string playerName = PlayerInfoManager.GetPlayerName();
        string json = $"{{\"name\":\"{playerName}\"}}";
        string url = PlayerInfoManager.Url + "?action=enterAssembly";
        
        using UnityWebRequest request = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");

        request.SendWebRequest();
        while (!request.isDone)
            await Task.Yield();
    }
}