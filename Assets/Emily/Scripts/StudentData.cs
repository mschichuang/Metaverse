using UnityEngine;
using SpatialSys.UnitySDK;
using System;

namespace Emily.Scripts
{
    /// <summary>
    /// 管理學生資料的單例類別。
    /// 使用 Spatial DataStore (雲端) 持久化儲存學生資訊。
    /// 注意：所有讀寫操作皆為非同步 (Async)。
    /// </summary>
    public static class StudentData
    {
        // DataStore Keys
        private const string KEY_STUDENT_ID = "student_id";
        private const string KEY_GROUP_NUMBER = "group_number";
        private const string KEY_COINS = "coins";

        // 本地快取 (Spatial DataStore 是雲端，存取較慢，本地快取加速讀取)
        private static string _cachedStudentID = null;
        private static string _cachedGroupNumber = null;
        private static int _cachedCoins = 0;
        private static bool _isInitialized = false;

        #region Initialization

        /// <summary>
        /// 初始化：從 Spatial DataStore 讀取資料到本地快取。
        /// 應在場景啟動時呼叫 (例如 LoginUI.Start())。
        /// </summary>
        public static void Initialize(Action<bool> onComplete = null)
        {
            if (_isInitialized)
            {
                onComplete?.Invoke(true);
                return;
            }

            // 使用 HasVariable 檢查是否有註冊資料
            var request = SpatialBridge.userWorldDataStoreService.HasVariable(KEY_STUDENT_ID);
            request.SetCompletedEvent((req) =>
            {
                if (req.hasVariable)
                {
                    // 有資料，載入所有欄位
                    LoadAllData(() =>
                    {
                        _isInitialized = true;
                        onComplete?.Invoke(true);
                    });
                }
                else
                {
                    // 沒有資料，首次使用
                    _isInitialized = true;
                    onComplete?.Invoke(false);
                }
            });
        }

        /// <summary>
        /// 從 DataStore 載入所有資料到本地快取
        /// </summary>
        private static void LoadAllData(Action onComplete)
        {
            int pendingRequests = 3;
            Action checkComplete = () =>
            {
                pendingRequests--;
                if (pendingRequests == 0) onComplete?.Invoke();
            };

            // Load StudentID
            var req1 = SpatialBridge.userWorldDataStoreService.GetVariable(KEY_STUDENT_ID, "");
            req1.SetCompletedEvent((r) =>
            {
                _cachedStudentID = r.stringValue ?? "";
                checkComplete();
            });

            // Load GroupNumber
            var req3 = SpatialBridge.userWorldDataStoreService.GetVariable(KEY_GROUP_NUMBER, "");
            req3.SetCompletedEvent((r) =>
            {
                _cachedGroupNumber = r.stringValue ?? "";
                checkComplete();
            });

            // Load Coins
            var req4 = SpatialBridge.userWorldDataStoreService.GetVariable(KEY_COINS, 0);
            req4.SetCompletedEvent((r) =>
            {
                _cachedCoins = r.intValue;
                checkComplete();
            });
        }

        #endregion

        #region Properties (讀取：使用快取)

        public static string StudentID => _cachedStudentID ?? "";
        public static string GroupNumber => _cachedGroupNumber ?? "";
        public static int Coins => _cachedCoins;
        public static bool HasRegisteredData => !string.IsNullOrEmpty(_cachedStudentID);

        #endregion

        #region Save Methods (寫入：同時更新快取和雲端)

        /// <summary>
        /// 儲存學生基本資訊
        /// </summary>
        public static void SaveStudentInfo(string studentID, string groupNumber, Action<bool> onComplete = null)
        {
            _cachedStudentID = studentID;
            _cachedGroupNumber = groupNumber;

            int pendingRequests = 2;
            bool allSucceeded = true;
            Action<bool> checkComplete = (success) =>
            {
                if (!success) allSucceeded = false;
                pendingRequests--;
                if (pendingRequests == 0) onComplete?.Invoke(allSucceeded);
            };

            var req1 = SpatialBridge.userWorldDataStoreService.SetVariable(KEY_STUDENT_ID, studentID);
            req1.SetCompletedEvent((r) => checkComplete(r.succeeded));

            var req2 = SpatialBridge.userWorldDataStoreService.SetVariable(KEY_GROUP_NUMBER, groupNumber);
            req2.SetCompletedEvent((r) => checkComplete(r.succeeded));
        }

        /// <summary>
        /// 增加金幣
        /// </summary>
        public static void AddCoins(int amount, Action<bool> onComplete = null)
        {
            _cachedCoins += amount;
            var request = SpatialBridge.userWorldDataStoreService.SetVariable(KEY_COINS, _cachedCoins);
            request.SetCompletedEvent((r) => onComplete?.Invoke(r.succeeded));
        }

        /// <summary>
        /// 扣除金幣 (如果餘額不足則返回 false)
        /// </summary>
        public static bool SpendCoins(int amount, Action<bool> onComplete = null)
        {
            if (_cachedCoins >= amount)
            {
                _cachedCoins -= amount;
                var request = SpatialBridge.userWorldDataStoreService.SetVariable(KEY_COINS, _cachedCoins);
                request.SetCompletedEvent((r) => onComplete?.Invoke(r.succeeded));
                return true;
            }
            onComplete?.Invoke(false);
            return false;
        }

        /// <summary>
        /// 清除所有資料 (用於測試或登出)
        /// </summary>
        public static void ClearAllData(Action<bool> onComplete = null)
        {
            _cachedStudentID = null;
            _cachedGroupNumber = null;
            _cachedCoins = 0;
            _isInitialized = false;

            var request = SpatialBridge.userWorldDataStoreService.ClearAllVariables();
            request.SetCompletedEvent((r) => onComplete?.Invoke(r.succeeded));
        }

        #endregion
    }
}
