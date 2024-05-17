using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
[Serializable]
public class QuizEntityList
{
    public List<QuizEntity> quizzes;
}

[Serializable]
public class QuizEntity
{
    public int quizid;
    public String quizQues;
    public String lectureName;
}
public class UploadQuiz : MonoBehaviour
{

    private string serverURL = "http://localhost:1234/quiz";

    private quizGenerator _quizGenerator;
    
    public TMP_InputField quizQuesInput;

    private int userid;

    private string json;

    // Start is called before the first frame update
    void Start()
    {
        userid = GameSceneUserDataManager.Instance().GetUserdata().id;
        _quizGenerator = FindObjectOfType<quizGenerator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartQuiz()
    {

        if (GameSceneUserDataManager.Instance().GetQuizString() == null)
        {
            Debug.Log("text is null");
            return;
        }
        
        
        String quizQues = GameSceneUserDataManager.Instance().GetQuizString();
        
        // "-" 기준으로 split 하여 배열에 저장
        List<string> Lists = quizQues.Split(new[] { "\n","\r" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        
        Debug.Log(quizQues);
        
        // 각 질문, 정답 옵션, 정답을 저장할 리스트
        
        
        
       SceneLoader.Instance().LoadQuizScene();
       GameSceneUserDataManager.Instance().setQuizData(Lists);
        
       StartCoroutine(PostRequest(serverURL, quizQues, userid));   


    }

    public void Report()
    {
        string quizQues = quizQuesInput.text;

        StartCoroutine(PostRequest(serverURL, quizQues, userid));
        
        
    }
    string responseText;

    // 서버에 POST 요청을 보내는 메서드
    IEnumerator PostRequest(string url, string quizQues, int userId)
    {
        // 요청 데이터 생성
        WWWForm form = new WWWForm();
        form.AddField("quizQues", quizQues);
        form.AddField("id", userId.ToString()); // 사용자 id 추가

        // POST 요청 보내기
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        
        // 요청 결과 확인
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Network error: " + www.error);
        }
        else
        {
            responseText = www.downloadHandler.text;
            Debug.Log("Quiz uploaded successfully");
        }
        

        if (responseText.Contains("success"))
        {
            Debug.Log("upload succeeded");
            
            
            Debug.Log(responseText);
            

        }
        else if(responseText.Contains("failed"))
        {
            Debug.Log("upload Failed: "+responseText);
         
        }
    }
    




    public void GetUserQuiz()
    {
        string serverURL = "http://localhost:1234/quiz/" +
                           GameSceneUserDataManager.Instance().GetUserdata().id;
        Debug.Log(serverURL);
        
        
        StartCoroutine(GetQuizzes(serverURL));
        
    }

    IEnumerator GetQuizzes(String url)
    {
        
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to get quizzes: " + www.error);
        }
        else
        {
            // 서버로부터 받은 JSON 응답을 Quiz 배열로 파싱
            //string json = www.downloadHandler.text;

            QuizEntityList quizEntities = JsonUtility.FromJson<QuizEntityList>("{\"quizzes\":" + www.downloadHandler.text + "}");

            
            Debug.Log(www.downloadHandler.text);
            
            
            // 받은 퀴즈 리스트를 처리 (예: 각 퀴즈별로 로그로 출력)
            foreach (var quiz in quizEntities.quizzes)
            {
                Debug.Log("id: " + quiz.quizid);
                Debug.Log("lecture name: " + quiz.lectureName);
                Debug.Log("quiz question: " + quiz.quizQues);
            }



           
            
            

        }
    }
}
    
    
    

