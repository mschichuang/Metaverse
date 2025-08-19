using UnityEngine;
using SpatialSys.UnitySDK;
using System.Collections.Generic;

public class BuyManager : MonoBehaviour
{
    public ProductCard productCard;
    public CoinUIManager coinUIManager;
    public PopupManager popupManager;
    public PurchaseHistoryManager purchaseHistoryManager;

    void Awake()
    {
        productCard = GetComponent<ProductCard>();
    }

    public void OnBuyButtonClick()
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
        }
        else
        {
            coinUIManager.SetCoins(currentCoins - price);

            string itemID = productCard.itemID;
            SpatialBridge.inventoryService.AddItem(itemID, 1);

            purchaseHistoryManager.AddPurchasedCategory(category);
            popupManager.ShowMessage("購買成功！");
        }
    }
}