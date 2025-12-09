using UnityEngine;
using SpatialSys.UnitySDK;
using Emily.Scripts;

/// <summary>
/// 成績提交管理器
/// 用於組裝區結束後提交學生資料到 Google Sheets
/// </summary>
public class SubmitManager : MonoBehaviour
{
    [Tooltip("提交按鈕（提交後會隱藏）")]
    public GameObject submitButton;
    
    // Google Apps Script Web App URL
    private const string GOOGLE_SCRIPT_URL = "https://script.google.com/macros/s/AKfycbx_dFr08pDSFm22YGbXq6GJGAAuNmhY228cUkbz-WyuUWB68DUgFS2WxIy5191Pi-2f/exec";
    
    /// <summary>
    /// 提交成績到 Google Sheets
    /// 在 Interactable 的 On Interact Event 設定此方法
    /// </summary>
    public void OnSubmitClicked()
    {
        // 建構提交 URL
        string url = StudentData.BuildSubmissionURL(GOOGLE_SCRIPT_URL);
        
        Debug.Log($"[SubmitManager] 提交: 組別={StudentData.GroupNumber}, 姓名={StudentData.StudentName}, 成績={StudentData.QuizScore}, 貢獻={StudentData.PersonalContribution}");
        
        // 開啟瀏覽器
        SpatialBridge.spaceService.OpenURL(url);
        
        // 隱藏提交按鈕
        if (submitButton != null)
        {
            submitButton.SetActive(false);
        }
    }
}
