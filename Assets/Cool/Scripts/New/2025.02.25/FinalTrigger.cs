using UnityEngine;
using UnityEngine.Video;

public class FinalTrigger : MonoBehaviour
{
    public VideoClip videoClip;
    private VideoPlayer videoPlayer; // 影片播放器

    private void Start()
    {
        // 找到場景中的 VideoPlayer (確保場景只有一個)
        videoPlayer = FindObjectOfType<VideoPlayer>();

        // 確保 VideoPlayer 存在
        if (videoPlayer == null)
        {
            Debug.LogError("找不到 VideoPlayer，請確認場景內有 VideoScreen 並附加 VideoPlayer");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 玩家踩到
        {
            if (videoPlayer != null && videoClip != null)
            {
                videoPlayer.clip = videoClip; // 切換影片
                videoPlayer.Play(); // 播放
            }
        }
    }
}
