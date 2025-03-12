using UnityEngine;
using UnityEngine.UI;

public class ComponentInfoDisplay : MonoBehaviour
{
    public Image infoImage; // 用於顯示元件圖片的 UI Image
    public Sprite defaultImage; // 預設圖片
    public Sprite componentImage; // 特定元件的圖片

    private void Start()
    {
        // 確保初始時圖片為預設狀態
        infoImage.sprite = defaultImage;
        infoImage.gameObject.SetActive(false); // 隱藏圖片
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 當玩家進入觸發區
        {
            infoImage.sprite = componentImage; // 切換為元件圖片
            infoImage.gameObject.SetActive(true); // 顯示圖片
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 當玩家離開觸發區
        {
            infoImage.gameObject.SetActive(false); // 隱藏圖片
        }
    }
}
