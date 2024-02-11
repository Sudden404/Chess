using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Reg : MonoBehaviour
{
    public void Registration()
    {
        string pass1 = GameObject.Find("Reg_password1").GetComponent<Text>().text;
        string pass2 = GameObject.Find("Reg_password2").GetComponent<Text>().text;
        string login = GameObject.Find("Reg_login").GetComponent<Text>().text;

        if (pass1 == pass2)
        {
            GameObject.Find("DBClass").GetComponent<DBClassScript>().login = login;
            GameObject.Find("DBClass").GetComponent<DBClassScript>().password = pass1;
            GameObject.Find("DBClass").GetComponent<DBClassScript>().Regis();
        }
    }

}
