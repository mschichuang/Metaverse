using UnityEngine;
using SpatialSys.UnitySDK;
using System;

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

        // 本地快取
        private static string _cachedGroupNumber = null;
        private static string _cachedStudentName = null;
        private static int _cachedCoins = 0;
        private static bool _isInitialized = false;

        #region Initialization

        /// <summary>
        /// 初始化：從 Spatial Display Name 解析學生資訊
        /// </summary>
        public static void Initialize(Action<bool> onComplete = null)
        {
            if (_isInitialized)
            {
                onComplete?.Invoke(true);
                return;
            }

            // 讀取 Spatial Display Name
            string displayName = SpatialBridge.actorService.localActor.displayName;
            ParseDisplayName(displayName);

            // 載入金幣資料
            var request = SpatialBridge.userWorldDataStoreService.GetVariable(KEY_COINS, 0);
            request.SetCompletedEvent((r) =>
            {
                _cachedCoins = r.intValue;
                _isInitialized = true;
                onComplete?.Invoke(IsValidFormat);
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
        
        /// <summary>
        /// 檢查 Display Name 格式是否正確
        /// </summary>
        public static bool IsValidFormat => !string.IsNullOrEmpty(_cachedGroupNumber) && !string.IsNullOrEmpty(_cachedStudentName);

        #endregion

        #region Coin Management

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
        /// 扣除金幣
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
        /// 清除金幣資料（測試用）
        /// </summary>
        public static void ClearCoins(Action<bool> onComplete = null)
        {
            _cachedCoins = 0;
            var request = SpatialBridge.userWorldDataStoreService.SetVariable(KEY_COINS, 0);
            request.SetCompletedEvent((r) => onComplete?.Invoke(r.succeeded));
        }

        #endregion
    }
}
