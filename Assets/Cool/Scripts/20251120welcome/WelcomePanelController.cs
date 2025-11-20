using UnityEngine;
using UnityEngine.UI;

public class WelcomePanelController : MonoBehaviour
{
    public GameObject welcomePanel;
    public Button confirmButton;

    void Start()
    {
        // 一進入世界就顯示面板
        welcomePanel.SetActive(true);

        // 註冊按鈕事件
        confirmButton.onClick.AddListener(ClosePanel);
    }

    void ClosePanel()
    {
        welcomePanel.SetActive(false);
    }
}
