using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeachingPanelController : MonoBehaviour
{
    [Header("UI 元件")]
    public TMP_Text titleText;            // 教學標題文字
    public Image contentImage;            // 教學圖片
    public TMP_Text descriptionText;      // 教學說明文字
    public Button confirmButton;          // 「我了解了」按鈕

    private void Start()
    {
        // 啟動時關閉面板
        gameObject.SetActive(false);

        // 綁定按鈕事件
        if (confirmButton != null)
            confirmButton.onClick.AddListener(HidePanel);
    }

    /// <summary>
    /// 顯示教學面板
    /// </summary>
    public void ShowPanel(string title, Sprite image, string description)
    {
        Debug.Log("📖 顯示教學面板：" + title);

        if (titleText != null) titleText.text = title;
        if (contentImage != null) contentImage.sprite = image;
        if (descriptionText != null) descriptionText.text = description;

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隱藏教學面板
    /// </summary>
    public void HidePanel()
    {
        Debug.Log("🧠 玩家確認理解：" + titleText.text);
        gameObject.SetActive(false);

        // ✅ 這裡可以寫給金幣或任務推進的程式碼
    }
}
