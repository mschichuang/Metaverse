using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Threading.Tasks;
using SpatialSys.UnitySDK;

public class QuizManager : MonoBehaviour
{
    public SpatialInteractable startQuizInteractable;
    public GameObject quizPanel;
    public TMP_Text questionText;
    public Button[] optionButtons;
    public GameObject resultPanel;
    public TMP_Text resultText;
    public TMP_Text correctAnswerText;
    public Button nextQuestionButton;
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public CoinUIManager coinUIManager;
    public GameObject coinPanel;
    public Emily.Scripts.UserGuideBoard userGuideBoard; // Reference to UserGuideBoard

    private QuestionData[] questions;
    private int currentIndex = 0;
    private int coinsPerQuestion = 2500;
    private int correctCount = 0;

    void Start()
    {
        // Don't load automatically if we want to start on button click
        // LoadQuestions(); 
    }

    // Call this from the Start Button Interactable
    public void StartQuiz()
    {
        quizPanel.SetActive(true);
        if (userGuideBoard != null && userGuideBoard.guideButton != null)
        {
            userGuideBoard.guideButton.gameObject.SetActive(false);
        }
        
        currentIndex = 0;
        correctCount = 0;
        LoadQuestions();
    }

    private async void LoadQuestions()
    {
        string url = "https://script.google.com/macros/s/AKfycbwWQGMamOQ8WOanSBUXOqW005QNCoX_VRWOCAH4MqohSD89y_BTnzM0CkCROuy7LWUT-Q/exec";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SendWebRequest();
            while (!request.isDone)
                await Task.Yield();

            string json = request.downloadHandler.text;
            questions = JsonUtility.FromJson<QuestionList>(json).questions;
            ShowQuestion();
        }
    }

    private async void ShowQuestion()
    {
        if (currentIndex >= 10)
        {
            quizPanel.SetActive(false);
            startQuizInteractable.enabled = false;

            // Show Guide Button again
            if (userGuideBoard != null && userGuideBoard.guideButton != null)
            {
                userGuideBoard.guideButton.gameObject.SetActive(true);
            }

            string name = PlayerInfoManager.GetPlayerName();
            await UploadScoreAndCoins(name, correctCount);
            await coinUIManager.UpdateCoinUI();
            coinPanel.SetActive(true);
            return;
        }

        QuestionData q = questions[currentIndex];
        questionText.text = q.question;
        string correctOption = q.options[q.answer - 1];

        for (int i = 0; i < 4; i++)
        {
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = q.options[i];
            optionButtons[i].onClick.RemoveAllListeners();

            int choiceIndex = i + 1;
            optionButtons[i].onClick.AddListener(() => CheckAnswer(choiceIndex, q.answer, correctOption));
        }
    }

    private void CheckAnswer(int playerChoice, int correctAnswer, string correctOption)
    {
        quizPanel.SetActive(false);
        resultPanel.SetActive(true);

        if (playerChoice == correctAnswer)
        {
            correctCount++;
            resultText.text = "Correct!";
            audioSource.PlayOneShot(correctSound);
        }
        else
        {
            resultText.text = "Wrong!";
            audioSource.PlayOneShot(wrongSound);
        }

        correctAnswerText.text = "Answer: " + correctOption;

        nextQuestionButton.onClick.RemoveAllListeners();
        nextQuestionButton.onClick.AddListener(() => NextQuestion());
    }

    private void NextQuestion()
    {
        resultPanel.SetActive(false);
        quizPanel.SetActive(true);

        currentIndex++;
        ShowQuestion();
    }

    private async Task UploadScoreAndCoins(string name, int correctCount)
    {
        int score = correctCount * 10;
        int coins = correctCount * coinsPerQuestion;
        string json = $"{{\"name\":\"{name}\", \"score\":{score}, \"coins\":{coins}}}";
        string url = PlayerInfoManager.Url + "?action=uploadQuiz";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.SendWebRequest();
            while (!request.isDone)
                await Task.Yield();
        }
    }

    [System.Serializable]
    public class QuestionList
    {
        public QuestionData[] questions;
    }

    [System.Serializable]
    public class QuestionData
    {
        public string question;
        public string[] options;
        public int answer;
    }
}