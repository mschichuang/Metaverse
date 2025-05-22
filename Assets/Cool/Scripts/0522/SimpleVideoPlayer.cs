using UnityEngine;
using UnityEngine.Video;
using SpatialSys.UnitySDK;

public class SimpleVideoPlayer : MonoBehaviour
{
    public string videoURL = "https://www.youtube.com/watch?v=_S8C9hRUYFQ";
    public MeshRenderer screenRenderer; // 牆面
    public Material videoMaterialTemplate; // Unlit 材質
    public SpatialInteractable interactable;

    private VideoPlayer videoPlayer;
    private Material runtimeMaterial;
    private RenderTexture renderTexture;

    private void Start()
    {
        // 初始化 VideoPlayer
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoURL;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        // 建立 RenderTexture 並設給材質
        renderTexture = new RenderTexture(1920, 1080, 0);
        runtimeMaterial = new Material(videoMaterialTemplate);
        runtimeMaterial.mainTexture = renderTexture;
        screenRenderer.material = runtimeMaterial;

        videoPlayer.targetTexture = renderTexture;
    }

    public void OnInteract()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            interactable.interactText = "Play Video";
        }
        else
        {
            videoPlayer.Play();
            interactable.interactText = "Stop Video";
        }
    }
}
