using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Emily.Scripts
{
    [RequireComponent(typeof(VideoPlayer))]
    public class SpatialVideoPlayer : MonoBehaviour
    {
        [Tooltip("The RawImage where the video will be displayed")]
        public RawImage targetImage;
        
        private VideoPlayer videoPlayer;
        private RenderTexture renderTexture;

        private void Awake()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            
            // Ensure correct settings for Spatial
            videoPlayer.playOnAwake = false; 
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.isLooping = true;
            
            // Force mute to ensure autoplay works on WebGL
            videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
            
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.errorReceived += OnVideoError;
        }

        private void OnEnable()
        {
            PlayVideo();
        }

        private void OnDisable()
        {
            StopVideo();
        }

        public void PlayVideo()
        {
            if (videoPlayer == null) return;

            // Note: We do NOT access videoPlayer.clip.width here because it causes MissingMethodException on WebGL
            // We wait for Prepare() to finish to get the correct dimensions.

            Debug.Log($"[SpatialVideoPlayer] Preparing video...");
            videoPlayer.Prepare();
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            Debug.Log($"[SpatialVideoPlayer] Video Prepared. Dimensions: {vp.width}x{vp.height}");

            // Create RenderTexture with correct dimensions from the prepared video
            // If vp.width is 0 (error), fallback to 1920x1080
            int width = (vp.width > 0) ? (int)vp.width : 1920;
            int height = (vp.height > 0) ? (int)vp.height : 1080;

            if (renderTexture == null || renderTexture.width != width || renderTexture.height != height)
            {
                if (renderTexture != null) renderTexture.Release();
                renderTexture = new RenderTexture(width, height, 0);
                renderTexture.Create();
            }

            videoPlayer.targetTexture = renderTexture;
            
            if (targetImage != null)
            {
                targetImage.texture = renderTexture;
            }

            vp.Play();
        }

        private void OnVideoError(VideoPlayer vp, string message)
        {
            Debug.LogError($"[SpatialVideoPlayer] Error: {message}");
        }

        public void StopVideo()
        {
            if (videoPlayer != null)
            {
                videoPlayer.Stop();
            }
        }

        private void OnDestroy()
        {
            if (renderTexture != null)
            {
                renderTexture.Release();
                Destroy(renderTexture);
            }
        }
    }
}
