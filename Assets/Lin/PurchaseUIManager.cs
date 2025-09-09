using UnityEngine;
using UnityEngine.UI;

public class PurchaseUIManager : MonoBehaviour
{
    public GameObject purchasePanel;   // 購買提示的 Panel
    public Text messageText;           // 顯示提示文字
    private UnlockManager currentItem; // 當前要購買的物品

    // 顯示購買提示
    public void ShowPurchaseUI(UnlockManager item, int price)
    {
        currentItem = item;
        purchasePanel.SetActive(true);
        messageText.text = $"您尚未購買此物品，是否花費 {price} 金幣購買？";
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

