#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;
using Emily.Scripts;

namespace Emily.Scripts.Editor
{
    public class FontWeightAdjuster : EditorWindow
    {
        [MenuItem("Tools/Font Weights/Make ProductCards Thicker (Add Bold)")]
        public static void MakeProductCardsBold()
        {
            UpdateFontWeight<ProductCard>(true, "ProductCard");
        }

        [MenuItem("Tools/Font Weights/Make Popups Thinner (Remove Bold)")]
        public static void MakePopupsThin()
        {
            // PopupManager 通常掛在畫布或管理器上，我們要找的是它控制的 Popup Panel
            // 這裡嘗試搜尋 Popup related scripts or objects
            UpdateFontWeight<PopupManager>(false, "PopupManager");
        }

        private static void UpdateFontWeight<T>(bool makeBold, string componentName) where T : Component
        {
            // 搜尋所有 Prefab 和場景物件
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Emily" });
            int updatedCount = 0;

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab != null)
                {
                    // 檢查這個 Prefab 是否有目標 Component (例如 ProductCard)
                    // 或者它是 Popup 介面的一部份
                    if (prefab.GetComponentInChildren<T>(true) != null)
                    {
                        TMP_Text[] tmpTexts = prefab.GetComponentsInChildren<TMP_Text>(true);
                        bool isDirty = false;

                        foreach (TMP_Text tmp in tmpTexts)
                        {
                            // 記錄原始狀態
                            Undo.RecordObject(tmp, $"Adjust Font Weight {componentName}");

                            if (makeBold)
                            {
                                // 增加粗體
                                if ((tmp.fontStyle & FontStyles.Bold) == 0)
                                {
                                    tmp.fontStyle |= FontStyles.Bold;
                                    isDirty = true;
                                }
                            }
                            else
                            {
                                // 移除粗體
                                if ((tmp.fontStyle & FontStyles.Bold) != 0)
                                {
                                    tmp.fontStyle &= ~FontStyles.Bold;
                                    isDirty = true;
                                }
                                
                                // 額外確保 Face Dilate 歸零 (避免材質造成的變粗)
                                if (tmp.fontSharedMaterial != null && tmp.fontSharedMaterial.HasProperty("_FaceDilate"))
                                {
                                    // 這裡無法直接修改 Material 資源，只能建議使用者檢查
                                    // 但如果是 Instance Material 可以改
                                }
                            }
                        }

                        if (isDirty)
                        {
                            EditorUtility.SetDirty(prefab);
                            updatedCount++;
                        }
                    }
                }
            }
            
            AssetDatabase.SaveAssets();
            Debug.Log($"✅ 完成！共調整了 {updatedCount} 個 {componentName} 相關的 Prefab。");
        }
    }
}
#endif
