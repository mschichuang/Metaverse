using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Threading.Tasks;
using SpatialSys.UnitySDK;
using Emily.Scripts;

public class QuizManager : MonoBehaviour
{
    [Header("References")]
    public TrailerBoard trailerBoard;
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
    
    [Header("成績提交")]
    [Tooltip("測驗完成後的提交 Interactable")]
    public SpatialInteractable submitScoreInteractable;
    [Tooltip("Google Apps Script Web App URL")]
    public string googleScriptURL = "https://script.google.com/macros/s/AKfycbx_dFr08pDSFm22YGbXq6GJGAAuNmhY228cUkbz-WyuUWB68DUgFS2WxIy5191Pi-2f/exec";

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
        // 初始化學生資料 (載入組別、姓名、金幣)
        StudentData.Initialize();
        
        quizPanel.SetActive(true);
        
        // Hide the trailer button when quiz starts
        if (trailerBoard != null && trailerBoard.replayButton != null)
        {
            trailerBoard.replayButton.gameObject.SetActive(false);
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
        // 如果是第 11 題 (超過10題),代表測驗已結束,顯示最終成績
        if (currentIndex >= 10)
        {
            ShowFinalScore();
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

        // 點擊 OK 後進入下一題 (或最終成績)
        nextQuestionButton.onClick.RemoveAllListeners();
        nextQuestionButton.onClick.AddListener(() => NextQuestion());
    }

    private void NextQuestion()
    {
        resultPanel.SetActive(false);
        currentIndex++;
        
        if (currentIndex >= 10)
        {
            // 超過10題,顯示最終成績
            ShowFinalScore();
        }
        else
        {
            // 還有題目,繼續顯示下一題
            quizPanel.SetActive(true);
            ShowQuestion();
        }
    }
    
    /// <summary>
    /// 顯示最終成績畫面
    /// </summary>
    private async void ShowFinalScore()
    {
        resultPanel.SetActive(true);
        quizPanel.SetActive(false);
        
        resultText.text = $"測驗完成！得分：{correctCount * 10}分";
        correctAnswerText.text = ""; // 清空正確答案顯示
        
        // 儲存成績到 StudentData (先儲存,確保資料準備好)
        int totalScore = correctCount * 10;
        StudentData.SetQuizScore(totalScore);
        
        // 設定 OK 按鈕:關閉面板,顯示 replay button 和提交物件
        nextQuestionButton.onClick.RemoveAllListeners();
        nextQuestionButton.onClick.AddListener(OnFinalScoreOKClicked);
        
        // 上傳成績到 Google Sheets (保留原有功能,async 放最後)
        string name = PlayerInfoManager.GetPlayerName();
        await UploadScoreAndCoins(name, correctCount);
        if (coinUIManager != null) await coinUIManager.UpdateCoinUI();
        if (coinPanel != null) coinPanel.SetActive(true);
    }
    
    /// <summary>
    /// 最終成績畫面點擊 OK 後
    /// </summary>
    private void OnFinalScoreOKClicked()
    {
        resultPanel.SetActive(false);
        
        // 隱藏測驗開始按鈕,防止重新進入
        if (startQuizInteractable != null)
        {
            startQuizInteractable.gameObject.SetActive(false);
        }
        
        // 顯示 replay button
        if (trailerBoard != null && trailerBoard.replayButton != null)
        {
            trailerBoard.replayButton.gameObject.SetActive(true);
        }
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
    
    #region 成績提交功能
    
    /// <summary>
    /// 提交成績到 Google Sheets
    /// 在 Inspector 中設定到 Interactable 的 On Interact Event
    /// </summary>
    public void OnSubmitScoreClicked()
    {
        if (string.IsNullOrEmpty(googleScriptURL) || googleScriptURL.Contains("YOUR_SCRIPT_ID"))
        {
            return;
        }
        
        // 建構提交 URL
        string url = StudentData.BuildSubmissionURL(googleScriptURL);
        
        // 開啟瀏覽器
        SpatialBridge.spaceService.OpenURL(url);
        
        // 隱藏提交 Interactable (避免重複提交)
        if (submitScoreInteractable != null)
        {
            submitScoreInteractable.gameObject.SetActive(false);
        }
    }
    
    #endregion
}