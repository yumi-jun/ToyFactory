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
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField loginId;

    public TMP_InputField loginId_login;
    public TMP_InputField password_login;


    
    private WebRequestManager _webRequestManager;
    public void Start()
    {
        _webRequestManager = FindObjectOfType<WebRequestManager>();
    }


    public void SignUpMember()
    {
        if (username.text == null && password.text == null&& loginId.text == null)
        {
            Debug.Log("데이터가 없다.");
            return;
        }
        _webRequestManager.SendDataToServer(loginId.text,username.text,password.text,_webRequestManager.GetInterest());
    }

    public void getMemberList()
    {
        _webRequestManager.GetDataToServer();
    }

    public void LoginMember()
    {
        _webRequestManager.FindDataToServer(loginId_login.text,password_login.text);
    }
   
}
