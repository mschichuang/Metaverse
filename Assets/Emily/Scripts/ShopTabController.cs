using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace Emily.Scripts
{
    public class ShopTabController : MonoBehaviour
    {
        [System.Serializable]
        public class TabPagePair
        {
            public string tabName;
            public Button tabButton;
            public string categoryID; // The category string (e.g. "Case", "CPU")
        }

        public ShopContentGenerator contentGenerator; // Reference to the generator
        public List<TabPagePair> tabs;
        
        [Header("Settings")]
        public Color activeColor = Color.white;
        public Color inactiveColor = Color.gray;

        private void Start()
        {
            // Register button clicks
            foreach (var tab in tabs)
            {
                if (tab.tabButton != null)
                {
                    tab.tabButton.onClick.AddListener(() => SwitchToTab(tab));
                }
            }

            // Open first tab by default
            if (tabs.Count > 0)
            {
                SwitchToTab(tabs[0]);
            }
        }

        public void SwitchToTab(TabPagePair selectedTab)
        {
            // 1. Tell Generator to show this category
            if (contentGenerator != null)
            {
                contentGenerator.ShowCategory(selectedTab.categoryID);
            }

            // 2. Update Tab Visuals
            foreach (var tab in tabs)
            {
                bool isActive = (tab == selectedTab);
                
                // Update Text Color
                if (tab.tabButton != null)
                {
                    var tmpText = tab.tabButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (tmpText != null)
                    {
                        tmpText.color = isActive ? activeColor : inactiveColor;
                    }
                    else
                    {
                        var legacyText = tab.tabButton.GetComponentInChildren<Text>();
                        if (legacyText != null)
                        {
                            legacyText.color = isActive ? activeColor : inactiveColor;
                        }
                    }
                }
            }
        }
    }
}
