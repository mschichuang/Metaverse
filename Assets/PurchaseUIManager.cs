using UnityEngine;
using UnityEngine.UI;

public class MyPurchaseUIManager : MonoBehaviour
{
    public GameObject panel;        // 購買視窗 (Panel)
    public Text itemNameText;       // 顯示物品名稱
    public Text priceText;          // 顯示價格
    public Button buyButton;        // 購買按鈕
    public Button cancelButton;     // 取消按鈕

    private UnlockManager currentUnlockItem; // 當前要購買的物件

    void Start()
    {
        panel.SetActive(false); // 一開始隱藏購買視窗

        buyButton.onClick.AddListener(OnBuyClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);
    }

    // 顯示購買介面
    public void ShowPurchaseUI(UnlockManager unlockItem, int price)
    {
        currentUnlockItem = unlockItem;
        itemNameText.text = unlockItem.gameObject.name;
        priceText.text = $"價格：{price} 鑽石";
        panel.SetActive(true);
    }

    private void OnBuyClicked()
    {
        if (currentUnlockItem != null)
        {
            bool success = currentUnlockItem.TryUnlock();

            if (success)
                Debug.Log($"{currentUnlockItem.gameObject.name} 已成功解鎖！");
            else
                Debug.Log("鑽石不足，購買失敗。");

            panel.SetActive(false);
        }
    }

    private void OnCancelClicked()
    {
        panel.SetActive(false);
    }
}
