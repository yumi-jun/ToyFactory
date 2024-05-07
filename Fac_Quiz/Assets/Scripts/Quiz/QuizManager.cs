using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Quiz;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class QuizManager : MonoBehaviour
{

    public List<QustionAndAnswers> QnA;

    public GameObject[] options;

    public int currentQuestion;

    public TextMeshProUGUI QuestionTxt;
    
    // Start is called before the first frame update
    void Start()
    {
        QnA = new List<QustionAndAnswers>(); // QnA 리스트 초기화
        AddQuestions();
        generateQuestion();
        
    }

    public void AddQuestions()
    {
        List<String> ql = GameSceneUserDataManager.Instance().getQuizData();
        
        
        List<string> questions = new List<string>();
        List<string> options = new List<string>();
        List<string> answers = new List<string>();

        // 데이터 처리
        string currentQuestion = "";
        string currentOptions = "";
        foreach (string line in ql)
        {
            if (line.StartsWith("Q."))
            {
                // 질문일 경우
                currentQuestion = line;
            }
            else if (line.StartsWith("a.") || line.StartsWith("b.") || line.StartsWith("c."))
            {
                Debug.Log("hh");
                // 정답 옵션일 경우
                currentOptions = line + "\n";
                options.Add(currentOptions);
            }
            else if (line.StartsWith("A."))
            {
                // 정답일 경우
                questions.Add(currentQuestion);
                answers.Add(line);
                currentOptions = ""; // 옵션 초기화
            }
        }

      

        // 결과 출력
        for (int i = 0; i < questions.Count; i++)
        {
            Debug.Log($"질문: {questions[i]}");
            Debug.Log($"답변 옵션:\n{options[i]}");
            Debug.Log($"정답: {answers[i]}\n");

           int ca = 0;
            if (answers[i].Contains("a"))
            {
                ca = 1;
            }
            else if (answers[i].Contains("b"))
            {
                ca = 2;

            }
            else if (answers[i].Contains("c"))
            {
                ca = 3;
            }
            else if (answers[i].Contains("d"))
            {
                ca =4;
            }
            QnA.Add(new QustionAndAnswers() { Question =questions[i] ,Answers = new string[] { options[3*i+0], options[3*i+1],options[3*i+2] }, CorrectAnswer = ca });
        }



       
        
    }
    public void Correct()
    {
        generateQuestion();
    }

    void SetAnswer()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answers[i];
            
            if(QnA[currentQuestion].CorrectAnswer==i+1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        
        Debug.Log(QnA.Count);
        
        // QnA 리스트가 비어 있는 경우
        if (QnA.Count == 0)
        {
            Debug.LogError("QnA 리스트가 비어 있습니다.");
            SceneLoader.Instance().LoadQuizScene("GameFin");
            return;
        }
        
        currentQuestion = Random.Range(0, QnA.Count);

        // 현재 질문이 리스트의 범위를 벗어나지 않도록 보정
        currentQuestion = Mathf.Clamp(currentQuestion, 0, QnA.Count - 1);

        QuestionTxt.text = QnA[currentQuestion].Question;
        SetAnswer();
        
        QnA.RemoveAt(currentQuestion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
