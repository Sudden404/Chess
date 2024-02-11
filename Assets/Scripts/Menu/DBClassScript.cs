using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class DBClassScript : MonoBehaviour
{
    public string login;
    public string password;

    public void Regis()
    {
        StartCoroutine(RegisterUser(login, password));
    }
    public void Sign()
    {
        StartCoroutine(Login(login, password));
    }
    public IEnumerator RegisterUser(string login, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", password);
        WWW www = new WWW("http://localhost/db.php", form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("������" + www.error);
            yield break;
        }
        Debug.Log("������ �������" + www.text);
        if (www.text == "1")
            GameObject.Find("success_reg").GetComponent<Text>().text = "�����";
        else
            GameObject.Find("success_reg").GetComponent<Text>().text = "������";
    }

    public IEnumerator Login(string login, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", password);
        WWW www = new WWW("http://localhost/dblogin.php", form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("������" + www.error);
            yield break;
        }
        Debug.Log("������ �������" + www.text);
        if (www.text == "1")
            GameObject.Find("success_login").GetComponent<Text>().text = "�����";
        else
            GameObject.Find("success_login").GetComponent<Text>().text = "������";
    }
}
