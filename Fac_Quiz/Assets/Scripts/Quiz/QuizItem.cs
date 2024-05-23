using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizItem : MonoBehaviour
{
    public TMP_Text questionText;

    public Button button;
    
    public void Setup(string question)
    {
        questionText.text = question;
    }

 
}