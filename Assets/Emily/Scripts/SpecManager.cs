using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpecManager : MonoBehaviour
{
    public GameObject specPanel;
    public Button closeButton;
    public RawImage specImage;
    public AspectRatioFitter aspectFitter;

    void Awake()
    {
        specPanel.SetActive(false);
        closeButton.onClick.AddListener(() => specPanel.SetActive(false));
    }

    public void ShowSpec(Texture texture)
    {
        specImage.texture = texture;

        float width = texture.width;
        float height = texture.height;
        aspectFitter.aspectRatio = width / height;

        specPanel.SetActive(true);
    }
}