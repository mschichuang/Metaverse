using UnityEngine;
using UnityEngine.UI;
using TMPro;   // 如果用 TextMeshPro 要加這個

public class PurchaseUIManager : MonoBehaviour
{
    public GameObject purchasePanel;     // 購買提示的 Panel
    public GameObject messageTextObject; // 🔹 改成 GameObject
    private TextMeshProUGUI messageTMP;  // TMP 文字元件 (比較新)
    private Text messageUI;              // UI 文字元件 (舊)

    private UnlockManager currentItem;   // 當前要購買的物品

    private void Awake()
    {
        // 嘗試自動抓取 TMP 或 Text
        if (messageTextObject != null)
        {
            messageTMP = messageTextObject.GetComponent<TextMeshProUGUI>();
            messageUI = messageTextObject.GetComponent<Text>();
        }
    }

    // 顯示購買提示
    public void ShowPurchaseUI(UnlockManager item, int price)
    {
        currentItem = item;
        purchasePanel.SetActive(true);

        string textMsg = $"You haven't purchased this item yet. Do you want to buy it for {price} coins?";

        if (messageTMP != null)
            messageTMP.text = textMsg;
        else if (messageUI != null)
            messageUI.text = textMsg;
    }

    // 確認購買
    public void OnConfirmPurchase()
    {
        if (currentItem != null)
        {
            currentItem.PayToUnlock();
        }
        purchasePanel.SetActive(false);
    }

    // 取消購買
    public void OnCancelPurchase()
    {
        purchasePanel.SetActive(false);
    }
}


