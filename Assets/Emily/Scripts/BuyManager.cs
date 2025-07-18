using UnityEngine;

public class BuyManager : MonoBehaviour
{
    public ProductCard productCard;
    public CoinUIManager coinUIManager;
    public PopupManager popupManager;

    void Awake()
    {
        productCard = GetComponent<ProductCard>();
    }

    public void OnBuyButtonClick()
    {
        int currentCoins = coinUIManager.CurrentCoins;
        int price = productCard.price;

        if (currentCoins < price)
        {
            popupManager.ShowMessage("金幣不足！");
        }
        else
        {
            coinUIManager.SetCoins(currentCoins - price);
            popupManager.ShowMessage("購買成功！");
        }
    }
}