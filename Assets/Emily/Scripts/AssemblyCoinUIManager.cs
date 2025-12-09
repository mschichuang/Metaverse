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
    
    [Header("個人貢獻 UI")]
    public TMP_Text personalCoinText;
    
    private GroupCoinManager groupCoinManager;
    private bool isInitialized = false;
    
    void Start()
    {
        // 等待 GroupCoinManager 初始化
        StartCoroutine(WaitForGroupCoinManager());
        
        // 同時讀取個人貢獻（從 DataStore）
        LoadPersonalContribution();
    }
    
    /// <summary>
    /// 從 DataStore 讀取個人貢獻金額
    /// </summary>
    private void LoadPersonalContribution()
    {
        // 確保 StudentData 已初始化
        if (!StudentData.IsInitialized)
        {
            StudentData.Initialize((success) =>
            {
                if (success)
                {
                    // 讀取並顯示個人金幣（這是進入組裝區前的金幣，即個人貢獻）
                    UpdatePersonalContributionUI(StudentData.PersonalContribution);
                }
            });
        }
        else
        {
            UpdatePersonalContributionUI(StudentData.PersonalContribution);
        }
    }
    
    private System.Collections.IEnumerator WaitForGroupCoinManager()
    {
        // 等待 GroupCoinManager 實例（最多等 3 秒）
        float timeout = 3f;
        while (GroupCoinManager.Instance == null && timeout > 0)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }
        
        if (GroupCoinManager.Instance == null)
        {
            Debug.LogWarning("[AssemblyCoinUIManager] GroupCoinManager 未找到");
            yield break;
        }
        
        groupCoinManager = GroupCoinManager.Instance;
        
        // 訂閱金幣變更事件
        groupCoinManager.OnGroupCoinsChanged += UpdateGroupCoinsUI;
        
        // 立即顯示（不再延遲）
        UpdateGroupCoinsUI(groupCoinManager.GetGroupCoins());
        
        // 如果 GroupCoinManager 有個人貢獻記錄，也更新
        if (groupCoinManager.PersonalContribution > 0)
        {
            UpdatePersonalContributionUI(groupCoinManager.PersonalContribution);
        }
        
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
    /// 更新個人貢獻 UI
    /// </summary>
    private void UpdatePersonalContributionUI(int contribution)
    {
        if (personalCoinText != null)
        {
            personalCoinText.text = contribution.ToString("N0");
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
            UpdatePersonalContributionUI(groupCoinManager.PersonalContribution);
        }
        else
        {
            // GroupCoinManager 還沒準備好，從 StudentData 讀取
            UpdatePersonalContributionUI(StudentData.PersonalContribution);
        }
    }
    
    /// <summary>
    /// 獲取當前組別金幣
    /// </summary>
    public int GetCurrentGroupCoins()
    {
        return groupCoinManager?.GetGroupCoins() ?? 0;
    }
    
    /// <summary>
    /// 獲取個人貢獻金額
    /// </summary>
    public int GetPersonalContribution()
    {
        return groupCoinManager?.PersonalContribution ?? StudentData.PersonalContribution;
    }
    
    void OnDestroy()
    {
        if (groupCoinManager != null)
        {
            groupCoinManager.OnGroupCoinsChanged -= UpdateGroupCoinsUI;
        }
    }
}
