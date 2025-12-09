using UnityEngine;
using TMPro;
using Emily.Scripts;

/// <summary>
/// 組裝區的金幣 UI 管理器
/// 顯示組別共用金幣池（使用 GroupCoinManager）
/// </summary>
public class AssemblyCoinUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text groupCoinText;
    
    [Header("可選：顯示個人貢獻")]
    public TMP_Text contributionText;
    
    private GroupCoinManager groupCoinManager;
    
    void Start()
    {
        // 等待 GroupCoinManager 初始化
        StartCoroutine(WaitForGroupCoinManager());
    }
    
    private System.Collections.IEnumerator WaitForGroupCoinManager()
    {
        // 等待 GroupCoinManager 實例
        while (GroupCoinManager.Instance == null)
        {
            yield return null;
        }
        
        groupCoinManager = GroupCoinManager.Instance;
        
        // 訂閱金幣變更事件
        groupCoinManager.OnGroupCoinsChanged += UpdateGroupCoinsUI;
        
        // 初始顯示
        yield return new WaitForSeconds(0.5f); // 等待金幣轉換完成
        UpdateGroupCoinsUI(groupCoinManager.GetGroupCoins());
        
        // 顯示個人貢獻
        if (contributionText != null)
        {
            contributionText.text = $"個人貢獻: {groupCoinManager.PersonalContribution:N0}";
        }
    }
    
    private void UpdateGroupCoinsUI(int coins)
    {
        if (groupCoinText != null)
        {
            groupCoinText.text = coins.ToString("N0");
        }
        
        Debug.Log($"[AssemblyCoinUIManager] 更新 UI: {coins}");
    }
    
    /// <summary>
    /// 手動刷新金幣顯示
    /// </summary>
    public void RefreshCoins()
    {
        if (groupCoinManager != null)
        {
            UpdateGroupCoinsUI(groupCoinManager.GetGroupCoins());
        }
    }
    
    /// <summary>
    /// 獲取當前組別金幣
    /// </summary>
    public int GetCurrentCoins()
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
