using UnityEngine;
using UnityEngine.UI;
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


    private QuestionData[] questions;
    private int currentIndex = 0;
    private int coinsPerQuestion = 3000;
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

    private void LoadQuestions()
    {
        // 題目 JSON (直接寫在程式碼中,避免 Spatial SDK 的 Resources.Load 限制)
        string questionsJson = @"{
  ""questions"": [
    {
      ""question"": ""What are the five components of the von Neumann architecture?"",
      ""options"": [
        ""CPU, RAM, Hard Drive, Keyboard, Monitor"",
        ""Bus System, Cache Memory, Registers, Input/Output Unit, Control Unit"",
        ""Memory Unit, Arithmetic/Logic Unit, Input Unit, Output Unit, Control Unit"",
        ""Memory Unit, Cache Memory, Arithmetic/Logic Unit, Input Unit, Output Unit""
      ],
      ""answer"": 3
    },
    {
      ""question"": ""Why is cache memory used in computer systems?"",
      ""options"": [
        ""To store large amounts of data permanently"",
        ""To increase the capacity of the main memory"",
        ""To reduce the time taken for memory accesses"",
        ""To perform complex arithmetic and logic operations""
      ],
      ""answer"": 3
    },
    {
      ""question"": ""What is the role of the bus in a von Neumann machine?"",
      ""options"": [
        ""Manages the flow of control signals"",
        ""Performs data processing operations"",
        ""Holds data and instructions temporarily"",
        ""Moves data between CPU, memory, and I/O devices""
      ],
      ""answer"": 4
    },
    {
      ""question"": ""Which component of the CPU acts as the organizing force responsible for the fetch-execute cycle?"",
      ""options"": [
        ""Bus System"",
        ""Control Unit"",
        ""Cache Memory"",
        ""Arithmetic/Logic Unit""
      ],
      ""answer"": 2
    },
    {
      ""question"": ""What role do registers play in a CPU?"",
      ""options"": [
        ""They store instructions for future execution"",
        ""They control the flow of electricity through the CPU"",
        ""They manage the flow of data between CPU and memory"",
        ""They provide high-speed storage for data currently being processed""
      ],
      ""answer"": 4
    },
    {
      ""question"": ""What is the primary function of the Arithmetic/Logic Unit in a CPU?"",
      ""options"": [
        ""Manages input and output devices"",
        ""Stores data temporarily for fast access"",
        ""Executes instructions fetched from memory"",
        ""Performs arithmetic and logical operations on data""
      ],
      ""answer"": 4
    },
    {
      ""question"": ""Which statement accurately describes the difference between RAM and ROM?"",
      ""options"": [
        ""RAM can be changed, while ROM cannot"",
        ""ROM is volatile, while RAM is non-volatile"",
        ""RAM is faster than ROM in accessing data"",
        ""RAM is used for permanent storage, while ROM is used for temporary storage""
      ],
      ""answer"": 1
    },
    {
      ""question"": ""What is the primary purpose of ROM in a computer system?"",
      ""options"": [
        ""To store frequently accessed data"",
        ""To hold temporary program instructions"",
        ""To store data during power-off situations"",
        ""To provide permanent storage for system instructions""
      ],
      ""answer"": 4
    },
    {
      ""question"": ""What distinguishes SSDs from traditional hard disks?"",
      ""options"": [
        ""SSDs use more power than hard disks"",
        ""SSDs have no moving parts and are faster"",
        ""SSDs have slower access times compared to hard disks"",
        ""SSDs are smaller in physical size compared to hard disks""
      ],
      ""answer"": 2
    },
    {
      ""question"": ""What type of memory is used in SSDs?"",
      ""options"": [
        ""Flash memory"",
        ""DRAM memory"",
        ""Volatile memory"",
        ""Magnetic memory""
      ],
      ""answer"": 1
    }
  ]
}";
        
        // 解析 JSON
        questions = JsonUtility.FromJson<QuestionList>(questionsJson).questions;
        
        Debug.Log($"[QuizManager] 成功載入 {questions.Length} 題");
        ShowQuestion();
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
            
            // 答對時增加金幣 (每題 2500 金幣)
            StudentData.AddCoins(coinsPerQuestion);
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
        
        // 更新金幣 UI
        if (coinUIManager != null) await coinUIManager.UpdateCoinUI();
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
        // Google Apps Script Web App URL (用於學生成績提交)
        string googleScriptURL = "https://script.google.com/macros/s/AKfycbx_dFr08pDSFm22YGbXq6GJGAAuNmhY228cUkbz-WyuUWB68DUgFS2WxIy5191Pi-2f/exec";
        
        // 建構提交 URL (測驗區不含組裝資料)
        string url = StudentData.BuildSubmissionURL(googleScriptURL, new System.Collections.Generic.Dictionary<string, int>());
        
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