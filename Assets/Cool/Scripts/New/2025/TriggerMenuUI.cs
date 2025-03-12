using UnityEngine;
using UnityEngine.Video;

public class TriggerMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel; // 主選單面板
    public GameObject teachingPanel; // 教學面板
    public GameObject videoPanel; // 播放影片的面板
    public GameObject interactiveTeachingPanel; // 互動教學面板

    [Header("Video Settings")]
    public VideoPlayer videoPlayer; // VideoPlayer 元件

    private void Start()
    {
        // 預設僅顯示主選單
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        teachingPanel?.SetActive(false);
        videoPanel?.SetActive(false);
        interactiveTeachingPanel?.SetActive(false);

        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop(); // 停止影片
        }
    }

    public void ShowTeaching()
    {
        mainMenuPanel.SetActive(false);
        teachingPanel?.SetActive(true);
    }

    public void PlayVideo()
    {
        mainMenuPanel.SetActive(false);
        videoPanel?.SetActive(true);
        videoPlayer?.Play();
    }

    public void ShowInteractiveTeaching()
    {
        mainMenuPanel.SetActive(false);
        interactiveTeachingPanel?.SetActive(true);
    }

    public void CloseMenu()
    {
        mainMenuPanel.SetActive(false);
    }
}
