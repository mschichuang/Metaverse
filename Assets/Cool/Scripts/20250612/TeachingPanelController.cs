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
        }
    }

    /// <summary>
    /// 從 TeachingTrigger 傳入所有三頁的內容。
    /// </summary>
    public void ShowPanel(
        string title1, Sprite image1, string desc1,
        string title2, Sprite image2, string desc2,
        VideoClip videoClip)
    {
        gameObject.SetActive(true);

        // 顯示第一頁
        page1.SetActive(true);
        page2.SetActive(false);
        page3.SetActive(false);

        // 第一頁資料
        titleText.text = title1;
        contentImage.sprite = image1;
        descriptionText.text = desc1;

        // 第二頁資料
        titleText2.text = title2;
        contentImage2.sprite = image2;
        descriptionText2.text = desc2;

        // 第三頁影片
        if (videoPlayer != null && videoClip != null)
        {
            videoPlayer.clip = videoClip;
            closeButton.gameObject.SetActive(false); // 影片沒播完前隱藏關閉鈕
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
            videoPlayer.Stop();
            videoPlayer.Play();
            closeButton.gameObject.SetActive(false); // 等影片播完才顯示
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
