using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Emily.Scripts
{
    public class UserGuideBoard : MonoBehaviour
    {
        [Header("Content")]
        [Tooltip("List of page objects (GameObjects with UI/Video content)")]
        public List<GameObject> pages;

        [Header("Controls")]
        public Button nextButton;
        public Button prevButton;
        public Button closeButton;
        public Button guideButton;  // [Renamed from openButton]

        [Header("Settings")]
        public bool autoOpenOnStart = true;

        private int currentIndex = 0;

        private void Start()
        {
            if (nextButton != null) nextButton.onClick.AddListener(NextPage);
            if (prevButton != null) prevButton.onClick.AddListener(PrevPage);
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
            
            currentIndex = 0;
            ShowPage(currentIndex);

            if (guideButton != null) guideButton.gameObject.SetActive(false);
            if (closeButton != null) closeButton.gameObject.SetActive(true);
        }

        public void CloseGuide()
        {
            if (pages != null)
            {
                foreach (var page in pages)
                {
                    if (page != null) page.SetActive(false);
                }
            }

            if (nextButton != null) nextButton.gameObject.SetActive(false);
            if (prevButton != null) prevButton.gameObject.SetActive(false);
            if (closeButton != null) closeButton.gameObject.SetActive(false);

            if (guideButton != null) guideButton.gameObject.SetActive(true);
        }

        public void NextPage()
        {
            if (pages == null || pages.Count == 0) return;

            currentIndex = (currentIndex + 1) % pages.Count;
            ShowPage(currentIndex);
        }

        public void PrevPage()
        {
            if (pages == null || pages.Count == 0) return;

            currentIndex--;
            if (currentIndex < 0) currentIndex = pages.Count - 1;
            ShowPage(currentIndex);
        }

        private void ShowPage(int index)
        {
            if (pages == null) return;

            for (int i = 0; i < pages.Count; i++)
            {
                if (pages[i] != null)
                {
                    bool isActive = (i == index);
                    pages[i].SetActive(isActive);
                }
            }
            
            // Ensure nav buttons are visible when guide is open
            if (nextButton != null) nextButton.gameObject.SetActive(true);
            if (prevButton != null) prevButton.gameObject.SetActive(true);
        }
    }
}
