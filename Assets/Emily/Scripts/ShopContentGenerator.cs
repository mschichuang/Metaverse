using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
            // [Auto-Load] If list is empty, try loading from Resources
            if (allProducts == null || allProducts.Count == 0)
            {
                // Resources.LoadAll is banned in Spatial. 
                // Please ensure allProducts are assigned in Inspector or via Editor Script.
                Debug.LogWarning("ShopContentGenerator: allProducts list is empty! Items must be assigned in Inspector.");
            }

            // Find CoinUIManager automatically
            var coinUIManager = FindObjectOfType<AssemblyCoinUIManager>();

            // 1. Clear existing items
            foreach (Transform child in listContainer)
            {
                Destroy(child.gameObject);
            }

            // 2. Filter, Sort, and Instantiate
            // Sort by Price Descending (Expensive -> Cheap)
            var filteredProducts = allProducts
                .Where(p => p != null && string.Equals(p.category, category, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(p => p.price);

            foreach (var product in filteredProducts)
            {

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
