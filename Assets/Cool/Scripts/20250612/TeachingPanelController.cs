using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class TeachingPanelController : MonoBehaviour
{
    [Header("第1頁元件")]
    public GameObject page1;
    public TMP_Text titleText1;
    public Image contentImage1;
    public TMP_Text descriptionText1;
    public Button confirmButton1;

    [Header("第2頁元件")]
    public GameObject page2;
    public TMP_Text titleText2;
    public Image contentImage2;
    public TMP_Text descriptionText2;
    public Button confirmButton2;

    [Header("第3頁：影片頁")]
    public GameObject videoPage;
    public VideoPlayer videoPlayer;

    private void Start()
    {
        // 確保一開始三頁都關閉
        page1.SetActive(false);
        page2.SetActive(false);
        videoPage.SetActive(false);

        // 綁定按鈕事件
        confirmButton1.onClick.AddListener(GoToPage2);
        confirmButton2.onClick.AddListener(ClosePanelAndPlayVideo);
    }

    /// <summary>
    /// 顯示第1頁內容
    /// </summary>
    public void ShowPage1(string title, Sprite image, string description)
    {
        page1.SetActive(true);
        page2.SetActive(false);
        videoPage.SetActive(false);

        titleText1.text = title;
        contentImage1.sprite = image;
        descriptionText1.text = description;
    }

    /// <summary>
    /// 顯示第2頁內容
    /// </summary>
    public void ShowPage2(string title, Sprite image, string description)
    {
        page1.SetActive(false);
        page2.SetActive(true);
        videoPage.SetActive(false);

        titleText2.text = title;
        contentImage2.sprite = image;
        descriptionText2.text = description;
    }

    /// <summary>
    /// 切換到第2頁
    /// </summary>
    private void GoToPage2()
    {
        Debug.Log("✅ 第一頁已完成，切換到第二頁");
        ShowPage2(titleText2.text, contentImage2.sprite, descriptionText2.text);
    }

    /// <summary>
    /// 關閉面板並播放影片
    /// </summary>
    private void ClosePanelAndPlayVideo()
    {
        page1.SetActive(false);
        page2.SetActive(false);
        videoPage.SetActive(true);

        if (videoPlayer != null)
        {
            Debug.Log("🎬 播放影片");
            videoPlayer.Play();
        }
    }
}