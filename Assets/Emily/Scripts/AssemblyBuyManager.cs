using UnityEngine;
using SpatialSys.UnitySDK;
using UnityEngine.UI;
using TMPro;
using Emily.Scripts;

/// <summary>
/// 組裝區專用的購買管理器
/// 使用組別共用金幣池（GroupCoinManager）
/// </summary>
public class AssemblyBuyManager : MonoBehaviour
{
    public ProductCard productCard;
    public AssemblyCoinUIManager coinUIManager;
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
        int price = productCard.price;
        string category = productCard.category;

        if (purchaseHistoryManager.HasPurchasedCategory(category))
        {
            popupManager.ShowMessage($"已擁有{category},不能重複購買!");
            return;
        }

        // 檢查組別金幣
        if (GroupCoinManager.Instance == null)
        {
            popupManager.ShowMessage("金幣系統尚未初始化!");
            return;
        }
        
        int currentCoins = GroupCoinManager.Instance.GetGroupCoins();
        if (currentCoins < price)
        {
            popupManager.ShowMessage("組別金幣不足!");
            return;
        }

        // 扣除組別金幣
        bool success = GroupCoinManager.Instance.SpendGroupCoins(price);
        
        if (!success)
        {
            popupManager.ShowMessage("購買失敗，請稍後再試!");
            return;
        }

        // 購買成功
        SpatialBridge.inventoryService.AddItem(productCard.itemID, 1);
        purchaseHistoryManager.AddPurchasedCategory(category, productCard.productName);

        popupManager.ShowMessage("購買成功!");
        isPurchased = true;
        UpdateButton();
        SpawnComponent(componentPrefab);

        // 更新 UI
        if (coinUIManager != null)
        {
            coinUIManager.RefreshCoins();
        }
    }

    private void ReturnItem()
    {
        SpatialBridge.inventoryService.DeleteItem(productCard.itemID);

        int price = productCard.price;
        string category = productCard.category;

        // 退款到組別金幣池
        if (GroupCoinManager.Instance != null)
        {
            GroupCoinManager.Instance.AddGroupCoins(price);
        }
        
        purchaseHistoryManager.RemovePurchasedCategory(category);

        popupManager.ShowMessage("退款成功!");
        isPurchased = false;
        UpdateButton();
        Destroy(spawnedComponent);

        // 更新 UI
        if (coinUIManager != null)
        {
            coinUIManager.RefreshCoins();
        }
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

        spawnPos.y += 0.5f;

        Quaternion spawnRot = Quaternion.LookRotation(forward, Vector3.up);
        spawnRot *= Quaternion.Euler(90f, 0f, 0f);

        spawnedComponent = Instantiate(prefab, spawnPos, spawnRot);
    }
}
