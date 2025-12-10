using UnityEngine;
using SpatialSys.UnitySDK;
using System;
using System.Collections.Generic;

namespace Emily.Scripts
{
    /// <summary>
    /// 管理學生資料的靜態類別。
    /// 從 Spatial Display Name 解析資料，格式：「組別 姓名」
    /// </summary>
    public static class StudentData
    {
        // DataStore Keys
        private const string KEY_COINS = "coins";
        private const string KEY_QUIZ_SCORE = "quiz_score";
        private const string KEY_SCORES = "scores";

        // 本地快取
        private static string _cachedGroupNumber = null;
        private static string _cachedStudentName = null;
        private static int _cachedCoins = 0;
        private static bool _isInitialized = false;
        
        // 成績快取 (MB, CPU, RAM, SSD - TestManager 用)
        private static Dictionary<string, int> _cachedScores = new Dictionary<string, int>();
        
        // 測驗總分 (QuizManager 10題測驗用)
        private static int _quizTotalScore = 0;

        #region Initialization

        /// <summary>
        /// 初始化：從 Spatial Display Name 解析學生資訊
        /// </summary>
        public static void Initialize(Action<bool> onComplete = null)
        {
            if (_isInitialized)
            {
                onComplete?.Invoke(IsValidFormat);
                return;
            }

            // 讀取 Spatial Display Name
            string displayName = SpatialBridge.actorService.localActor.displayName;
            ParseDisplayName(displayName);

            // 載入金幣資料
            var coinsRequest = SpatialBridge.userWorldDataStoreService.GetVariable(KEY_COINS, 0);
            coinsRequest.SetCompletedEvent((r) =>
            {
                _cachedCoins = r.intValue;
                
                // 載入測驗成績
                var scoreRequest = SpatialBridge.userWorldDataStoreService.GetVariable(KEY_QUIZ_SCORE, 0);
                scoreRequest.SetCompletedEvent((sr) =>
                {
                    _quizTotalScore = sr.intValue;
                    _isInitialized = true;
                    onComplete?.Invoke(IsValidFormat);
                });
            });
        }

        /// <summary>
        /// 解析 Display Name (格式：「組別 姓名」)
        /// </summary>
        private static void ParseDisplayName(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                Debug.LogWarning("[StudentData] Display Name 是空的！");
                return;
            }

