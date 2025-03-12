using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class ComponentTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel; // 主面板 (Component 面板)
    public GameObject menuPanel; // 主選單面板 (TriggerMenuUI)
    public GameObject videoPanel; // 撥放影片的面板
    public RawImage descriptionImage; // 描述圖片
    public TMP_Text coinText; // 金幣文字

    [Header("Video Settings")]
    public VideoPlayer videoPlayer; // VideoPlayer 控制影片播放

    [Header("Component Data")]
    public GameObject modelToShow; // 要顯示的 3D 模型
    public GameObject modelDisplayArea; // 模型顯示區域（空物件，共用可行）
    public AudioClip audioClip; // 音效
    public Texture descriptionTexture; // 描述圖片
    public Vector3 modelRotation; // 模型的自訂旋轉角度（Inspector 設置）
    public Vector3 modelPositionOffset; // 模型位置偏移（上下左右）
    public Vector3 modelScale = Vector3.one; // 模型縮放倍率（Inspector 設置，預設為 1,1,1）

    [Header("Coin System")]
    public int coinReward = 200;

    private bool hasTriggered = false; // 是否曾進入過觸發點
    private bool coinClaimed = false; // 判斷金幣是否已領取
    private GameObject currentModelInstance; // 當前顯示的模型實例
    private AudioSource audioSource; // 音效播放源

    private void Start()
    {
        // 確保音效源存在
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false; // 避免自動播放音效

        // 初始隱藏面板
        if (panel != null) panel.SetActive(false);
        if (videoPanel != null) videoPanel.SetActive(false);
    }

    public void ShowMenuPanel()
    {
        if (menuPanel != null) menuPanel.SetActive(true); // 顯示主選單
        if (panel != null) panel.SetActive(false); // 隱藏 Component 面板
    }

    public void ShowComponentPanel()
    {
        if (menuPanel != null) menuPanel.SetActive(false); // 隱藏主選單
        if (panel != null) panel.SetActive(true); // 顯示 Component 面板
    }

    public void PlayVideo()
    {
        if (videoPanel != null && videoPlayer != null)
        {
            videoPanel.SetActive(true); // 顯示影片面板
            menuPanel.SetActive(false); // 隱藏主選單
            videoPlayer.Play(); // 撥放影片
        }
    }

    public void StopVideo()
    {
        if (videoPanel != null)
        {
            videoPanel.SetActive(false); // 隱藏影片面板
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // 停止播放影片
        }

        if (menuPanel != null)
        {
            menuPanel.SetActive(true); // 返回主選單
        }
    }

    private void ShowModel()
    {
        if (modelToShow != null && modelDisplayArea != null)
        {
            // 清理舊模型
            foreach (Transform child in modelDisplayArea.transform)
            {
                Destroy(child.gameObject);
            }

            // 生成新模型
            currentModelInstance = Instantiate(modelToShow, modelDisplayArea.transform);
            currentModelInstance.transform.localPosition = modelPositionOffset;
            currentModelInstance.transform.localRotation = Quaternion.Euler(modelRotation);
            currentModelInstance.transform.localScale = modelScale;
        }
    }

    private void HideModel()
    {
        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance);
            currentModelInstance = null;
        }
    }

    private void TryAddCoins()
    {
        if (coinClaimed)
        {
            Debug.Log("金幣已領取，無法再次獲得");
            return;
        }

        string currentText = coinText.text;
        string[] parts = currentText.Split(':');
        int currentCoins = 0;
        if (parts.Length > 1)
        {
            int.TryParse(parts[1].Trim(), out currentCoins);
        }

        currentCoins += coinReward;
        coinText.text = "Coin: " + currentCoins;
        coinClaimed = true;
    }
}
