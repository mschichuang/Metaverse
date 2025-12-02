using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SpatialSys.UnitySDK;

namespace Emily.Scripts
{
    public class TrailerBoard : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("The single trailer page object (GameObject with UI/Video content)")]
        public GameObject trailerPage;

        [Tooltip("The button to skip/close the trailer")]
        public Button skipButton;

        [Tooltip("The button to replay the trailer manually")]
        public Button replayButton;

        [Tooltip("Whether to play the trailer automatically on start")]
        public bool autoPlayOnStart = true;

        private void Start()
        {
            // Setup Skip Button
            if (skipButton != null)
            {
                skipButton.onClick.AddListener(CloseTrailer);
            }

            // Setup Replay Button (Open)
            if (replayButton != null)
            {
                replayButton.onClick.AddListener(PlayTrailer);
            }

            // Initialize state
            if (autoPlayOnStart)
            {
                PlayTrailer();
            }
            else
            {
                CloseTrailer();
            }
        }

        public void PlayTrailer()
        {
            Debug.Log("[TrailerBoard] PlayTrailer called");
            if (trailerPage != null)
            {
                ShowPage();
                // Show skip button when trailer is playing
                if (skipButton != null) skipButton.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("[TrailerBoard] Trailer Page is not assigned!");
            }

            // Hide the replay button while trailer is showing
            if (replayButton != null)
            {
                replayButton.gameObject.SetActive(false);
            }
        }

        public void CloseTrailer()
        {
            Debug.Log("[TrailerBoard] CloseTrailer called");
            if (trailerPage != null)
            {
                // Hide the page using CanvasGroup
                CanvasGroup cg = trailerPage.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.alpha = 0;
                    cg.interactable = false;
                    cg.blocksRaycasts = false;
                }
                else
                {
                    trailerPage.SetActive(false);
                }

                // Pause video if playing
                SpatialVideoPlayer videoPlayer = trailerPage.GetComponentInChildren<SpatialVideoPlayer>();
                if (videoPlayer != null)
                {
                    // We rely on the user closing it to stop watching.
                }
            }

            // Hide skip button
            if (skipButton != null) skipButton.gameObject.SetActive(false);

            // Show the replay button again so user can watch again if needed
            if (replayButton != null)
            {
                replayButton.gameObject.SetActive(true);
            }
        }

        private void ShowPage()
        {
            if (trailerPage == null) return;

            // Show using CanvasGroup
            CanvasGroup cg = trailerPage.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = trailerPage.AddComponent<CanvasGroup>();
            }

            if (cg != null)
            {
                // Keep object active so VideoPlayer can buffer
                if (!trailerPage.activeSelf) trailerPage.SetActive(true); 

                cg.alpha = 1;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
            else
            {
                trailerPage.SetActive(true);
            }

            // Auto-play video when page is shown
            SpatialVideoPlayer videoPlayer = trailerPage.GetComponentInChildren<SpatialVideoPlayer>();
            if (videoPlayer != null)
            {
                Debug.Log("[TrailerBoard] Found SpatialVideoPlayer, calling RewindAndPlay");
                videoPlayer.RewindAndPlay();
            }
            else
            {
                Debug.LogError("[TrailerBoard] SpatialVideoPlayer component not found in Trailer Page children!");
            }
        }
    }
}
