using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager : MonoBehaviour
{
    public string serverURL = "http://localhost:80/member/signup";
    public string serverLoginURL = "http://localhost:80/member/check";



    public GameObject SignInCanvas;
    public GameObject SingUpCanvas;

    public void Start()
    {
        SignInCanvas.SetActive(false);
        SingUpCanvas.SetActive(true);
    }

    public void SendLoginDataToServer(String username, String password)
    {
        string jsonLoginData = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
        StartCoroutine(PostRequest(serverLoginURL, jsonLoginData));
    }
    
    public void SendDataToServer(String username,String password)
    {
        string jsonData = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
        StartCoroutine(PostRequest(serverURL, jsonData));
    }

    public void GetDataToServer()
    {
        StartCoroutine(GetText());
    }

    public void FindDataToServer(string username,string password)
    {
        StartCoroutine(GetUserFromServer(username,password));
    }
    

    IEnumerator PostRequest(string url, string jsonData)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, jsonData))
        {
            
            Debug.Log(www);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            
            Debug.Log("Request Body: " + System.Text.Encoding.UTF8.GetString(bodyRaw)); // 요청 본문을 출력합니다.

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Data sent successfully");
                SignInCanvas.SetActive(true);
                SingUpCanvas.SetActive(false);
            }
        }
    }
    
    
    IEnumerator GetText() {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:80/member/list");
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
 
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
    
    IEnumerator GetUserFromServer(string username,string password)
    {
        string url = "http://localhost:80/member/check?username=" + WWW.EscapeURL(username) + "&password=" + WWW.EscapeURL(password);
        
        Debug.Log(username);
        
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string userData = www.downloadHandler.text;
            Debug.Log(userData);
            
            FindObjectOfType<GameSceneUserDataManager>().setJsonData(userData);
            
            FindObjectOfType<SceneLoader>().LoadNextScene();
            // 받은 데이터를 파싱하여 사용합니다.
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }
}