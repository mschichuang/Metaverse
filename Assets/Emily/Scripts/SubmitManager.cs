using UnityEngine;
using SpatialSys.UnitySDK;
using Emily.Scripts;
using System.Collections.Generic;

/// <summary>
/// 成績提交管理器
/// 用於組裝區結束後提交學生資料到 Google Sheets
/// </summary>
public class SubmitManager : MonoBehaviour
{
    [Tooltip("提交按鈕（提交後會隱藏）")]
    public GameObject submissionMatrix;
    
    // Google Apps Script Web App URL
    private const string GOOGLE_SCRIPT_URL = "https://script.google.com/macros/s/AKfycbx_dFr08pDSFm22YGbXq6GJGAAuNmhY228cUkbz-WyuUWB68DUgFS2WxIy5191Pi-2f/exec";
    
    /// <summary>
    /// 提交成績到 Google Sheets
    /// 在 Interactable 的 On Interact Event 設定此方法
    /// </summary>
    public void OnSubmitClicked()
    {
        // 取得所有元件的等級 (8個獨立參數)
        Dictionary<string, int> tiers = new Dictionary<string, int>();
        PurchaseHistoryManager historyManager = FindObjectOfType<PurchaseHistoryManager>();
        if (historyManager != null)
        {
            tiers = historyManager.GetAllTiers();
        }

        // 建構提交 URL (8個獨立參數)
        string url = StudentData.BuildSubmissionURL(GOOGLE_SCRIPT_URL, tiers);
        
        // Debug
        Debug.Log($"[SubmitManager] URL: {url}");
        
        // 開啟瀏覽器
        SpatialBridge.spaceService.OpenURL(url);
        
        // 隱藏提交物件
        if (submissionMatrix != null)
        {
            submissionMatrix.SetActive(false);
        }
    }
}
