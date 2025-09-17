using UnityEngine;
using UnityEngine.UI;

public class PlayerWallet : MonoBehaviour
{
    public static PlayerWallet Instance;   // 單例
    public int coins = 0;                  // 當前金幣
    public GameObject coinTextObject;      // 🔹 直接接 GameObject
    private Text coinText;                 // 🔹 內部抓 Text 元件

    private void Awake()
    {
        // 確保全場景只有一個 Wallet
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 從 GameObject 取出 Text 元件
        if (coinTextObject != null)
        {
            coinText = coinTextObject.GetComponent<Text>();
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    // 🔹 增加金幣
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    // 🔹 扣除金幣（回傳是否成功）
    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateUI();
            return true;
        }
        else
        {
            Debug.Log("💰 金幣不足！");
            return false;
        }
    }

    // 🔹 更新 UI
    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = $"💰 {coins}";
        }
    }
}


