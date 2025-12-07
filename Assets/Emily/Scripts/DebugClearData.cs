using UnityEngine;
using UnityEngine.UI;
using SpatialSys.UnitySDK;

namespace Emily.Scripts
{
    /// <summary>
    /// Debug 工具：清除學生登入資料並返回登入畫面
    /// 僅用於開發測試，正式版本應移除此腳本
    /// </summary>
    public class DebugClearData : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("LoginUI 腳本 (需手動指定)")]
        public LoginUI loginUI;

        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(ClearAndShowLogin);
            }
        }

        private void ClearAndShowLogin()
        {
            Debug.Log("[DebugClearData] 清除資料並返回登入畫面...");
            
            // 清除本地快取
            StudentData.ClearAllData(null);

            // 隱藏 Quest UI
            SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.QuestSystem, false);

            // 重置登入表單
            if (loginUI != null)
            {
                loginUI.ResetLoginForm();
            }

            Debug.Log("[DebugClearData] 已返回登入畫面！");
        }
    }
}
