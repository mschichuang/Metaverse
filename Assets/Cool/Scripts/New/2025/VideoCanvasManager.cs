using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoCanvasManager : MonoBehaviour
{
    public GameObject videoCanvas; // 用於播放影片的 Canvas
    public GameObject menuCanvas;  // 主選單 Canvas
    public VideoPlayer videoPlayer; // 影片播放器
    public Button returnButton;    // 返回按鈕

    private void Start()
    {
        // 禁用返回按鈕，直到影片播放完成
        if (returnButton != null)
        {
            returnButton.interactable = false;
        }

        // 設置影片播放完成事件
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    // 當影片播放完成時觸發
    private void OnVideoFinished(VideoPlayer vp)
    {
        if (returnButton != null)
        {
            returnButton.interactable = true; // 啟用返回按鈕
        }
    }

    // 按下返回按鈕時觸發
    public void ReturnToMenu()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // 停止影片播放
        }

        // 隱藏 VideoCanvas，顯示 MenuCanvas
        if (videoCanvas != null)
        {
            videoCanvas.SetActive(false);
        }
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true);
        }
    }
}
