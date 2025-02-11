using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    public GameObject registerPanel, loginPanel;
    public GameObject tip;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Register()
    {
        int id = int.Parse(registerPanel.transform.GetChild(0).GetComponentInChildren<InputField>().text);
        string pw = registerPanel.transform.GetChild(1).GetComponentInChildren<InputField>().text;
        string pw2 = registerPanel.transform.GetChild(2).GetComponentInChildren<InputField>().text;

        print(pw);
        print(pw);
        if (pw != pw2)
        {
            ShowTip("两次密码不一样，注册失败");
            return;
        }
        else
        {
            DBManager.instance.Register(id, pw, id.ToString());
        }
    }

    public void Login()
    {
        if (loginPanel.transform.GetChild(0).GetComponentInChildren<InputField>().text != "")
        {
            int id = int.Parse(loginPanel.transform.GetChild(0).GetComponentInChildren<InputField>().text);
            string pw = loginPanel.transform.GetChild(1).GetComponentInChildren<InputField>().text;
            DBManager.instance.Login(id, pw);
        }
 
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowTip(string str)
    {
        tip.SetActive(true);
        tip.GetComponentInChildren<Text>().text = str;
        Invoke("HideTip", 1);
    }

    public void HideTip()
    {
        tip.SetActive(false);
    }
}
