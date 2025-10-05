using UnityEngine;
using SpatialSys.UnitySDK;
using UnityEngine.UI;
using TMPro;

public class BuyManager : MonoBehaviour
{
    public ProductCard productCard;
    public CoinUIManager coinUIManager;
    public PopupManager popupManager;
    public SpecManager specManager;
    public PurchaseHistoryManager purchaseHistoryManager;
    public Button actionButton;
    public Button infoButton;
    private bool isPurchased = false;

    void Awake()
    {
        productCard = GetComponent<ProductCard>();
    }

    void Start()
    {
        actionButton.onClick.AddListener(HandleAction);
        infoButton.onClick.AddListener(ShowSpec);
        UpdateButton();
    }

    private void HandleAction()
    {
        if (isPurchased)
        {
            ReturnItem();
        }
        else
        {
            BuyItem();
        }
    }

    private void BuyItem()
    {
        int currentCoins = coinUIManager.CurrentCoins;
        int price = productCard.price;
        string category = productCard.category;

        if (purchaseHistoryManager.HasPurchasedCategory(category))
        {
            popupManager.ShowMessage($"已擁有{category}，不能重複購買！");
            return;
        }

        if (currentCoins < price)
        {
            popupManager.ShowMessage("金幣不足！");
            return;
        }

        coinUIManager.SetCoins(currentCoins - price);

        string itemID = productCard.itemID;
        SpatialBridge.inventoryService.AddItem(itemID, 1);

        purchaseHistoryManager.AddPurchasedCategory(category, productCard.productName);
        popupManager.ShowMessage("購買成功！");

        isPurchased = true;
        UpdateButton();
    }

    private void ReturnItem()
    {
        int price = productCard.price;
        string category = productCard.category;

        SpatialBridge.inventoryService.DeleteItem(productCard.itemID);

        coinUIManager.SetCoins(coinUIManager.CurrentCoins + price);
        purchaseHistoryManager.RemovePurchasedCategory(category);

        popupManager.ShowMessage("退款成功！");

        isPurchased = false;
        UpdateButton();
    }

    private void UpdateButton()
    {
        actionButton.GetComponentInChildren<TMP_Text>().text = isPurchased ? "取消" : "購買";
        actionButton.GetComponent<Image>().color = isPurchased ? new Color32(220, 50, 70, 255) : new Color32(255, 255, 50, 255);
    }

    private void ShowSpec()
    {
        Texture specTexture = productCard.specTexture;
        specManager.ShowSpec(specTexture);
    }
}