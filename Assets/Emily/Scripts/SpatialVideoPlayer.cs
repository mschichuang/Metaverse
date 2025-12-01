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
        private bool isPreloaded = false;

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
            videoPlayer.seekCompleted += OnSeekCompleted;
        }

        private void Start()
        {
            // Auto-preload on start
            PreloadVideo();
        }

        private void OnEnable()
        {
            // If already prepared, rewind and play immediately
            if (videoPlayer.isPrepared)
            {
                RewindAndPlay();
            }
            else if (!isPreloaded)
            {
                PreloadVideo();
            }
        }

        private void OnDisable()
        {
            if (videoPlayer != null)
            {
                videoPlayer.Pause(); // Pause instead of Stop to keep buffer
            }
        }

        public void PreloadVideo()
        {
            if (videoPlayer == null || isPreloaded) return;

            // Setup Source for URL if needed (Inspector URL is handled automatically by VideoPlayer, 
            // but we need to trigger Prepare)
            
            Debug.Log($"[SpatialVideoPlayer] Preloading video...");
            videoPlayer.Prepare();
            isPreloaded = true;
        }

        // [NEW] Method to manually rewind and play
        public void RewindAndPlay()
        {
            if (videoPlayer != null && videoPlayer.isPrepared)
            {
                // Hide image to prevent flash of old frame
                if (targetImage != null) targetImage.color = new Color(1, 1, 1, 0);
                
                videoPlayer.time = 0;
                videoPlayer.Play();
            }
            else
            {
                PlayVideo();
            }
        }

        private void OnSeekCompleted(VideoPlayer vp)
        {
            // Restore visibility when seek is done and new frame is ready
            if (targetImage != null) targetImage.color = Color.white;
        }

        public void PlayVideo()
        {
            if (videoPlayer == null) return;

            if (videoPlayer.isPrepared)
            {
                videoPlayer.Play();
            }
            else
            {
                PreloadVideo();
            }
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            Debug.Log($"[SpatialVideoPlayer] Video Prepared. Dimensions: {vp.width}x{vp.height}");

            // Create RenderTexture with correct dimensions
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
                targetImage.color = Color.white; // Ensure visible
            }

            // If the object is active (user is looking at it), play. 
            // If not (background preload), just stay prepared.
            if (gameObject.activeInHierarchy)
            {
                vp.Play();
            }
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
