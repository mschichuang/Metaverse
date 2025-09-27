using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpecManager : MonoBehaviour
{
    public GameObject specPanel;
    public TMP_Text specText;
    public Button closeButton;

    void Awake()
    {
        specPanel.SetActive(false);
        closeButton.onClick.AddListener(() => specPanel.SetActive(false));
    }

    public void ShowSpec(string specTextContent)
    {
        specText.text = specTextContent;
        specPanel.SetActive(true);
    }
}