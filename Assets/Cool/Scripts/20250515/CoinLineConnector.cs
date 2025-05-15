using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CoinLineConnector : MonoBehaviour
{
    public Transform pointA;         // 第一顆金幣（Transform）
    public Transform pointB;         // 第二顆金幣（Transform）

    private CoinCollect coinA;
    private CoinCollect coinB;

    private bool coinACollected = false;
    private bool coinBCollected = false;

    private LineRenderer lr;

    void Start()
    {
        // ========  防呆檢查  ========
        if (pointA == null || pointB == null)
        {
            Debug.LogError($"[CoinLineConnector] {name}：pointA 或 pointB 尚未指派！已停用腳本以避免錯誤。");
            enabled = false;                           // 停用此腳本
            return;
        }
        // ===========================

        lr = GetComponent<LineRenderer>();
        coinA = pointA.GetComponent<CoinCollect>();
        coinB = pointB.GetComponent<CoinCollect>();

        if (coinA != null)  coinA.connectedLine = this;
        if (coinB != null)  coinB.connectedLine = this;
    }

    void Update()
    {
        // 若其中一端已被回收（Transform 為 null），就先退出，不再嘗試更新座標
        if (pointA == null || pointB == null || lr == null)
            return;

        lr.SetPosition(0, pointA.position);
        lr.SetPosition(1, pointB.position);
    }

    /// <summary>被其中一顆金幣呼叫，通知此端已收集。</summary>
    public void NotifyCoinCollected(CoinCollect collectedCoin)
    {
        if (collectedCoin == coinA)  coinACollected = true;
        if (collectedCoin == coinB)  coinBCollected = true;

        // 兩端都收集後再關閉線條
        if (coinACollected && coinBCollected)
        {
            Debug.Log($"[CoinLineConnector] 兩端金幣已收集，隱藏線條：{name}");
            gameObject.SetActive(false);               // 或 Destroy(gameObject);
        }
    }
}
