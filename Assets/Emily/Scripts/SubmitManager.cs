using UnityEngine;
using SpatialSys.UnitySDK;
using Emily.Scripts;
using System.Collections.Generic;

/// <summary>
/// 成績提交管理器 - 組裝區提交學生資料到 Google Sheets
/// </summary>
public class SubmitManager : MonoBehaviour
{
    private const string GOOGLE_SCRIPT_URL = "https://script.google.com/macros/s/AKfycbx_dFr08pDSFm22YGbXq6GJGAAuNmhY228cUkbz-WyuUWB68DUgFS2WxIy5191Pi-2f/exec";
    
    /// <summary>
    /// 提交成績 - 在 Interactable 的 On Interact Event 設定此方法
    /// </summary>
    public void OnSubmitClicked()
    {
        // 取得所有元件的等級
        var tiers = new Dictionary<string, int>();
        var historyManager = FindObjectOfType<PurchaseHistoryManager>();
        if (historyManager != null)
        {
            tiers = historyManager.GetAllTiers();
        }

        // 開啟提交頁面
        string url = StudentData.BuildSubmissionURL(GOOGLE_SCRIPT_URL, tiers);
        SpatialBridge.spaceService.OpenURL(url);
    }
}
