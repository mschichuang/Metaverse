using UnityEngine;
using SpatialSys.UnitySDK;

public class TransferCoins : MonoBehaviour
{
    public GameObject transferDiamond;

    public void OnTransferTriggered()
    {
        transferDiamond.SetActive(false);

        string playerName = SpatialBridge.actorService.localActor.displayName.Split(' ')[1];

        bool isLeader = false; // ← 之後改成從表單取得
    }
}