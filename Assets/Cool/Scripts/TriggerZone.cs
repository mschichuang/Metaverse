using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public InteractiveTeachingManager teachingManager;  // 引用 InteractiveTeachingManager

    // 當玩家進入觸發區域
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // 如果是玩家進入區域
        {
            teachingManager.ShowButtons();  // 顯示按鈕和圖片
        }
    }

    // 當玩家離開觸發區域
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))  // 如果是玩家離開區域
        {
            teachingManager.HideButtons();  // 隱藏按鈕和圖片
        }
    }
}
