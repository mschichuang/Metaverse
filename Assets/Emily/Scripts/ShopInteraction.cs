using UnityEngine;
using SpatialSys.UnitySDK;

namespace Emily.Scripts
{
    public class ShopInteraction : MonoBehaviour
    {
        [Header("UI Reference")]
        public GameObject shopPanel;

        private void Start()
        {
            // Ensure shop is closed at start
            if (shopPanel != null)
            {
                shopPanel.SetActive(false);
            }
        }

        // Call this from Spatial Interactable's OnInteract event
        public void OpenShop()
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(true);
                
                // Unlock cursor for UI interaction
                // SpatialBridge calls removed due to API mismatch
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public void CloseShop()
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(false);
                
                // Lock cursor back to game
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
