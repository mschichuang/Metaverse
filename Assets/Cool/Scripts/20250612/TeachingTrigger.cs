using UnityEngine;

public class TeachingTrigger : MonoBehaviour
{
    public TeachingPanelController panelController;

    [Header("第一頁內容")]
    public string titlePage1;
    public Sprite imagePage1;
    [TextArea(3, 5)] public string descriptionPage1;

    [Header("第二頁內容")]
    public string titlePage2;
    public Sprite imagePage2;
    [TextArea(3, 5)] public string descriptionPage2;

    [Header("影片網址")]
    [Tooltip("請填入 HTTPS 開頭的 mp4 影片網址")]
    public string videoUrl = "https://www.w3schools.com/html/mov_bbb.mp4";

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.name.Contains("Avatar"))
        {
            hasTriggered = true;

            if (panelController != null)
            {
                panelController.ShowPanel(
                    titlePage1, imagePage1, descriptionPage1,
                    titlePage2, imagePage2, descriptionPage2,
                    videoUrl
                );
            }
            else
            {
                Debug.LogWarning("⚠️ TeachingPanelController 未連結！");
            }
        }
    }
}
