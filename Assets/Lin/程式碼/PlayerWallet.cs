using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public static PlayerWallet Instance { get; private set; }

    private int coins = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ✅ 新增：設定金幣數量（載入時使用）
    public void SetCoins(int value)
    {
        coins = Mathf.Max(0, value);
    }

    // ✅ 新增：取得目前金幣數量
    public int GetCoins()
    {
        return coins;
    }

    // ✅ 扣錢（成功回傳 true）
    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            return true;
        }
        return false;
    }

    // ✅ 增加金幣
    public void AddCoins(int amount)
    {
        coins += Mathf.Max(0, amount);
    }
}
