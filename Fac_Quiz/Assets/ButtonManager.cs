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


    public void SignUpMember()
    {
        if (name.text == null && name.text == null)
        {
            Debug.Log("데이터가 없다.");
            return;
        }
        _webRequestManager.SendDataToServer(name.text,password.text);
    }

    public void getMemberList()
    {
        _webRequestManager.GetDataToServer();
    }

    public void LoginMember()
    {
        _webRequestManager.FindDataToServer(name.text,password.text);
    }
   
}
