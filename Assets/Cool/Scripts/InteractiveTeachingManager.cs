using UnityEngine;
using UnityEngine.UI; // 引用 UnityEngine.UI 命名空間

public class InteractiveTeachingManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject buttonPanel; // 包含按鈕的 UI Panel
    public RawImage displayImage;  // 顯示圖片的 UI
    public Material ramMaterial;   // RAM 的材質
    public Material romMaterial;   // ROM 的材質

    [Header("Menu Reference")]
    public GameObject menuCanvas; // TriggerMenuUI 的主介面 Canvas

    private void Start()
    {
        // 隱藏按鈕面板
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(false);
        }
    }

    // 顯示按鈕和圖片的面板
    public void ShowButtons()
    {
        if (buttonPanel != null) buttonPanel.SetActive(true); // 顯示按鈕面板
    }

    // 隱藏按鈕和圖片的面板
    public void HideButtons()
    {
        if (buttonPanel != null) buttonPanel.SetActive(false); // 隱藏按鈕面板
        if (displayImage != null) displayImage.texture = null; // 清空圖片
    }

    // 顯示 RAM 的圖片
    public void ShowRamImage()
    {
        if (displayImage != null && ramMaterial != null)
        {
            displayImage.texture = ramMaterial.mainTexture;
        }
    }

    // 顯示 ROM 的圖片
    public void ShowRomImage()
    {
        if (displayImage != null && romMaterial != null)
        {
            displayImage.texture = romMaterial.mainTexture;
        }
    }

    // 返回到 TriggerMenuUI
    public void BackToMenu()
    {
        Debug.Log("返回到主選單");

        // 隱藏互動教學面板
        HideButtons();

        // 顯示主選單 Canvas
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true);
        }
    }
}
