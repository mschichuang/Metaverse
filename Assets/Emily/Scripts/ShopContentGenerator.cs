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
        public Transform listContainer; // The single container (Grid) where items will spawn

        [Header("Manager References (For Injection)")]
        [Header("Manager References (For Injection)")]
        public AssemblyCoinUIManager coinUIManager;
        public PopupManager popupManager;
        public SpecManager specManager;
        public PurchaseHistoryManager purchaseHistoryManager;

        // Note: No Start() generation anymore. We wait for TabController to call ShowCategory.

        public void ShowCategory(string category)
        {
            if (productCardPrefab == null || listContainer == null)
            {
                Debug.LogError("ShopContentGenerator: Missing Prefab or Container!");
                return;
            }

            // [Auto-Load] If list is empty, try loading from Resources
            if (allProducts == null || allProducts.Count == 0)
            {
                // Note: User manually assigned items, but just in case
                // var loaded = Resources.LoadAll<ProductData>("Products"); 
                // allProducts = new List<ProductData>(loaded);
            }

            // 1. Clear existing items
            foreach (Transform child in listContainer)
            {
                Destroy(child.gameObject);
            }

            // 2. Filter and Instantiate
            foreach (var product in allProducts)
            {
                if (product == null) continue;

                // Case-insensitive comparison
                if (!string.Equals(product.category, category, StringComparison.OrdinalIgnoreCase))
                    continue;

                // Instantiate Card
                GameObject cardObj = Instantiate(productCardPrefab, listContainer);
                cardObj.name = $"Card_{product.productName}";

                // Setup ProductCard
                ProductCard card = cardObj.GetComponent<ProductCard>();
                if (card != null)
                {
                    card.Setup(product);
                }

                // Setup BuyManager (Inject Dependencies)
                // Note: We are now passing AssemblyCoinUIManager
                BuyManager buyManager = cardObj.GetComponent<BuyManager>();
                if (buyManager != null)
                {
                    buyManager.Setup(coinUIManager, popupManager, specManager, purchaseHistoryManager);
                }
            }
        }
    }
}
