using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class TeachingPanelController : MonoBehaviour
{
    [Header("Page 1 元件")]
    public GameObject page1;
    public TMP_Text titleText;
    public Image contentImage;
    public TMP_Text descriptionText;
    public Button confirmButton;

    [Header("Page 2 元件")]
    public GameObject page2;
    public TMP_Text titleText2;
    public Image contentImage2;
    public TMP_Text descriptionText2;
    public Button confirmButton2;

    [Header("Page 3 (影片頁)")]
    public GameObject page3;
    public VideoPlayer videoPlayer;
    public Button closeButton;

    private void Start()
    {
        gameObject.SetActive(false);

        confirmButton.onClick.AddListener(ShowPage2);
        confirmButton2.onClick.AddListener(ShowPage3);
        closeButton.onClick.AddListener(HidePanel);

        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;

            // 加入錯誤訊息偵測，方便查 Spatial 播放問題
            videoPlayer.errorReceived += (vp, msg) =>
            {
                Debug.LogError("❌ 影片播放錯誤：" + msg);
            };
        }
    }

    /// <summary>
    /// 接收三頁的資料與影片網址（String）
    /// </summary>
    public void ShowPanel(
        string title1, Sprite image1, string desc1,
        string title2, Sprite image2, string desc2,
        string videoUrl)
    {
        gameObject.SetActive(true);

        // 第一頁內容
        page1.SetActive(true);
        page2.SetActive(false);
        page3.SetActive(false);
        titleText.text = title1;
        contentImage.sprite = image1;
        descriptionText.text = desc1;

        // 第二頁內容
        titleText2.text = title2;
        contentImage2.sprite = image2;
        descriptionText2.text = desc2;

        // 第三頁影片網址設定（注意：使用 URL 模式）
        if (videoPlayer != null && !string.IsNullOrEmpty(videoUrl))
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = videoUrl;

            // 提前停掉上一部影片
            videoPlayer.Stop();
            closeButton.gameObject.SetActive(false);
        }
    }

    private void ShowPage2()
    {
        page1.SetActive(false);
        page2.SetActive(true);
        page3.SetActive(false);
    }

    private void ShowPage3()
    {
        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(true);

        if (videoPlayer != null)
        {
            videoPlayer.Play();
            closeButton.gameObject.SetActive(false);
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        closeButton.gameObject.SetActive(true);
    }

    private void HidePanel()
    {
        if (videoPlayer != null) videoPlayer.Stop();

        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(false);

        gameObject.SetActive(false);
    }
}