            // 移除多餘空格並分割
            string[] parts = displayName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 2)
            {
                _cachedGroupNumber = parts[0];
                _cachedStudentName = string.Join(" ", parts, 1, parts.Length - 1);
                
                Debug.Log($"[StudentData] 解析成功 - 組別: {_cachedGroupNumber}, 姓名: {_cachedStudentName}");
            }
            else
            {
                Debug.LogError($"[StudentData] Display Name 格式錯誤！應為「組別 姓名」，實際為: {displayName}");
            }
        }

        #endregion

        #region Properties

        public static string GroupNumber => _cachedGroupNumber ?? "";
        public static string StudentName => _cachedStudentName ?? "";
        public static int Coins => _cachedCoins;
        
        // 個人貢獻金額（用於提交到 Google Sheets）
        private static int _personalContribution = 0;
        public static int PersonalContribution => _personalContribution;
        
        /// <summary>
        /// 檢查 StudentData 是否已初始化
        /// </summary>
        public static bool IsInitialized => _isInitialized;
        
        /// <summary>
        /// 檢查 Display Name 格式是否正確
        /// </summary>
        public static bool IsValidFormat => !string.IsNullOrEmpty(_cachedGroupNumber) && !string.IsNullOrEmpty(_cachedStudentName);

        #endregion

        #region Coin Management
        
        /// <summary>
        /// 金幣變更事件 (新金幣數量)
        /// </summary>
        public static event Action<int> OnCoinsChanged;

        /// <summary>
        /// 增加金幣
        /// </summary>
        public static void AddCoins(int amount, Action<bool> onComplete = null)
        {
            _cachedCoins += amount;
            var request = SpatialBridge.userWorldDataStoreService.SetVariable(KEY_COINS, _cachedCoins);
            request.SetCompletedEvent((r) => {
                if (r.succeeded)
                {
                    // 觸發金幣變更事件
                    OnCoinsChanged?.Invoke(_cachedCoins);
                }
                onComplete?.Invoke(r.succeeded);
            });
        }

        /// <summary>
        /// 扣除金幣
        /// </summary>
        public static bool SpendCoins(int amount, Action<bool> onComplete = null)
        {
            if (_cachedCoins >= amount)
            {
                _cachedCoins -= amount;
                var request = SpatialBridge.userWorldDataStoreService.SetVariable(KEY_COINS, _cachedCoins);
                request.SetCompletedEvent((r) => {
                    if (r.succeeded)
                    {
                        // 觸發金幣變更事件
                        OnCoinsChanged?.Invoke(_cachedCoins);
                    }
                    onComplete?.Invoke(r.succeeded);
                });
                return true;
            }
            onComplete?.Invoke(false);
            return false;
        }

        /// <summary>
        /// 清除金幣資料（測試用）
        /// </summary>
        public static void ClearCoins(Action<bool> onComplete = null)
        {
            _cachedCoins = 0;
            var request = SpatialBridge.userWorldDataStoreService.SetVariable(KEY_COINS, 0);
            request.SetCompletedEvent((r) => onComplete?.Invoke(r.succeeded));
        }

        #endregion

        #region Score Management

        /// <summary>
        /// 更新測驗成績
        /// </summary>
        public static void UpdateScore(string subject, int score)
        {
            _cachedScores[subject] = score;
            Debug.Log($"[StudentData] 成績更新 - {subject}: {score}");
        }

        /// <summary>
        /// 取得特定科目成績
        /// </summary>
        public static int GetScore(string subject)
        {
            return _cachedScores.ContainsKey(subject) ? _cachedScores[subject] : -1;
        }

        /// <summary>
        /// 取得所有成績
        /// </summary>
        public static Dictionary<string, int> GetAllScores()
        {
            return new Dictionary<string, int>(_cachedScores);
        }

        /// <summary>
        /// 取得成績或顯示 "-"
        /// </summary>
        public static string GetScoreOrNA(string subject)
        {
            return _cachedScores.ContainsKey(subject) ? _cachedScores[subject].ToString() : "-";
        }
        
        /// <summary>
        /// 設定測驗總分 (QuizManager 用)
        /// 儲存到 DataStore 以支援跨場景使用
        /// </summary>
        public static void SetQuizScore(int score)
        {
            _quizTotalScore = score;
            
            // 儲存到 DataStore
            var request = SpatialBridge.userWorldDataStoreService.SetVariable(KEY_QUIZ_SCORE, score);
            request.SetCompletedEvent((r) =>
            {
                if (r.succeeded)
                {
                    Debug.Log($"[StudentData] 測驗總分已存到 DataStore: {score}");
                }
                else
                {
                    Debug.LogError($"[StudentData] 測驗總分存檔失敗: {score}");
                }
            });
        }
        
        /// <summary>
        /// 取得測驗總分
        /// </summary>
        public static int QuizScore => _quizTotalScore;

        #endregion

        #region Submission

        /// <summary>
        /// 建構 Google Apps Script 提交頁面 URL
        /// 傳送: 學生資料 + 8個獨立的元件等級參數
        /// URL格式: ?group=1&name=xxx&score=80&coins=100&case=2&mb=2&cpu=3&cooler=2&ram=1&ssd=0&gpu=2&psu=0
        /// </summary>
        public static string BuildSubmissionURL(string baseURL, Dictionary<string, int> tiers)
        {
            string url = baseURL + "?";
            url += $"group={Uri.EscapeDataString(GroupNumber)}";
            url += $"&name={Uri.EscapeDataString(StudentName)}";
            url += $"&score={QuizScore}";
            url += $"&coins={Coins}";
            
            // 添加8個獨立的元件等級參數 (用小寫避免編碼問題)
            if (tiers != null)
            {
                url += $"&case={tiers.GetValueOrDefault("Case", 0)}";
                url += $"&mb={tiers.GetValueOrDefault("MB", 0)}";
                url += $"&cpu={tiers.GetValueOrDefault("CPU", 0)}";
                url += $"&cooler={tiers.GetValueOrDefault("Cooler", 0)}";
                url += $"&ram={tiers.GetValueOrDefault("RAM", 0)}";
                url += $"&ssd={tiers.GetValueOrDefault("SSD", 0)}";
                url += $"&gpu={tiers.GetValueOrDefault("GPU", 0)}";
                url += $"&psu={tiers.GetValueOrDefault("PSU", 0)}";
            }
            
            return url;
        }
        
        /// <summary>
        /// 設定個人貢獻金額
        /// </summary>
        public static void SetPersonalContribution(int amount)
        {
            _personalContribution = amount;
            Debug.Log($"[StudentData] 設定個人貢獻: {amount}");
        }

        #endregion
    }
}
