using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    public void sign()
    {
        GameObject.Find("DBClass").GetComponent<DBClassScript>().login = GameObject.Find("Sign_login").GetComponent<Text>().text;
        GameObject.Find("DBClass").GetComponent<DBClassScript>().password = GameObject.Find("Sign_password").GetComponent<Text>().text;
        GameObject.Find("DBClass").GetComponent<DBClassScript>().Sign();
    }
}
