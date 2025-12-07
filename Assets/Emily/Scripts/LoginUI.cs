using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Emily.Scripts
{
    /// <summary>
    /// 登入 UI 控制器。
    /// 掛載於導覽區場景，負責收集學生資訊並儲存至 Spatial DataStore。
    /// </summary>
    public class LoginUI : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("登入面板 (全螢幕遮罩)")]
        public GameObject loginPanel;

        [Tooltip("學號輸入框")]
        public TMP_InputField studentIdInput;

        [Tooltip("組別輸入框")]
        public TMP_InputField groupNumberInput;

        [Tooltip("開始體驗按鈕")]
        public Button startButton;

        private void Start()
        {
            // 初始化 StudentData (從 Spatial DataStore 讀取)
            StudentData.Initialize(OnDataInitialized);
        }

        /// <summary>
        /// 資料初始化完成後的回調
        /// </summary>
        private void OnDataInitialized(bool hasExistingData)
        {
            if (hasExistingData)
            {
                // 自動登入：隱藏登入面板
                if (loginPanel != null)
                {
                    loginPanel.SetActive(false);
                }

                Debug.Log($"[LoginUI] 自動登入: {StudentData.StudentID}");
            }
            else
            {
                // 顯示登入面板
                if (loginPanel != null)
                {
                    loginPanel.SetActive(true);
                }

                // 設定按鈕事件
                if (startButton != null)
                {
                    startButton.onClick.AddListener(OnStartButtonClicked);
                    startButton.interactable = false; // 預設不可點擊
                }

                // 監聽輸入變化以驗證欄位
                if (studentIdInput != null)
                    studentIdInput.onValueChanged.AddListener(_ => ValidateInputs());
                if (groupNumberInput != null)
                    groupNumberInput.onValueChanged.AddListener(_ => ValidateInputs());
            }
        }



        /// <summary>
        /// 驗證所有輸入欄位是否已填寫
        /// </summary>
        private void ValidateInputs()
        {
            bool allFilled = !string.IsNullOrWhiteSpace(studentIdInput?.text) &&
                             !string.IsNullOrWhiteSpace(groupNumberInput?.text);

            if (startButton != null)
            {
                startButton.interactable = allFilled;
            }
        }

        /// <summary>
        /// 開始按鈕點擊事件
        /// </summary>
        private void OnStartButtonClicked()
        {
            // 鎖定按鈕防止重複點擊
            if (startButton != null) startButton.interactable = false;

            string studentId = studentIdInput.text.Trim();
            string groupNumber = groupNumberInput.text.Trim();

            // 儲存資料到 Spatial DataStore
            StudentData.SaveStudentInfo(studentId, groupNumber, OnSaveComplete);
        }

        /// <summary>
        /// 資料儲存完成後的回調
        /// </summary>
        private void OnSaveComplete(bool success)
        {
            if (success)
            {
                Debug.Log($"[LoginUI] 註冊成功: {StudentData.StudentID}, 組別: {StudentData.GroupNumber}");

                // 隱藏登入面板
                if (loginPanel != null)
                {
                    loginPanel.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("[LoginUI] 資料儲存失敗！請檢查網路連線。");
                // 重新啟用按鈕讓使用者再試
                if (startButton != null) startButton.interactable = true;
            }
        }
    }
}
