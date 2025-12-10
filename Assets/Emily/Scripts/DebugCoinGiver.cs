using UnityEngine;

namespace Emily.Scripts
{
    /// <summary>
    /// 測試用腳本：增加組別金幣
    /// 請掛在場景物件上，並使用 Spatial Interactable 的 On Interact 事件呼叫 AddTestCoins()
    /// </summary>
    public class DebugCoinGiver : MonoBehaviour
    {
        public void AddTestCoins()
        {
            if (GroupCoinManager.Instance != null)
            {
                GroupCoinManager.Instance.AddGroupCoins(100000);
            }
        }
    }
}
