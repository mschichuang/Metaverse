using UnityEngine;

namespace Emily.Scripts
{
    [CreateAssetMenu(fileName = "NewProduct", menuName = "Shop/Product Data")]
    public class ProductData : ScriptableObject
    {
        [Header("基本資料")]
        public string productName;
        public string itemID;
        public int price;
        
        [Header("分類 (Case, MB, CPU, Cooler, RAM, SSD, GPU, PSU)")]
        public string category;

        [Header("等級分數 (金=3, 銀=2, 銅=1, 同價/單一=2)")]
        [Tooltip("金=3, 銀=2, 銅=1, 機殼/散熱器等同價或單一選項=2")]
        [Range(1, 3)]
        public int tier = 2;

        [Header("顯示資源")]
        public Sprite productSprite;
        public Texture specTexture;

        [Header("3D 模組")]
        public GameObject componentPrefab;
    }
}
