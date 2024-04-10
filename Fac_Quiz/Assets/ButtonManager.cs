using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Networking;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update

    private string add_url = "http://localhost:80/member?";
    public TMP_InputField name;
    public TMP_InputField password;


    private WebRequestManager _webRequestManager;
    public void Start()
    {
        _webRequestManager = FindObjectOfType<WebRequestManager>();
    }


    public void AddMember()
    {
        if (name.text == null && name.text == null)
        {
            Debug.Log("데이터가 없다.");
            return;
        }
        _webRequestManager.SendLoginDataToServer(name.text,password.text);
    }
    public void getPerson()
    {
        StartCoroutine(RandomRequest());
    }

    public void addPerson()
    {
        StartCoroutine(AddRequest());
    }

    private string parseInput()
    {
        Member member = new Member();
        member.username = name.text;
        member.password = password.text;
        
       // Debug.Log(name.text+password.text);

        return JsonUtility.ToJson(member);

    }
    IEnumerator AddRequest()
    {
        string personJson = parseInput();
        
        Debug.Log("json : "+personJson);
        
        UnityWebRequest webreq = UnityWebRequest.Put(add_url,personJson);
        
        //set the uploaded data //스트링으로 넘기면 json 구성이 깨지기 때문에 byte로 변환 후 파일로 업로드해준다
        byte[] jsonInBytes = new System.Text.UTF8Encoding().GetBytes(personJson);
        
        webreq.uploadHandler = new UploadHandlerRaw(jsonInBytes);
        webreq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
         
        // set the header 
        
        webreq.SetRequestHeader("Content-Type","application/json");

        yield return webreq.SendWebRequest(); //send the request

        switch (webreq.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(("Error: "+webreq.error));
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Member sent succesfully"+webreq.downloadHandler.text);
               
                break;
        }

    }
    IEnumerator RandomRequest()
    {
        UnityWebRequest webreq = UnityWebRequest.Get(add_url);
        
        
        // set the header 
        
        webreq.SetRequestHeader("Accept","application/json");

        yield return webreq.SendWebRequest(); //send the request

        switch (webreq.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(("Error: "+webreq.error));
                break;
            case UnityWebRequest.Result.Success:
                string json = webreq.downloadHandler.text;
                Debug.Log("received person: "+json);
                ParseResult(json);
                break;
        }

    }

    private void ParseResult(string json)
    {
        Member member = JsonUtility.FromJson<Member>(json);

        name.text = member.getusername();
        password.text = member.getpassword();
    }
   
}
