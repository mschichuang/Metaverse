using System.Collections.Generic;
using UnityEngine;
using System;

namespace Emily.Scripts
{
    [System.Serializable]
    public class CategoryMapping
    {
        public string categoryName; // e.g. "Case", "GPU"
        public Transform container; // The content object under Scroll View
    }

    public class ShopContentGenerator : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject productCardPrefab;
        public List<ProductData> allProducts;
        
        [Header("UI Containers")]
        public List<CategoryMapping> categoryMappings;

        [Header("Manager References (For Injection)")]
        public CoinUIManager coinUIManager;
        public PopupManager popupManager;
        public SpecManager specManager;
        public PurchaseHistoryManager purchaseHistoryManager;

        void Start()
        {
            GenerateShopContent();
        }

        [ContextMenu("Refresh Shop UI")]
        public void GenerateShopContent()
        {
            if (productCardPrefab == null)
            {
                Debug.LogError("ProductCardPrefab is missing!");
                return;
            }

            // 1. Clear existing generated items (if any logic for that exists, optional)
            // For now, we assume we append or the containers are empty at start.

            // 2. Loop through all products
            foreach (var product in allProducts)
            {
                if (product == null) continue;

                // Find container for this category
                Transform targetContainer = GetContainerForCategory(product.category);
                if (targetContainer == null)
                {
                    Debug.LogWarning($"No container found for category: {product.category}");
                    continue;
                }

                // Instantiate Card
                GameObject cardObj = Instantiate(productCardPrefab, targetContainer);
                cardObj.name = $"Card_{product.productName}";

                // Setup ProductCard
                ProductCard card = cardObj.GetComponent<ProductCard>();
                if (card != null)
                {
                    card.Setup(product);
                }

                // Setup BuyManager (Inject Dependencies)
                BuyManager buyManager = cardObj.GetComponent<BuyManager>();
                if (buyManager != null)
                {
                    buyManager.Setup(coinUIManager, popupManager, specManager, purchaseHistoryManager);
                }
            }
        }

        private Transform GetContainerForCategory(string category)
        {
            foreach (var mapping in categoryMappings)
            {
                if (mapping.categoryName.Equals(category, StringComparison.OrdinalIgnoreCase))
                {
                    return mapping.container;
                }
            }
            return null;
        }
    }
}
