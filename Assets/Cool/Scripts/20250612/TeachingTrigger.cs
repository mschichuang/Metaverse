using UnityEngine;
using UnityEngine.Video;

public class TeachingTrigger : MonoBehaviour
{
    public TeachingPanelController panelController;

    [Header("第一頁內容（玩家一進來看到）")]
    public string titlePage1;
    public Sprite imagePage1;
    [TextArea(3, 5)] public string descriptionPage1;

    [Header("第二頁內容（按下確認後）")]
    public string titlePage2;
    public Sprite imagePage2;
    [TextArea(3, 5)] public string descriptionPage2;

    [Header("第三頁影片")]
    public VideoClip videoClip;

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
                    videoClip
                );
            }
            else
            {
                Debug.LogWarning("⚠️ panelController 尚未連結！");
            }
        }
    }
}
