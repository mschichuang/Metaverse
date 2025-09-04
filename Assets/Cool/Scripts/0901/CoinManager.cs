using UnityEngine;
using TMPro; // 一定要加這行，才可以控制 TextMeshPro

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance; // 方便別的腳本叫它
    public TextMeshProUGUI coinText;    // 左上角文字
    private int coinCount = 0;          // 當前金幣數量

    private void Awake()
    {
        // 如果場景裡沒有 CoinManager，就設定自己
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateCoinText(); // 開始時把 UI 更新成 0
    }

    // 這個方法就是「撿到金幣就加數字」
    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = "Coins: " + coinCount;
    }
}
