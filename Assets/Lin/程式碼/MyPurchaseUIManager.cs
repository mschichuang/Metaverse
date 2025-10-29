using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyPurchaseUIManager : MonoBehaviour
{
    public UnlockManager unlockManager;
    public Button purchaseButton;
    public TMP_Text coinText;

    private void Start()
    {
        // 初始化玩家金幣，直接用 PlayerWallet 預設值
        if (PlayerWallet.Instance == null)
        {
            Debug.LogError("PlayerWallet 未初始化");
            return;
        }

        purchaseButton.onClick.AddListener(OnPurchaseClick);
        HidePurchaseUI(); // 初始隱藏購買 UI
        UpdateUI();
    }

    private void OnPurchaseClick()
    {
        if (unlockManager != null && unlockManager.TryUnlock())
        {
            // 不存檔，直接更新金幣
            UpdateUI();
            HidePurchaseUI();
        }
    }

    public void ShowPurchaseUI(UnlockManager manager, int price)
    {
        unlockManager = manager;
        coinText.text = $"💰 {PlayerWallet.Instance.GetCoins()} 金幣\n解鎖價格：{price}";
        purchaseButton.gameObject.SetActive(true);
    }

    public void HidePurchaseUI()
    {
        purchaseButton.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        coinText.text = $"💰 {PlayerWallet.Instance.GetCoins()} 金幣";
    }
}
