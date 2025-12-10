using UnityEngine;
using TMPro;
using Emily.Scripts;

/// <summary>
/// 組裝區的金幣 UI 管理器
/// 顯示組別共用金幣池和個人貢獻
/// </summary>
public class AssemblyCoinUIManager : MonoBehaviour
{
    [Header("組別金幣 UI")]
    public TMP_Text groupCoinText;
    
    private GroupCoinManager groupCoinManager;
    private bool isInitialized = false;
    
    void Start()
    {
        // 等待 GroupCoinManager 初始化
        StartCoroutine(WaitForGroupCoinManager());
    }
    
    private System.Collections.IEnumerator WaitForGroupCoinManager()
    {
        // 等待 GroupCoinManager 實例（最多等 3 秒）
        float timeout = 3f;
        while ((GroupCoinManager.Instance == null || !GroupCoinManager.Instance.IsReady) && timeout > 0)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }
        
        if (GroupCoinManager.Instance == null)
        {
            yield break;
        }
        
        groupCoinManager = GroupCoinManager.Instance;
        
        // 訂閱金幣變更事件
        groupCoinManager.OnGroupCoinsChanged += UpdateGroupCoinsUI;
        
        // 立即顯示（不再延遲）
        UpdateGroupCoinsUI(groupCoinManager.GetGroupCoins());
        
        isInitialized = true;
    }
    
    /// <summary>
    /// 更新組別金幣 UI
    /// </summary>
    private void UpdateGroupCoinsUI(int coins)
    {
        if (groupCoinText != null)
        {
            groupCoinText.text = coins.ToString("N0");
        }
    }
    
    /// <summary>
    /// 手動刷新金幣顯示（購買後呼叫）
    /// </summary>
    public void RefreshCoins()
    {
        if (groupCoinManager != null)
        {
            UpdateGroupCoinsUI(groupCoinManager.GetGroupCoins());
        }
    }
    
    /// <summary>
    /// 強制刷新所有 UI（由 TrailerBoard 在影片結束時呼叫）
    /// </summary>
    public void ForceRefresh()
    {
        // 刷新組別金幣
        if (groupCoinManager != null)
        {
            UpdateGroupCoinsUI(groupCoinManager.GetGroupCoins());

        }
    }
    
    /// <summary>
    /// 獲取當前組別金幣
    /// </summary>
    public int GetCurrentGroupCoins()
    {
        return groupCoinManager?.GetGroupCoins() ?? 0;
    }
    
    void OnDestroy()
    {
        if (groupCoinManager != null)
        {
            groupCoinManager.OnGroupCoinsChanged -= UpdateGroupCoinsUI;
        }
    }
}
