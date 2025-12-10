using UnityEngine;
using SpatialSys.UnitySDK;
using UnityEngine.UI;
using TMPro;
using Emily.Scripts;


namespace Emily.Scripts
{
    public class BuyManager : MonoBehaviour
    {
        [Header("Dependencies (Auto-Injected)")]
        public ProductCard productCard;
        public AssemblyCoinUIManager coinUIManager; // Changed to AssemblyCoinUIManager
        public PopupManager popupManager;
        public SpecManager specManager;
        public PurchaseHistoryManager purchaseHistoryManager;
        
        [Header("UI Components")]
        public Button actionButton;
        public Button infoButton;
        
        private GameObject spawnedComponent;
        private bool isPurchased = false;
        private bool hasViewedSpec = false;

        public void Setup(AssemblyCoinUIManager coinUI, PopupManager popup, SpecManager spec, PurchaseHistoryManager history)
        {
            coinUIManager = coinUI;
            popupManager = popup;
            specManager = spec;
            purchaseHistoryManager = history;
            
            productCard = GetComponent<ProductCard>();

            actionButton.onClick.RemoveAllListeners();
            infoButton.onClick.RemoveAllListeners();

            actionButton.onClick.AddListener(HandleAction);
            infoButton.onClick.AddListener(ShowSpec);

            // Check if already purchased
            if (productCard.productData != null)
            {
                isPurchased = purchaseHistoryManager.HasPurchasedCategory(productCard.productData.category);
            }
            
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
            if (productCard.productData == null) return;
            
            int price = productCard.productData.price;
            string category = productCard.productData.category;

            if (purchaseHistoryManager.HasPurchasedCategory(category))
            {
                popupManager.ShowMessage($"已擁有{category},不能重複購買！");
                return;
            }

            // 檢查組別金幣
            if (GroupCoinManager.Instance == null)
            {
                popupManager.ShowMessage("金幣系統尚未初始化！");
                return;
            }
            
            int currentCoins = GroupCoinManager.Instance.GetGroupCoins();
            if (currentCoins < price)
            {
                popupManager.ShowMessage("組別金幣不足！");
                return;
            }

            // 扣除組別金幣
            bool success = GroupCoinManager.Instance.SpendGroupCoins(price);
            
            if (!success)
            {
                popupManager.ShowMessage("購買失敗，請稍後再試！");
                return;
            }

            // 購買成功
            purchaseHistoryManager.AddPurchasedCategory(category, productCard.productData.productName);

            popupManager.ShowMessage("購買成功！");
            isPurchased = true;
            UpdateButton();
            
            if (productCard.productData.componentPrefab != null)
            {
                SpawnComponent(productCard.productData.componentPrefab);
            }

            // 更新 UI
            if (coinUIManager != null)
            {
                coinUIManager.RefreshCoins();
            }
        }

        private void ReturnItem()
        {
            if (productCard.productData == null) return;

            int price = productCard.productData.price;
            string category = productCard.productData.category;

            // 退款到組別金幣池
            if (GroupCoinManager.Instance != null)
            {
                GroupCoinManager.Instance.AddGroupCoins(price);
            }
            
            purchaseHistoryManager.RemovePurchasedCategory(category);

            popupManager.ShowMessage("退款成功！");
            isPurchased = false;
            UpdateButton();
            
            if (spawnedComponent != null)
            {
                Destroy(spawnedComponent);
            }
        }

        private void UpdateButton()
        {
            if (actionButton == null) return;
            
            var textComp = actionButton.GetComponentInChildren<TMP_Text>();
            if (textComp != null) textComp.text = isPurchased ? "取消" : "購買";

            var imgComp = actionButton.GetComponent<Image>();
            if (imgComp != null)
            {
                imgComp.color = isPurchased ? new Color32(220, 50, 70, 255) : new Color32(255, 255, 50, 255);
            }
        }

        private void ShowSpec()
        {
            if (productCard.productData != null)
            {
                specManager.ShowSpec(productCard.productData.specTexture);
                hasViewedSpec = true;
            }
        }

        private void SpawnComponent(GameObject prefab)
        {
            if (prefab == null) return;

            var avatar = SpatialBridge.actorService.localActor.avatar;

            Vector3 forward = avatar.rotation * Vector3.forward;
            Vector3 spawnPos = avatar.position + forward * 1.5f;
            spawnPos.y += 0.5f;

            Quaternion spawnRot = Quaternion.LookRotation(forward, Vector3.up);
            spawnRot *= Quaternion.Euler(90f, 0f, 0f);

            spawnedComponent = Instantiate(prefab, spawnPos, spawnRot);
        }
    }
}