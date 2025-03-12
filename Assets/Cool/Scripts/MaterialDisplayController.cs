using UnityEngine;
using UnityEngine.UI;

public class MaterialDisplayController : MonoBehaviour
{
    public RawImage displayImage; // 用於顯示圖片的 UI 元件
    public Material ramMaterial; // RAM 對應的材質
    public Material romMaterial; // ROM 對應的材質

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
}
