using UnityEngine;

public class ComponentRAMManager : MonoBehaviour
{
    public GameObject componentCanvas; // RAM 教學的 Canvas
    public GameObject menuCanvas;      // 主選單的 Canvas

    // 返回按鈕行為
    public void ReturnToMenu()
    {
        // 隱藏教學 Canvas
        if (componentCanvas != null)
        {
            componentCanvas.SetActive(false);
        }

        // 顯示主選單 Canvas
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true);
        }
    }
}
