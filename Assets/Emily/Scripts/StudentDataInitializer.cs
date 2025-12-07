using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;

namespace Emily.Scripts
{
    /// <summary>
    /// 初始化學生資料並檢查格式
    /// </summary>
    public class StudentDataInitializer : MonoBehaviour
    {
        [Header("Error UI (Optional)")]
        [Tooltip("格式錯誤提示面板")]
        public GameObject errorPanel;
        
        [Tooltip("錯誤訊息文字")]
        public TMP_Text errorMessageText;

        private void Start()
        {
            // 初始化學生資料
            StudentData.Initialize(OnInitialized);
        }

        private void OnInitialized(bool isValid)
        {
            if (isValid)
            {
                // 格式正確，顯示 Quest UI
                SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.QuestSystem, true);
                
                Debug.Log($"[StudentDataInitializer] 學生資料載入成功 - 組別: {StudentData.GroupNumber}, 姓名: {StudentData.StudentName}");
            }
            else
            {
                // 格式錯誤，隱藏 Quest UI 並顯示錯誤提示
                SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.QuestSystem, false);
                
                ShowError();
            }
        }

        private void ShowError()
        {
            if (errorPanel != null)
            {
                errorPanel.SetActive(true);
            }

            if (errorMessageText != null)
            {
                errorMessageText.text = "Name 格式錯誤！\n\n請將您的 Spatial Name 設定為：\n組別 姓名\n\n範例：1 王小明";
            }

            Debug.LogError("[StudentDataInitializer] Spatial Name 格式錯誤！請設定為「組別 姓名」格式");
        }
    }
}
