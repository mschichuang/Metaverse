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
    public void PayToUnlock()
    {
        if (isUnlocked)
        {
            Debug.Log($"{gameObject.name} 已經解鎖過了！");
            return;
        }

        if (PlayerWallet.Instance != null && PlayerWallet.Instance.SpendCoins(price))
        {
            UnlockItem();
            Debug.Log($"{gameObject.name} 已成功解鎖！");
        }
        else
        {
            Debug.Log($"金幣不足，無法解鎖 {gameObject.name}！");
        }
    }

    // 使用物品
    public void UseItem()
    {
        if (isUnlocked)
        {
            Debug.Log($"{gameObject.name} 被使用！");
            // ⚡ 在這裡放你的物品效果（攻擊、回血、開門…）
        }
        else
        {
            Debug.Log($"{gameObject.name} 還沒解鎖！");
        }
    }

    // 移動物品
    public void MoveItem(Vector3 newPosition)
    {
        if (isUnlocked)
        {
            transform.position = newPosition;
            Debug.Log($"{gameObject.name} 移動到新位置！");
        }
        else
        {
            Debug.Log($"{gameObject.name} 還沒解鎖，不能移動！");
        }
    }

    // 是否解鎖，給 DragAndDrop 用
    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    // 私有方法，解鎖
    private void UnlockItem()
    {
        isUnlocked = true;
        Debug.Log($"{gameObject.name} 已解鎖，可以使用！");
    }

    // 私有方法，鎖住
    private void LockItem()
    {
        isUnlocked = false;
        Debug.Log($"{gameObject.name} 被鎖住了！");
    }
}
