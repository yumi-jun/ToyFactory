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

        foreach (String q in ql)
        {
            QnA.Add(new QustionAndAnswers() { Question = q.ToString() ,Answers = new string[] { "답변 1", "답변 2" }, CorrectAnswer = 1 });
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
