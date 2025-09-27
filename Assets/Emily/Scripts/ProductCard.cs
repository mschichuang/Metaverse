using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductCard : MonoBehaviour
{
    public TMP_Text nameText;
    public Image productImage;
    public TMP_Text priceText;

    [Header("商品資料")]
    public string productName;
    public Sprite productSprite;
    public int price;
    public string itemID;
    public string category;

    void Start()
    {
        InitializeCard();
    }

    public void InitializeCard()
    {
        nameText.text = productName;
        productImage.sprite = productSprite;
        priceText.text = price.ToString();
    }
}