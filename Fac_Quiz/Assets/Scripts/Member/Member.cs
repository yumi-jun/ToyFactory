using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Member 
{
    // Start is called before the first frame update
    private int id;
    
    public String loginid;
    
    public String password;
    
    public String username;

    public Member()
    {
    }

    public Member(string loginid,string password, string username)
    {
        this.loginid = loginid;
        this.password = password;
        this.username = username;
    }

    public String getpassword()
    {
        return password;
    }

    public void setpassword(String s)
    {
        password = s;
    }

    public String getusername()
    {
        return username;
    }

    public void setusername(String username)
    {
        this.username = username;
    }
    
}
