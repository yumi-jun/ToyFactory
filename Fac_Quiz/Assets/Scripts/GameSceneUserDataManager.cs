using System;
using System.Collections;
using System.Collections.Generic;
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

    public List<String> getQuizData()
    {
        return QuestionData;
    }
    

}
