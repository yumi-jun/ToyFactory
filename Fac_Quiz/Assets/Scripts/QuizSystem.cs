using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizSystem : MonoBehaviour
{

    private List<Transform> quizes = new List<Transform>();

    [SerializeField]
    private GameObject gaugeBar;

    public float gaugeIncrease;
    private int panelNum = 0;
    private AudioSource errorSound;
    public GameObject conveyor;
    private bool isLastQuiz = false;

    [SerializeField]
    private int targetToyNum; //퀴즈 수에 따라 달라짐! 지금은 9문제니까 3문제에 하나가 생기므로 3개!

    // Start is called before the first frame update
    void Start()
    {
        //Transform[] allQuiz = GetComponentsInChildren<Transform>();
        for(int i = 0; i < transform.childCount; i++)
        {
            quizes.Add(transform.GetChild(i));
            Debug.Log(quizes);
        }
        Debug.Log(quizes.Count);
        errorSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectCorrectAnswer()
    {
        Debug.Log("correct!");
        gaugeBar.GetComponent<GaugeBar>().IncreaseGauge(gaugeIncrease);
        NextQuiz();
    }

    public void SelectWrongAnswer()
    {
        Debug.Log("wrong!");
        //sound
        errorSound.Play();
        NextQuiz();
    }

    public void NextQuiz()
    {
        quizes[panelNum].gameObject.SetActive(false);
        if (++panelNum <= quizes.Count - 1)
        {
            quizes[panelNum].gameObject.SetActive(true);
        }
        else //마지막 퀴즈를 풀었을 때
        {
            isLastQuiz = true;
        }
    }

    public void MakeQuiz()
    {
        //자동으로 quiz 만들기
    }

    public int GetTargetToyNum()
    {
        return targetToyNum;
    }

    public bool GetIsLastQuiz()
    {
        return isLastQuiz;
    }
}
