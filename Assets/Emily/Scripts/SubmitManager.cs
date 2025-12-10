using UnityEngine;
using SpatialSys.UnitySDK;
using Emily.Scripts;
using System.Collections.Generic;

/// <summary>
/// 提交管理器 - 可用於測驗區或組裝區
/// 透過 Inspector 設定 submitMode 來決定提交類型
/// </summary>
public class SubmitManager : MonoBehaviour
{
    private const string GOOGLE_SCRIPT_URL = "https://script.google.com/macros/s/AKfycbx_dFr08pDSFm22YGbXq6GJGAAuNmhY228cUkbz-WyuUWB68DUgFS2WxIy5191Pi-2f/exec";
    
    public enum SubmitMode
    {
        Quiz,       // 測驗區：提交個人成績
        Assembly    // 組裝區：提交小組組裝
    }
    
    [Header("提交設定")]
    [Tooltip("Quiz = 測驗區（個人成績）\nAssembly = 組裝區（小組組裝）")]
    public SubmitMode submitMode = SubmitMode.Quiz;
    
    [Header("提交矩陣物件")]
    [Tooltip("提交後要隱藏的 GameObject（通常是整個提交矩陣）")]
    public GameObject submitMatrix;
    
    void Start()
    {
        // 只有組裝區需要監聽同步隱藏事件
        if (submitMode == SubmitMode.Assembly && GroupCoinManager.Instance != null)
        {
            GroupCoinManager.Instance.OnGroupSubmitted += OnGroupSubmitted;
            
            // 檢查是否已經提交過（其他組員先提交的情況）
            if (GroupCoinManager.Instance.IsGroupSubmitted())
            {
                HideSubmitMatrix();
            }
        }
    }
    
    void OnDestroy()
    {
        if (submitMode == SubmitMode.Assembly && GroupCoinManager.Instance != null)
        {
            GroupCoinManager.Instance.OnGroupSubmitted -= OnGroupSubmitted;
        }
    }
    
    /// <summary>
    /// 當組別已提交事件觸發時，隱藏提交矩陣（僅組裝區使用）
    /// </summary>
    private void OnGroupSubmitted()
    {
        HideSubmitMatrix();
    }
    
    /// <summary>
    /// 隱藏提交矩陣
    /// </summary>
    private void HideSubmitMatrix()
    {
        if (submitMatrix != null)
        {
            submitMatrix.SetActive(false);
        }
        else
        {
            // 如果沒設定，隱藏自己
            gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 提交 - 在 Interactable 的 On Interact Event 設定此方法
    /// 根據 submitMode 決定提交類型
    /// </summary>
    public void OnSubmitClicked()
    {
        string url;
        
        if (submitMode == SubmitMode.Quiz)
        {
            // 測驗區：提交個人成績
            url = StudentData.BuildQuizSubmissionURL(GOOGLE_SCRIPT_URL);
        }
        else
        {
            // 組裝區：提交小組組裝
            var tiers = new Dictionary<string, int>();
            var historyManager = FindObjectOfType<PurchaseHistoryManager>();
            if (historyManager != null)
            {
                tiers = historyManager.GetAllTiers();
            }
            url = StudentData.BuildAssemblySubmissionURL(GOOGLE_SCRIPT_URL, tiers);
            
            // 設定組別已提交（同步通知所有同組組員）
            if (GroupCoinManager.Instance != null)
            {
                GroupCoinManager.Instance.SetGroupSubmitted();
            }
        }
        
        // 開啟瀏覽器
        SpatialBridge.spaceService.OpenURL(url);
        
        // 隱藏提交矩陣
        HideSubmitMatrix();
    }
}
