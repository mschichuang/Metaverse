using UnityEngine;
using SpatialSys.UnitySDK;

public class CoinCollect : MonoBehaviour
{
    public float rotateSpeed = 90f; // 每秒旋轉角度
    public ulong rewardAmount = 1000; // 獎勵金幣數量

    void Update()
    {
        // 讓金幣繞 Y 軸旋轉（像立起來的硬幣轉動）
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 印出誰碰到金幣，幫助除錯
        Debug.Log("碰到金幣的物體名稱：" + other.gameObject.name);

        // 如果是玩家碰到，就給錢並刪除金幣
        if (other.gameObject.name.Contains("Avatar"))
        {
            Debug.Log("玩家觸發金幣，發送獎勵！");
            SpatialBridge.inventoryService.AwardWorldCurrency(rewardAmount);
            Destroy(gameObject);
        }
    }
}
