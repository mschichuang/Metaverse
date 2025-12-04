using UnityEngine;
using UnityEngine.Video; // 這一行最重要，讓我們可以控制影片

public class PlayVideoOnTrigger : MonoBehaviour
{
    // 這是一個「插槽」，等一下要把 VIDEOSCREEN 拉進來
    public VideoPlayer targetScreen; 

    // 當有東西進入 Trigger 範圍時
    void OnTriggerEnter(Collider other)
    {
        // 檢查撞到的是不是玩家 (Tag 必須是 Player)
        if (other.CompareTag("Player")) 
        {
            // 執行播放
            targetScreen.Play(); 
        }
    }
}