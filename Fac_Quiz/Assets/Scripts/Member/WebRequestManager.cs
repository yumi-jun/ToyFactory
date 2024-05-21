using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class WebRequestManager : MonoBehaviour
{
    private string serverURL = "http://localhost:1234/members";
    private string serverLoginURL = "http://localhost:1234/member/login";

    private String InterestCombine;
    public GameObject SignInCanvas;
    public GameObject SingUpCanvas;
    public GameObject MainCanvas;
    public GameObject errorCanvas;

    public void Start()
    {
        
    }

    public void SetInterest(String s)
    {
        InterestCombine += s + " ";
    }

    public String GetInterest()
    {
        return InterestCombine;
    }
    
    public void SendDataToServer(String userlogin,String username,String password,String interest)
    {
      string jsonData = "{\"userlogin\":\"" + userlogin + "\",\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"interest\":\""+ interest + "\"}";
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
    

    // 회원 가입 Post to database
    // 요청과 응답에 집중할 것
    IEnumerator PostRequest(string url,string jsonData)
    {
        

        UnityWebRequest www = new UnityWebRequest(url, "POST");
        // 요청에 JSON 데이터 추가
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        // 응답을 받을 DownloadHandler 지정
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // Content-Type 헤더 설정
        www.SetRequestHeader("Content-Type", "application/json");

        // 요청 보내기 및 응답 대기
        yield return www.SendWebRequest();


            if (www.result != UnityWebRequest.Result.Success)
            {
                // 1.1 요청 실패 Failed
                Debug.Log(www.error);
                // 요청 실패할 시
                errorCanvas.SetActive(true);
            }
            else
            {
                // 1.2 요청 성공 success
                Debug.Log("Data sent successfully");
                
                // 서버로 부터 받은 응답 확인
                string responseText = www.downloadHandler.text;

                if (responseText.Contains("success"))
                {
                    Debug.Log("Signed up succeeded");
                    //MainCanvas.SetActive(true);
                    SingUpCanvas.SetActive(false);
                }
                else
                {
                    Debug.Log("Signed Failed: "+responseText);
                    //errorCanvas.SetActive(true);
                }
                
            }
        
    }
    
    
    IEnumerator GetText() {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:1234/members/list");
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
    
    IEnumerator GetUserFromServer(string username, string password)
    {
        string url = "http://localhost:1234/members?userlogin=" + WWW.EscapeURL(username) + "&password=" + WWW.EscapeURL(password);

        Debug.Log(username);

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("success");
            
            
            
            string jsonResponse = www.downloadHandler.text;
            
            Debug.Log(jsonResponse);
            
            GameSceneUserDataManager.Instance().setJsonData(jsonResponse);
            SceneLoader.Instance().LoadQuizScene("factory_asset");
            // 성공적으로 요청을 수행한 경우의 처리
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            // 요청 실패 시의 처리
            Debug.LogError("Network error: " + www.error + " "+jsonResponse);
        }
    }
}