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

        private int currentIndex = 0;

        private void Start()
        {
            if (nextButton != null) nextButton.onClick.AddListener(NextPage);
            if (prevButton != null) prevButton.onClick.AddListener(PrevPage);

            // Initialize
            ShowPage(0);
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
        }
    }
}
