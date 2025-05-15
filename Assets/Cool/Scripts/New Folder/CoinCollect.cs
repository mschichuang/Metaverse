using UnityEngine;
using SpatialSys.UnitySDK;

public class CoinCollect : MonoBehaviour
{
    public float rotateSpeed = 90f; // 每秒旋轉角度（繞 Z 軸）
    public ulong rewardAmount = 1000; // 獎勵金額

    [HideInInspector]
    public CoinLineConnector connectedLine; // 由 CoinLineConnector 設定

    void Update()
    {
        // 讓金幣繞 Z 軸旋轉（像立起來的硬幣轉動）
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果是 Avatar 玩家碰到
        if (other.gameObject.name.Contains("Avatar"))
        {
            Debug.Log($"玩家觸發金幣 {gameObject.name}，發送獎勵！");
            SpatialBridge.inventoryService.AwardWorldCurrency(rewardAmount);

            // 通知線條（如果有）
            if (connectedLine != null)
            {
                Debug.Log("通知線條其中一端金幣被收集！");
                connectedLine.NotifyCoinCollected(this);
            }

            // 銅板消失
            Destroy(gameObject);
        }
    }
}
