using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCTriggerTracker : MonoBehaviour
{
    public List<NPCTriggerPoint> triggerPoints; // 追蹤所有觸發點
    public TextMeshProUGUI npcText; // NPC 的 UI 對話框
    public GameObject npcDialogPanel; // NPC 對話面板（UI）

    private bool playerInRange = false; // 玩家是否在 NPC 附近

    private void Start()
    {
        npcDialogPanel.SetActive(false); // 一開始隱藏 NPC 面板
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            UpdateNPCDialog(); // 玩家進入範圍時立即更新對話
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            npcDialogPanel.SetActive(false); // 離開範圍後隱藏對話
        }
    }

    public void MarkTriggerAsVisited(NPCTriggerPoint point)
    {
        if (triggerPoints.Contains(point))
        {
            UpdateNPCDialog(); // 更新 NPC 對話
        }
    }

    private void UpdateNPCDialog()
    {
        List<string> unvisitedPoints = new List<string>();

        foreach (NPCTriggerPoint point in triggerPoints)
        {
            if (!point.GetVisited()) // 使用 GetVisited() 確保正確檢查
            {
                unvisitedPoints.Add(point.pointName);
            }
        }

        if (unvisitedPoints.Count > 0)
        {
            npcText.text = $"你還有 {unvisitedPoints.Count} 個教學點沒去:\n{string.Join(", ", unvisitedPoints)}";
        }
        else
        {
            npcText.text = "你已經探索完所有元件教學了!";
        }

        npcDialogPanel.SetActive(true); // 顯示 NPC 對話框
    }
}
