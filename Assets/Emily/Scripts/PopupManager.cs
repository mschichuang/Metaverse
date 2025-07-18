using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text messageText;
    public Button closeButton;

    void Awake()
    {
        popupPanel.SetActive(false);
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
        popupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}