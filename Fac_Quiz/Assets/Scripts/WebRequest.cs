using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    public string serverURL = "http://localhost:80/api/endpoint";
    public string jsonDataToSend = "{\"username\":\"exampleUser\",\"password\":\"examplePassword\"}";

    public void Start()
    {
        SendDataToServer();
    }

    public void SendDataToServer()
    {
        string jsonData = "{\"username\":\"" + "asdf" + "\",\"password\":\"" + "asdf" + "\"}";
        StartCoroutine(PostRequest(serverURL, jsonData));
    }

    IEnumerator PostRequest(string url, string jsonData)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, jsonData))
        {
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
            }
        }
    }
}

