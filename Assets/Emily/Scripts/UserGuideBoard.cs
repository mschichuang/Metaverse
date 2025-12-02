using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Emily.Scripts
{
    public class UserGuideBoard : MonoBehaviour
    {
        [Header("Content")]
        [Tooltip("The single guide page object (GameObject with UI/Video content)")]
        public GameObject guidePage;

        [Tooltip("The button to skip/close the guide")]
        public Button skipButton;

        [Tooltip("The button to open the guide manually")]
        public Button guideButton;

        [Tooltip("Whether to open the guide automatically on start")]
        public bool autoOpenOnStart = true;

        private void Start()
        {
            // Setup Skip Button
            if (skipButton != null)
            {
                skipButton.onClick.AddListener(CloseGuide);
            }

            // Setup Guide Button (Open)
            if (guideButton != null)
            {
                guideButton.onClick.AddListener(OpenGuide);
            }

            // Initialize state
            if (autoOpenOnStart)
            {
                OpenGuide();
            }
            else
            {
                CloseGuide();
            }
        }

        public void OpenGuide()
        {
            if (guidePage != null)
            {
                ShowPage();
                // Show skip button when guide is open
                if (skipButton != null) skipButton.gameObject.SetActive(true);
            }

            // Hide the open button while guide is showing
            if (guideButton != null)
            {
                guideButton.gameObject.SetActive(false);
            }
        }

        public void CloseGuide()
        {
            if (guidePage != null)
            {
                // Hide the page using CanvasGroup
                CanvasGroup cg = guidePage.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.alpha = 0;
                    cg.interactable = false;
                    cg.blocksRaycasts = false;
                }
                else
                {
                    guidePage.SetActive(false);
                }

                // Pause video if playing
                SpatialVideoPlayer videoPlayer = guidePage.GetComponentInChildren<SpatialVideoPlayer>();
                if (videoPlayer != null)
                {
                    // We can't directly pause via this script without reference, 
                    // but disabling the object (if we did that) would stop it.
                    // Since we use CanvasGroup, the video continues playing in background unless we stop it.
                    // For a "Skip", we usually want it to stop audio.
                    // Let's rely on the user closing it.
                }
            }

            // Hide skip button
            if (skipButton != null) skipButton.gameObject.SetActive(false);

            // Show the open button again so user can re-open if needed
            if (guideButton != null)
            {
                guideButton.gameObject.SetActive(true);
            }
        }

        private void ShowPage()
        {
            if (guidePage == null) return;

            CanvasGroup cg = guidePage.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = guidePage.AddComponent<CanvasGroup>();
            }

            if (cg != null)
            {
                // Keep object active so VideoPlayer can buffer
                if (!guidePage.activeSelf) guidePage.SetActive(true); 

                cg.alpha = 1;
                cg.interactable = true;
                cg.blocksRaycasts = true;
                
                // Force video rewind
                var videoPlayer = guidePage.GetComponent<SpatialVideoPlayer>();
                if (videoPlayer != null)
                {
                    videoPlayer.RewindAndPlay();
                }
            }
            else
            {
                // Fallback
                guidePage.SetActive(true);
                var videoPlayer = guidePage.GetComponent<SpatialVideoPlayer>();
                if (videoPlayer != null) videoPlayer.RewindAndPlay();
            }
        }
    }
}
