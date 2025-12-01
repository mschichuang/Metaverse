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

        [Header("Controls")]
        public Button closeButton;
        public Button guideButton;

        [Header("Settings")]
        public bool autoOpenOnStart = true;

        private void Start()
        {
            if (closeButton != null) closeButton.onClick.AddListener(CloseGuide);
            if (guideButton != null) guideButton.onClick.AddListener(OpenGuide);

            // Initialize
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
            gameObject.SetActive(true);
            ShowPage();

            if (guideButton != null) guideButton.gameObject.SetActive(false);
            if (closeButton != null) closeButton.gameObject.SetActive(true);
        }

        public void CloseGuide()
        {
            if (guidePage != null)
            {
                // Hide using CanvasGroup to keep video buffered
                CanvasGroup cg = guidePage.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.alpha = 0;
                    cg.blocksRaycasts = false;
                }
                else
                {
                    guidePage.SetActive(false);
                }
            }

            if (closeButton != null) closeButton.gameObject.SetActive(false);
            if (guideButton != null) guideButton.gameObject.SetActive(true);
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
