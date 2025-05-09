using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Threading.Tasks;
using SpatialSys.UnitySDK;

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

public class QuizManager : MonoBehaviour
{
    private const string WebAppUrl = "https://script.google.com/macros/s/AKfycbyQD56ArfGkOuYfa-RRqYFPbSDLbSdsU98UWw86XBcjPaQ4NJ9GhegNnocDrX5hdlfZ/exec";
    private string playerName;
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
    private QuestionData[] questions;
    private int currentQuestionIndex = 0;
    private int totalQuestions = 10;
    private int rewardAmount = 10000;
    private int correctCount = 0;

    void Start()
    {
        playerName = SpatialBridge.actorService.localActor.displayName.Split(' ')[1];
        InvokeRepeating(nameof(LoadQuestions), 0f, 1f);
    }

    async void LoadQuestions()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(WebAppUrl))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            string json = request.downloadHandler.text;
            questions = JsonUtility.FromJson<QuestionList>(json).questions;
            ShowQuestion();
        }
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= totalQuestions)
        {
            quizPanel.SetActive(false);
            startQuizInteractable.enabled = false;

            int finalScore = correctCount * 10;
            UploadScore(playerName, finalScore);
            
            return;
        }

        QuestionData q = questions[currentQuestionIndex];
        questionText.text = q.question;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = q.options[i];
            optionButtons[i].onClick.RemoveAllListeners();

            int choiceIndex = i + 1;
            string correctOption = q.options[q.answer - 1];

            optionButtons[i].onClick.AddListener(() => CheckAnswer(choiceIndex, q.answer, correctOption));
        }
    }

    void CheckAnswer(int playerChoice, int correctAnswer, string correctOption)
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

    void NextQuestion()
    {
        resultPanel.SetActive(false);
        quizPanel.SetActive(true);

        currentQuestionIndex++;
        ShowQuestion();
    }

    public async void UploadScore(string name, int score)
    {
        string url = "https://script.google.com/macros/s/AKfycbyQD56ArfGkOuYfa-RRqYFPbSDLbSdsU98UWw86XBcjPaQ4NJ9GhegNnocDrX5hdlfZ/exec";
        string json = $"{{\"name\":\"{name}\", \"score\":{score}}}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var op = request.SendWebRequest();
            while (!op.isDone)
                await Task.Yield();
        }
    }
}