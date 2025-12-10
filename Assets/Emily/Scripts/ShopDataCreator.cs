#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Emily.Scripts.Editor
{
    public class ShopDataCreator : EditorWindow
    {
        [MenuItem("Shop/Quick Create All Assets")]
        public static void CreateAssets()
        {
            string folderPath = "Assets/Emily/Products";
            
            // Ensure folder exists properly
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.Refresh(); // Force Unity to acknowledge the folder
            }

            // Define real products from user request
            var products = new (string name, string category, int price, string id)[]
            {
                // 機殼
                ("Phanteks NV7銀", "機殼", 6790, "case_nv7_silver"),
                ("Phanteks NV7金", "機殼", 6790, "case_nv7_gold"),

                // 主機板
                ("華碩 ROG MAXIMUS Z790 HERO", "主機板", 19290, "mb_rog_z790_hero"),
                ("華碩 PRO WS W680M-ACE SE", "主機板", 12990, "mb_pro_ws_w680m"),
                ("華碩 ROG STRIX B760-G GAMING WIFI", "主機板", 6790, "mb_rog_b760g"),

                // 中央處理器
                ("Intel i9-14900K", "中央處理器", 19999, "cpu_i9_14900k"),
                ("Intel i7-14700K", "中央處理器", 13800, "cpu_i7_14700k"),
                ("Intel i5-14500", "中央處理器", 7800, "cpu_i5_14500"),

                // 散熱器
                ("貓頭鷹 NH-D15", "散熱器", 3615, "cooler_nh_d15"),

                // 記憶體
                ("金士頓 64GB DDR5-6400/CL32 FURY Beast", "記憶體", 7500, "ram_kingston_64g"),
                ("金士頓 32GB DDR5-6800/CL34 FURY Beast", "記憶體", 5250, "ram_kingston_32g_6800"),
                ("金士頓 32GB DDR5-5600/CL36 FURY Beast", "記憶體", 3200, "ram_kingston_32g_5600"),

                // 固態硬碟
                ("三星 990 PRO 4TB含散熱片", "固態硬碟", 13499, "ssd_990pro_4tb"),
                ("三星 990 PRO 2TB含散熱片", "固態硬碟", 6599, "ssd_990pro_2tb"),
                ("三星 980 PRO 1TB", "固態硬碟", 2999, "ssd_980pro_1tb"),

                // 顯示卡
                ("技嘉 AORUS RTX4090 MASTER 24G", "顯示卡", 65990, "gpu_4090_master"),
                ("技嘉 AORUS RTX4080 SUPER MASTER 16G", "顯示卡", 40590, "gpu_4080_super"),
                ("技嘉 AORUS RTX4060 ELITE 8G", "顯示卡", 12890, "gpu_4060_elite"),

                // 電源供應器
                ("海韻 PRIME TX-1300 ATX3.0", "電源供應器", 16490, "psu_prime_1300w"),
                ("海韻 VERTEX PX-1200", "電源供應器", 8390, "psu_vertex_1200w"),
                ("海韻 FOCUS GX-850", "電源供應器", 3890, "psu_focus_850w")
            };

            foreach (var p in products)
            {
                CreateProductAsset(folderPath, p.name, p.category, p.price, p.id);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"🎉 All ProductAssets created in {folderPath}!");
        }

        private static void CreateProductAsset(string path, string name, string category, int price, string id)
        {
            // Sanitize filename: Replace slash with score, space with score
            string safeName = name.Replace("/", "-").Replace(" ", "_");
            foreach(char c in Path.GetInvalidFileNameChars())
            {
                safeName = safeName.Replace(c, '-');
            }

            string fullPath = $"{path}/{safeName}.asset";
            
            // Try to load existing asset
            ProductData asset = AssetDatabase.LoadAssetAtPath<ProductData>(fullPath);
            
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<ProductData>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }

            // Update data but preserve other fields (like Image, Prefab)
            asset.productName = name;
            asset.category = category;
            asset.price = price;
            asset.itemID = id;

            EditorUtility.SetDirty(asset);
        }
    }
}
#endif
