using UnityEngine;


    public class AnswerScript : MonoBehaviour
    {

        public bool isCorrect = false;

        public QuizManager quizmanager;
        
        
        public void Answer()
        {
            if (isCorrect)
            {
                Debug.Log("Correct Answer");
                quizmanager.Correct();
                
            }
            else
            {
                Debug.Log("Wrong Answer");
                quizmanager.Correct();
            }
        }
    }
