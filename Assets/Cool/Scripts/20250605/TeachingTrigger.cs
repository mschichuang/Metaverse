using UnityEngine;

public class TeachingTrigger : MonoBehaviour
{
    public TeachingPanelController panelController;
    public string title;
    public Sprite image;
    [TextArea(3, 5)]
    public string description;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("⚠️ 碰撞發生：" + other.name);

    // 取消 CompareTag，直接判斷碰撞物件名稱或元件
    if (!hasTriggered && other.name.Contains("Avatar"))  // 簡單判斷名字包含 Avatar
    {
        Debug.Log("✅ 玩家碰到教學點：" + title);
        hasTriggered = true;

        if (panelController != null)
        {
            panelController.ShowPanel(title, image, description);
        }
        else
        {
            Debug.LogWarning("⚠️ panelController 尚未連結！");
        }
    }
}
}
