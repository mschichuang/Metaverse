using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Emily.Scripts;

public class CoinUIManager : MonoBehaviour
{
    public TMP_Text coinText;
    public int CurrentCoins { get; private set; }

    void Start()
    {
        // 訂閱金幣變更事件
        StudentData.OnCoinsChanged += OnCoinsChanged;
        
        _ = UpdateCoinUI();
    }
    
    void OnDestroy()
    {
        // 取消訂閱
        StudentData.OnCoinsChanged -= OnCoinsChanged;
    }
    
    /// <summary>
    /// 金幣變更時自動更新 UI
    /// </summary>
    private void OnCoinsChanged(int newCoins)
    {
        SetCoins(newCoins);
    }

    public async Task UpdateCoinUI()
    {
        // 從 StudentData DataStore 讀取金幣
        SetCoins(StudentData.Coins);
    }

    public void SetCoins(int amount)
    {
        CurrentCoins = amount;
        coinText.text = amount.ToString();
    }

    [System.Serializable]
    private class CoinResponse
    {
        public int coins;
    }
}