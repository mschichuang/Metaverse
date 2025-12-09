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
                // Case
                ("Phanteks NV7銀", "Case", 6790, "case_nv7_silver"),
                ("Phanteks NV7金", "Case", 6790, "case_nv7_gold"),

                // MB
                ("華碩 ROG MAXIMUS Z790 HERO", "MB", 19290, "mb_rog_z790_hero"),
                ("華碩 PRO WS W680M-ACE SE", "MB", 12990, "mb_pro_ws_w680m"),
                ("華碩 ROG STRIX B760-G GAMING WIFI", "MB", 6790, "mb_rog_b760g"),

                // CPU
                ("Intel i9-14900K", "CPU", 19999, "cpu_i9_14900k"),
                ("Intel i7-14700K", "CPU", 13800, "cpu_i7_14700k"),
                ("Intel i5-14500", "CPU", 7800, "cpu_i5_14500"),

                // Cooler
                ("貓頭鷹 NH-D15", "Cooler", 3615, "cooler_nh_d15"),

                // RAM
                ("金士頓 64GB DDR5-6400/CL32 FURY Beast", "RAM", 7500, "ram_kingston_64g"),
                ("金士頓 32GB DDR5-6800/CL34 FURY Beast", "RAM", 5250, "ram_kingston_32g_6800"),
                ("金士頓 32GB DDR5-5600/CL36 FURY Beast", "RAM", 3200, "ram_kingston_32g_5600"),

                // SSD
                ("三星 990 PRO 4TB含散熱片", "SSD", 13499, "ssd_990pro_4tb"),
                ("三星 990 PRO 2TB含散熱片", "SSD", 6599, "ssd_990pro_2tb"),
                ("三星 980 PRO 1TB", "SSD", 2999, "ssd_980pro_1tb"),

                // GPU
                ("技嘉 AORUS RTX4090 MASTER 24G", "GPU", 65990, "gpu_4090_master"),
                ("技嘉 AORUS RTX4080 SUPER MASTER 16G", "GPU", 40590, "gpu_4080_super"),
                ("技嘉 AORUS RTX4060 ELITE 8G", "GPU", 12890, "gpu_4060_elite"),

                // PSU
                ("海韻 PRIME TX-1300 ATX3.0", "PSU", 16490, "psu_prime_1300w"),
                ("海韻 VERTEX PX-1200", "PSU", 8390, "psu_vertex_1200w"),
                ("海韻 FOCUS GX-850", "PSU", 3890, "psu_focus_850w")
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
            
            // Don't overwrite existing
            if (File.Exists(fullPath)) return;

            ProductData asset = ScriptableObject.CreateInstance<ProductData>();
            asset.productName = name; // Keep original name for display
            asset.category = category;
            asset.price = price;
            asset.itemID = id;

            AssetDatabase.CreateAsset(asset, fullPath);
        }
    }
}
#endif
