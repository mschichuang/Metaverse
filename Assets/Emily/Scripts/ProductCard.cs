using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Emily.Scripts
{
    public class ProductCard : MonoBehaviour
    {
        public TMP_Text nameText;
        public Image productImage;
        public TMP_Text priceText;
        public RawImage specImage;

        [Header("Debug Info")]
        public ProductData productData; // Reference to source data

        /// <summary>
        /// Initialize the card with data
        /// </summary>
        public void Setup(ProductData data)
        {
            productData = data;

            if (nameText != null) nameText.text = data.productName;
            if (productImage != null) productImage.sprite = data.productSprite;
            if (priceText != null) priceText.text = data.price.ToString();
            // specImage is usually for the popup, but if there's a mini spec view on card:
            if (specImage != null && data.specTexture != null) specImage.texture = data.specTexture;
        }
    }
}