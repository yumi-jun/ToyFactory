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
        
        Debug.Log(personJson);
        
        UnityWebRequest webreq = UnityWebRequest.PostWwwForm(add_url,personJson);
        
        //set the uploaded data
        byte[] jsonInBytes = new System.Text.UTF8Encoding().GetBytes(personJson);
        Debug.Log(jsonInBytes.Length);
        webreq.uploadHandler = new UploadHandlerRaw(jsonInBytes);
        
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
                Debug.Log("Member sent succesfully");
               
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
