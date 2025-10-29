using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public int price = 50; // 解鎖需要的金幣
    private bool isUnlocked = false;

    private void Start()
    {
        LockItem(); // 開始時鎖住物品
    }

    // 嘗試付費解鎖
    public bool TryUnlock()
    {
        if (isUnlocked)
        {
            Debug.Log($"{gameObject.name} 已經解鎖過了！");
            return false;
        }

        if (PlayerWallet.Instance != null && PlayerWallet.Instance.SpendCoins(price))
        {
            UnlockItem();
            Debug.Log($"{gameObject.name} 已成功解鎖！");
            return true;
        }
        else
        {
            Debug.Log($"金幣不足，無法解鎖 {gameObject.name}！");
            return false;
        }
    }

    // 是否已解鎖，給外部判斷
    public bool IsUnlocked() => isUnlocked;

    // 私有：解鎖
    private void UnlockItem()
    {
        isUnlocked = true;
        Debug.Log($"{gameObject.name} 已解鎖，可以使用！");
    }

    // 私有：鎖住
    private void LockItem()
    {
        isUnlocked = false;
        Debug.Log($"{gameObject.name} 被鎖住了！");
    }
}
