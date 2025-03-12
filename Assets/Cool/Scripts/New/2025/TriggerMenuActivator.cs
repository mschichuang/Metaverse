using UnityEngine;

public class TriggerMenuActivator : MonoBehaviour
{
    public GameObject mainMenuPanel; // 主選單面板

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true); // 顯示主選單
        }
    }
}
