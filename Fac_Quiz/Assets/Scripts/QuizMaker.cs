using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizMaker : MonoBehaviour
{
    [SerializeField] string quizType;
    [SerializeField] GameObject multiplePrefab;
    [SerializeField] GameObject oxPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void makeQuiz()
    {
        if (quizType.Equals("multiple"))
        {
            GameObject newQuiz= Instantiate(multiplePrefab, transform);
        }
    }
}
