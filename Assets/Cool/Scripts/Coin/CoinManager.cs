using UnityEngine;
using TMPro; // 一定要加這行,才可以控制 TextMeshPro
using Emily.Scripts;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance; // 方便別的腳本叫它
    public TextMeshProUGUI coinText;    // 左上角文字 (舊的 UI)

    private void Awake()
    {
        // 如果場景裡沒有 CoinManager,就設定自己
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // 訂閱 StudentData 金幣變更事件
        StudentData.OnCoinsChanged += OnCoinsChanged;
        
        UpdateCoinText(); // 開始時更新 UI
    }
    
    private void OnDestroy()
    {
        // 取消訂閱
        if (Instance == this)
        {
            StudentData.OnCoinsChanged -= OnCoinsChanged;
        }
    }
    
    /// <summary>
    /// 金幣變更時自動更新舊的 UI
    /// </summary>
    private void OnCoinsChanged(int newCoins)
    {
        UpdateCoinText();
    }

    public void AddCoin(int amount)
    {
        // 使用 StudentData.AddCoins 存到 DataStore
        // 會自動觸發事件,更新所有 UI (包括新舊 UI)
        StudentData.AddCoins(amount);
    }

    private void UpdateCoinText()
    {
        // 從 StudentData 讀取金幣
        if (coinText != null)
        {
            coinText.text = "" + StudentData.Coins;
        }
    }
}
