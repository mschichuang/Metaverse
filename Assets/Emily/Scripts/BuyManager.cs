using UnityEngine;
using SpatialSys.UnitySDK;
using UnityEngine.UI;
using TMPro;
using Emily.Scripts;

public class BuyManager : MonoBehaviour
{
    public ProductCard productCard;
    public CoinUIManager coinUIManager;
    public PopupManager popupManager;
    public SpecManager specManager;
    public PurchaseHistoryManager purchaseHistoryManager;
    public Button actionButton;
    public Button infoButton;
    public GameObject componentPrefab;
    private GameObject spawnedComponent;
    private bool isPurchased = false;
    private bool hasViewedSpec = false;

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
        if (!hasViewedSpec)
        {
            popupManager.ShowMessage("請先查看此元件的規格後再進行購買。");
            return;
        }

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
        int currentCoins = StudentData.Coins;
        int price = productCard.price;
        string category = productCard.category;

        if (purchaseHistoryManager.HasPurchasedCategory(category))
        {
            popupManager.ShowMessage($"已擁有{category},不能重複購買!");
            return;
        }

        if (currentCoins < price)
        {
            popupManager.ShowMessage("金幣不足!");
            return;
        }

        // 使用 StudentData.SpendCoins 扣除金幣並存到 DataStore
        bool success = StudentData.SpendCoins(price, (result) => {
            if (result)
            {
                // 扣款成功,更新 UI
                coinUIManager.SetCoins(StudentData.Coins);
            }
        });
        
        if (!success)
        {
            popupManager.ShowMessage("金幣不足!");
            return;
        }

        SpatialBridge.inventoryService.AddItem(productCard.itemID, 1);
        purchaseHistoryManager.AddPurchasedCategory(category, productCard.productName);

        popupManager.ShowMessage("購買成功!");
        isPurchased = true;
        UpdateButton();
        SpawnComponent(componentPrefab);
    }

    private void ReturnItem()
    {
        SpatialBridge.inventoryService.DeleteItem(productCard.itemID);

        int price = productCard.price;
        string category = productCard.category;

        // 使用 StudentData.AddCoins 增加金幣並存到 DataStore
        StudentData.AddCoins(price, (result) => {
            if (result)
            {
                // 退款成功,更新 UI
                coinUIManager.SetCoins(StudentData.Coins);
            }
        });
        
        purchaseHistoryManager.RemovePurchasedCategory(category);

        popupManager.ShowMessage("退款成功!");
        isPurchased = false;
        UpdateButton();
        Destroy(spawnedComponent);
    }

    private void UpdateButton()
    {
        actionButton.GetComponentInChildren<TMP_Text>().text =
            isPurchased ? "取消" : "購買";

        actionButton.GetComponent<Image>().color =
            isPurchased ? new Color32(220, 50, 70, 255)
                        : new Color32(255, 255, 50, 255);
    }

    private void ShowSpec()
    {
        Texture specTexture = productCard.specTexture;
        specManager.ShowSpec(specTexture);

        hasViewedSpec = true;
    }

    private void SpawnComponent(GameObject prefab)
    {
        var avatar = SpatialBridge.actorService.localActor.avatar;

        Vector3 forward = avatar.rotation * Vector3.forward;
        Vector3 spawnPos = avatar.position + forward * 1.5f;

        // ⭐ 增加高度
        spawnPos.y += 0.5f;

        // ⭐ 先依玩家方向，再讓物體平躺
        Quaternion spawnRot = Quaternion.LookRotation(forward, Vector3.up);
        spawnRot *= Quaternion.Euler(90f, 0f, 0f);

        spawnedComponent = Instantiate(prefab, spawnPos, spawnRot);
    }
}