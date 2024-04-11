using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnityWebRequestExample : MonoBehaviour
{
    // Start is called before the first frame update
    public string serverURL = "http://localhost:80/member/signup";

    private void Start()
    {
        SendDataToServer("sdf","sdf");
    }

    public void SendDataToServer(string username, string password)
    {
        // JSON 형식의 데이터 생성
        string jsonData = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
        StartCoroutine(PostRequest(serverURL, jsonData));
    }

    IEnumerator PostRequest(string url, string jsonData)
    {
        // UnityWebRequest 객체 생성
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        // 요청에 JSON 데이터 추가
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        // 응답을 받을 DownloadHandler 지정
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // Content-Type 헤더 설정
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 보내기 및 응답 대기
        yield return request.SendWebRequest();

        // 요청 성공 여부 확인
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("POST 요청 성공: " + request.downloadHandler.text);
            // 받은 데이터 처리 (예: UI 업데이트 등)
        }
        else
        {
            Debug.LogError("POST 요청 실패: " + request.error);
        }
    }
}
