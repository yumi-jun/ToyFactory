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
        generateQuestion();
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
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];
            
            if(QnA[currentQuestion].CorrectAnswer==i+1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {

        List<String> ql = GameSceneUserDataManager.Instance().getQuizData();
        
        
        for (int i = 0; i <ql.Count ; i++)
        {
            QnA[i].Question = ql[i];
        }
        
        currentQuestion = Random.Range(0, QnA.Count);

        QuestionTxt.text = QnA[currentQuestion].Question;
        SetAnswer();
        
        QnA.RemoveAt(currentQuestion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
