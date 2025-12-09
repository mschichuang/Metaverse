using UnityEngine;
using SpatialSys.UnitySDK;
using System;
using Emily.Scripts;

/// <summary>
/// 組別共用金幣池管理器
/// 使用 SpatialNetworkBehaviour 和 NetworkVariable 實現玩家間金幣同步
/// 在組裝區場景中使用，需要掛載在有 SpatialNetworkObject 的 GameObject 上
/// </summary>
public class GroupCoinManager : SpatialNetworkBehaviour, IVariablesChanged
{
    // 各組的共用金幣池（最多支援 10 組）
    public NetworkVariable<int> group1Coins = new NetworkVariable<int>(0);
    public NetworkVariable<int> group2Coins = new NetworkVariable<int>(0);
    public NetworkVariable<int> group3Coins = new NetworkVariable<int>(0);
    public NetworkVariable<int> group4Coins = new NetworkVariable<int>(0);
    public NetworkVariable<int> group5Coins = new NetworkVariable<int>(0);
    public NetworkVariable<int> group6Coins = new NetworkVariable<int>(0);
    public NetworkVariable<int> group7Coins = new NetworkVariable<int>(0);
    public NetworkVariable<int> group8Coins = new NetworkVariable<int>(0);
    public NetworkVariable<int> group9Coins = new NetworkVariable<int>(0);
    public NetworkVariable<int> group10Coins = new NetworkVariable<int>(0);
    
    // 單例實例（方便其他腳本存取）
    public static GroupCoinManager Instance { get; private set; }
    
    // 金幣變更事件（用於更新 UI）
    public event Action<int> OnGroupCoinsChanged;
    
    // 記錄本地玩家是否已轉換金幣
    private bool hasTransferred = false;
    
    // 個人貢獻金額（本地記錄）
    private int personalContribution = 0;
    public int PersonalContribution => personalContribution;
    
    // 確保 NetworkObject 已 Spawn 且變數可存取
    public bool IsReady { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("[GroupCoinManager] 已存在實例！");
        }
    }
    
    public override void Spawned()
    {
        base.Spawned();
        Debug.Log("[GroupCoinManager] Network Object 已 Spawn");
        IsReady = true;
        
        // 自動轉換個人金幣到組別金幣池
        TransferPersonalCoinsToGroup();
    }
    
    /// <summary>
    /// 當 NetworkVariable 變更時觸發（包括其他玩家的修改）
    /// </summary>
    public void OnVariablesChanged(NetworkObjectVariablesChangedEventArgs args)
    {
        // 通知 UI 更新
        int currentGroupCoins = GetGroupCoins();
        OnGroupCoinsChanged?.Invoke(currentGroupCoins);
        Debug.Log($"[GroupCoinManager] 金幣變更通知: {currentGroupCoins}");
    }
    
    /// <summary>
    /// 進入組裝區時，將個人金幣轉換到組別金幣池
    /// </summary>
    public void TransferPersonalCoinsToGroup()
    {
        if (hasTransferred)
        {
            Debug.Log("[GroupCoinManager] 已經轉換過金幣");
            return;
        }
        
        // 確保 StudentData 已初始化
        if (!StudentData.IsInitialized)
        {
            StudentData.Initialize((success) =>
            {
                if (success)
                {
                    DoTransfer();
                }
                else
                {
                    Debug.LogError("[GroupCoinManager] StudentData 初始化失敗！");
                }
            });
        }
        else
        {
            DoTransfer();
        }
    }
    
    private void DoTransfer()
    {
        int personalCoins = StudentData.Coins;
        
        if (personalCoins <= 0)
        {
            Debug.Log("[GroupCoinManager] 個人金幣為 0，不需轉換");
            hasTransferred = true;
            return;
        }
        
        // 記錄個人貢獻
        personalContribution = personalCoins;
        
        // 取得擁有權才能修改 NetworkVariable
        networkObject.RequestOwnership();
        
        // 增加到組別金幣池
        string groupNumber = StudentData.GroupNumber;
        NetworkVariable<int> groupCoins = GetGroupCoinsVariable(groupNumber);
        
        if (groupCoins != null)
        {
            groupCoins.value += personalCoins;
            Debug.Log($"[GroupCoinManager] 轉換成功！組別: {groupNumber}, 貢獻: {personalCoins}, 組別總金幣: {groupCoins.value}");
        }
        
        // 清空個人金幣
        StudentData.ClearCoins((cleared) =>
        {
            if (cleared)
            {
                hasTransferred = true;
                Debug.Log("[GroupCoinManager] 個人金幣已清空");
            }
        });
        
        // 記錄個人貢獻到 DataStore（用於提交）
        StudentData.SetPersonalContribution(personalContribution);
    }
    
    /// <summary>
    /// 獲取當前玩家組別的金幣池
    /// </summary>
    public int GetGroupCoins()
    {
        string groupNumber = StudentData.GroupNumber;
        NetworkVariable<int> groupCoins = GetGroupCoinsVariable(groupNumber);
        return groupCoins?.value ?? 0;
    }
    
    /// <summary>
    /// 扣除組別金幣（購買時使用）
    /// </summary>
    public bool SpendGroupCoins(int amount)
    {
        string groupNumber = StudentData.GroupNumber;
        NetworkVariable<int> groupCoins = GetGroupCoinsVariable(groupNumber);
        
        if (groupCoins == null)
        {
            Debug.LogError("[GroupCoinManager] 找不到組別金幣池");
            return false;
        }
        
        if (groupCoins.value < amount)
        {
            Debug.LogWarning($"[GroupCoinManager] 金幣不足！當前: {groupCoins.value}, 需要: {amount}");
            return false;
        }
        
        // 取得擁有權才能修改
        networkObject.RequestOwnership();
        groupCoins.value -= amount;
        
        Debug.Log($"[GroupCoinManager] 扣除 {amount}，剩餘: {groupCoins.value}");
        return true;
    }
    
    /// <summary>
    /// 增加組別金幣（退款時使用）
    /// </summary>
    public void AddGroupCoins(int amount)
    {
        string groupNumber = StudentData.GroupNumber;
        NetworkVariable<int> groupCoins = GetGroupCoinsVariable(groupNumber);
        
        if (groupCoins != null)
        {
            networkObject.RequestOwnership();
            groupCoins.value += amount;
            Debug.Log($"[GroupCoinManager] 增加 {amount}，總計: {groupCoins.value}");
        }
    }
    
    /// <summary>
    /// 根據組別編號獲取對應的 NetworkVariable
    /// </summary>
    private NetworkVariable<int> GetGroupCoinsVariable(string groupNumber)
    {
        switch (groupNumber)
        {
            case "1": return group1Coins;
            case "2": return group2Coins;
            case "3": return group3Coins;
            case "4": return group4Coins;
            case "5": return group5Coins;
            case "6": return group6Coins;
            case "7": return group7Coins;
            case "8": return group8Coins;
            case "9": return group9Coins;
            case "10": return group10Coins;
            default:
#if UNITY_EDITOR
                // 在編輯器測試時，如果找不到組別，預設使用第一組，避免報錯卡住
                Debug.LogWarning($"[GroupCoinManager] (Editor Only) 不支援的組別編號: {groupNumber}，自動切換至 Group 1 進行測試。");
                return group1Coins;
#else
                Debug.LogError($"[GroupCoinManager] 不支援的組別編號: {groupNumber}");
                return null;
#endif
        }
    }
    
    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
