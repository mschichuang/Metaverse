using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    private bool isUnlocked = false;
    public int price = 100;
    public int playerCoins = 50;

    private void Start()
    {
        LockItem();
    }

    public void PayToUnlock()
    {
        if (isUnlocked)
        {
            Debug.Log($"{gameObject.name} 已經解鎖！");
            return;
        }

        if (playerCoins >= price)
        {
            playerCoins -= price;
            UnlockItem();
            Debug.Log($"{gameObject.name} 已付費解鎖，玩家剩餘金幣: {playerCoins}");
        }
        else
        {
            Debug.Log($"金幣不足，無法解鎖 {gameObject.name}！");
        }
    }

    private void UnlockItem()
    {
        isUnlocked = true;
        Debug.Log($"{gameObject.name} 已解鎖，可以使用！");
    }

    private void LockItem()
    {
        isUnlocked = false;
        Debug.Log($"{gameObject.name} 被鎖住了！");
    }

    public void UseItem()
    {
        if (isUnlocked)
        {
            Debug.Log($"{gameObject.name} 被使用！");
        }
        else
        {
            Debug.Log($"{gameObject.name} 還沒解鎖，不能使用！");
        }
    }

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

    // ✅ 新增這個方法讓 DragAndDrop 可以呼叫
    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}
