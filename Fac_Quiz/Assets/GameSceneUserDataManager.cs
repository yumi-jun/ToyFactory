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
    public string username;
    public string password;
}
public class GameSceneUserDataManager: MonoBehaviour
{
    
    private static GameSceneUserDataManager instance;

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
        UserData userData = JsonUtility.FromJson<UserData>(json);
        
        Debug.Log("ID: " + userData.id);
        Debug.Log("Username: " + userData.username);
        Debug.Log("Password: " + userData.password);

       
        
    }
    

}
