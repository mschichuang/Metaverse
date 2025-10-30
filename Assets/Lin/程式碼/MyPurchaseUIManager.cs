using UnityEngine;
using UnityEngine.UI;

public class MyPurchaseUIManager : MonoBehaviour
{
    public GameObject panel;
    public Text itemNameText;
    public Text priceText;
    public Button buyButton;
    public Button cancelButton;

    private UnlockManager currentUnlockItem;

    void Start()
    {
        panel.SetActive(false);
        buyButton.onClick.AddListener(OnBuyClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);
    }

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
