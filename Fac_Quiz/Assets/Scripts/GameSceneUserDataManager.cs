using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class UserData
{
    public int id;
    public string userlogin;
    public string username;
    public string password;
}
public class GameSceneUserDataManager: MonoBehaviour
{
    
    private static GameSceneUserDataManager instance=new GameSceneUserDataManager();

    private UserData userData;

    private List<String> QuestionData;

    private String QuizData;
    private GameSceneUserDataManager() {
        // 생성자는 외부에서 호출못하게 private 으로 지정해야 한다.
    }

    public static GameSceneUserDataManager Instance() {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            // 씬 전환 시 파괴되지 않도록 설정
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            // 이미 인스턴스가 존재하면 이전 인스턴스를 파괴
            Destroy(gameObject);
        }
    }
    
    private string json;
   // public GameObject id;
    //public GameObject username;

    public void setJsonData(String json)
    {
        userData = JsonUtility.FromJson<UserData>(json);
        
        Debug.Log(userData);
        
        Debug.Log("ID: " + userData.id);
        Debug.Log("loginid"+userData.userlogin);
        Debug.Log("Username: " + userData.username);
        Debug.Log("Password: " + userData.password);

       
        
    }

    public UserData GetUserdata()
    {
        return userData;
    }

    public void setQuizData(List<String> str)
    {
        
        
        QuestionData = str;
        

    }

    public void SetQuizString(String str)
    {
        QuizData = str;
    }

    public String GetQuizString()
    {
        //Debug.Log("quiz : "+QuizData);
        return QuizData;
    }

    public List<String> getQuizData()
    {
        return QuestionData;
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
            else if (line.StartsWith("a)") || line.StartsWith("b)") || line.StartsWith("c)")|| line.StartsWith("d)")|| line.StartsWith("e)"))
            {
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
            else if (line.StartsWith(" 해설"))
            {
                Debug.Log("해설 : "+line);
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
                ca = 4;
            }
            else if (answers[i].Contains("e"))
            {
                ca = 5;
            }
        }
    }
}
