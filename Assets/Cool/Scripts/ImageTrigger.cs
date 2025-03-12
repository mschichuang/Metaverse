using UnityEngine;
using UnityEngine.UI;

public class ImageTrigger : MonoBehaviour
{
    public Image imageToDisplay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (imageToDisplay != null)
            {
                imageToDisplay.enabled = true; // 顯示圖片
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (imageToDisplay != null)
            {
                imageToDisplay.enabled = false; // 隱藏圖片
            }
        }
    }
}
