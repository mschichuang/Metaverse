using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using SimpleJSON; // ✅ 引入 SimpleJSON

public class QuizManager : MonoBehaviour
{
    private string webAppUrl = "https://script.google.com/macros/s/AKfycbyDf-c6IJnXywhNmqR41dwqKl8fcEW9Me78rW5lp084/dev"; // 👈 替換成 Google Apps Script Web App URL

    public TextMeshProUGUI questionText;  // 顯示題目的 UI

    private List<string> questions = new List<string>(); // 存放題目
    private int currentQuestionIndex = 0; // 當前題目索引

    async void Start()
    {
        await LoadQuestionsFromGoogleSheets();
    }

    async Task LoadQuestionsFromGoogleSheets()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(webAppUrl))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log("✅ 成功獲取題目: " + json); // 檢查 JSON 是否正確

                try
                {
                    var parsedJson = JSON.Parse(json);
                    Debug.Log("🔍 解析後的 JSON: " + parsedJson.ToString()); // 確認 JSON 結構

                    foreach (JSONNode questionNode in parsedJson["questions"].AsArray)
                    {
                        string question = questionNode["question"].Value; // ✅ 加上 `.Value`
                        questions.Add(question);
                        Debug.Log("📌 取得題目: " + question);
                    }

                    if (questions.Count > 0)
                        LoadQuestion();
                    else
                        Debug.LogError("❌ 題庫為空！");
                }
                catch (System.Exception e)
                {
                    Debug.LogError("❌ JSON 解析失敗: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("❌ 讀取失敗: " + request.error);
                Debug.LogError("❌ 回應內容: " + request.downloadHandler.text);
            }
        }
    }

    void LoadQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            Debug.Log("🎉 測驗完成！");
            questionText.text = "Quiz Completed!";
            return;
        }

        questionText.text = questions[currentQuestionIndex]; // ✅ 顯示題目
    }
}