using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace.Quiz;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField] string quizType = "multiple";
    private List<QustionAndAnswers> qna;

    [SerializeField] GameObject multiplePrefab;
    [SerializeField] GameObject oxPrefab;

    [SerializeField]
    private int targetToyNum; //���� ���� ���� �޶���! ������ 9�����ϱ� 3������ �ϳ��� ����Ƿ� 3��!

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
        else //������ ��� Ǯ���� ��
        {
            isLastQuiz = true;
        }
    }

    public void MakeQuiz()
    {
        //�ڵ����� quiz �����
        if (qna == null)
            return;

        if (quizType.Equals("multiple"))
        {
            for (int i = 0; i < qna.Count; i++)
            {
                GameObject newQuiz = Instantiate(multiplePrefab, transform);
                //quiz question ����
                newQuiz.transform.GetChild(0).GetComponent<TMP_Text>().text = qna[i].Question;
                //quiz answer ����
                for (int j = 0; j < qna[i].Answers.Length; j++)
                {
                    Transform currentOption = newQuiz.transform.GetChild(1).GetChild(j);
                    Debug.Log("문제 번호: " + i + " 답 번호: " + j );
                    Debug.Log("생성될 옵션: " + qna[i].Answers[j]);
                    Debug.Log("옵션 텍스트 컴포넌트: " + currentOption.GetChild(0).GetComponent<TMP_Text>());
                    currentOption.GetChild(0).GetComponent<TMP_Text>().text = qna[i].Answers[j];
                    
                    Debug.Log("i 와 j "+i+j+" "+qna[i].CorrectAnswer);
                    if (qna[i].CorrectAnswer == j + 1)
                    {
                        //���̸� correctAnswer �޾��ֱ�
                        currentOption.GetComponent<Button>().onClick.AddListener(SelectCorrectAnswer);
                    }
                    else
                    {
                        //���̸� wrongAnswer �޾��ֱ�
                        currentOption.GetComponent<Button>().onClick.AddListener(SelectWrongAnswer);
                    }
                }

                quizes.Add(newQuiz.transform);
                if (i != 0)
                {
                    newQuiz.SetActive(false);
                }

            }
        }
    }

    public int GetTargetToyNum()
    {
        return targetToyNum;
    }

    public bool GetIsLastQuiz()
    {
        return isLastQuiz;
    }

    public void SetQNA(List<QustionAndAnswers> qna)
    {
        this.qna = qna;
        MakeQuiz();
    }
}
