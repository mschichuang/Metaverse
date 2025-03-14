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
    private const string WebAppUrl = "https://script.google.com/macros/s/AKfycbwWQGMamOQ8WOanSBUXOqW005QNCoX_VRWOCAH4MqohSD89y_BTnzM0CkCROuy7LWUT-Q/exec";
    public SpatialInteractable startQuizInteractable;
    public GameObject quizPanel;
    public TMP_Text questionText;
    public Button[] optionButtons;
    public GameObject resultPanel;
    public TMP_Text resultText;
    public TMP_Text correctAnswerText;
    public Button nextQuestionButton;
    private QuestionData[] questions;
    private int currentQuestionIndex = 0;
    private int totalQuestions = 10;
    private ulong rewardAmount = 1000;

    void Start()
    {
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
            resultText.text = "Correct!";
            SpatialBridge.inventoryService.AwardWorldCurrency(rewardAmount);
        }
        else
            resultText.text = "Wrong!";
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
}