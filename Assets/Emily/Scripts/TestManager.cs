using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TestManager : MonoBehaviour
{
    public GameObject testPanel;
    public GameObject resultPanel;
    public TMP_Text questionText;
    public TMP_Text[] optionTexts;
    public TMP_Text resultText;
    public Button nextQuestionButton;
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    private string currentComponent;
    
    private Dictionary<string, string> questions = new Dictionary<string, string>()
    {
        { "MB", "Which of the following best describes the role of a motherboard in a computer system?" },
        { "CPU", "What is the primary function of the CPU in a computer?" },
        { "RAM", "Why is RAM considered a crucial component for system performance?" },
        { "SSD", "Compared to a traditional HDD, what is a key advantage of an SSD?" }
    };

    private Dictionary<string, string[]> options = new Dictionary<string, string[]>()
    {
        { "MB", new string[] { 
            "It provides power to all components", 
            "It connects and allows communication between hardware components", 
            "It permanently stores the operating system", 
            "It processes data and instructions" 
        } },

        { "CPU", new string[] { 
            "It acts as the main memory for temporary data storage", 
            "It executes instructions and performs calculations", 
            "It provides long-term data storage", 
            "It manages power distribution in the system" 
        } },

        { "RAM", new string[] { 
            "It stores the operating system permanently", 
            "It temporarily holds data for quick access by the CPU", 
            "It processes instructions like a CPU", 
            "It controls communication between input and output devices" 
        } },

        { "SSD", new string[] { 
            "It has moving mechanical parts that increase durability", 
            "It is generally slower but provides larger storage capacity", 
            "It has no moving parts, making it faster and more reliable", 
            "It requires more power than traditional HDDs" 
        } }
    };

    private Dictionary<string, int> correctAnswers = new Dictionary<string, int>()
    {
        { "MB", 1 },
        { "CPU", 1 },
        { "RAM", 1 },
        { "SSD", 2 }
    };

    public void OpenQuiz(string component)
    {
        testPanel.SetActive(true);
        resultPanel.SetActive(false);
        currentComponent = component;

        questionText.text = questions[component];
        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionTexts[i].text = options[component][i];
            Button button = optionTexts[i].GetComponentInParent<Button>();
            button.onClick.RemoveAllListeners();
            int choiceIndex = i;
            button.onClick.AddListener(() => CheckAnswer(choiceIndex));
        }
    }

    private void CheckAnswer(int choiceIndex)
    {
        testPanel.SetActive(false);
        resultPanel.SetActive(true);

        if (correctAnswers[currentComponent] == choiceIndex)
        {
            resultText.text = "Correct!";
            audioSource.PlayOneShot(correctSound);
            nextQuestionButton.onClick.RemoveAllListeners();
            nextQuestionButton.onClick.AddListener(() => CloseResultPanel());
        }
        else
        {
            resultText.text = "Try Again!";
            audioSource.PlayOneShot(wrongSound);
            nextQuestionButton.onClick.RemoveAllListeners();
            nextQuestionButton.onClick.AddListener(() => RetryQuestion());
        }
    }

    private void CloseResultPanel()
    {
        resultPanel.SetActive(false);
    }

    private void RetryQuestion()
    {
        resultPanel.SetActive(false);
        testPanel.SetActive(true);
    }
}