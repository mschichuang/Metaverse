using UnityEngine;

public class NPCTriggerPoint : MonoBehaviour
{
    public string pointName; // 觸發點名稱
    private bool isVisited = false; // 是否已探索

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isVisited)
        {
            isVisited = true;
            NPCTriggerTracker tracker = FindObjectOfType<NPCTriggerTracker>(); // 找到 NPC
            if (tracker != null)
            {
                tracker.MarkTriggerAsVisited(this); // 通知 NPC 更新狀態
            }
        }
    }

    public bool GetVisited()
    {
        return isVisited;
    }
}
