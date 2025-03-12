using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TeachingPanelManager : MonoBehaviour
{
    public GameObject teachingPanel; // UI 面板
    public Button confirmButton; // 確認按鈕
    public VideoPlayer videoPlayer; // 播放影片的 VideoPlayer

    private void Start()
    {
        teachingPanel.SetActive(false); // 開始時隱藏面板
        confirmButton.onClick.AddListener(PlayVideo); // 綁定按鈕事件
    }

    public void ShowTeachingPanel()
    {
        teachingPanel.SetActive(true); // 顯示面板
    }

    private void PlayVideo()
    {
        teachingPanel.SetActive(false); // 關閉面板
        videoPlayer.Play(); // 播放影片
    }
}
