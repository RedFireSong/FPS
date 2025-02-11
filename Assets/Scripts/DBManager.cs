using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System;

public class DBManager : MonoBehaviour
{
    public static DBManager instance;
    MySqlConnection sqlConn;

    public int userID;
    public string userName;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        //数据库
        string connStr = "Database=net;Data Source=127.0.0.1;";
        connStr += "User Id=root;Password=root;port=3306";
        sqlConn = new MySqlConnection(connStr);
        try
        {
            Debug.Log("[数据库]链接成功");
            sqlConn.Open();
        }
        catch (Exception e)
        {
            Debug.Log("[数据库]链接失败" + e.Message);
            throw;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Login(int id, string pw)
    {
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand command = new MySqlCommand("select * from player where id=@id " +
                                                       "and pw=@pw", sqlConn);
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("pw", pw);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                userID = id;
                string name = reader.GetString("name");
                PlayerPrefs.SetString("name", name);
                GameObject.Find("LoginScene").GetComponent<LoginScene>().StartGame();
                reader.Close();
                return;
            }
            else
            {
                reader.Close();
                GameObject.Find("LoginScene").GetComponent<LoginScene>().ShowTip("登录失败");
            }
        }
        catch (Exception e)
        {
            reader.Close();
            print(e.ToString());
            GameObject.Find("LoginScene").GetComponent<LoginScene>().ShowTip("登录失败");
        }
    }

    public void Register(int id, string pw, string name)
    {
        MySqlCommand cmd = new MySqlCommand("insert into player set id=@id , pw = @pw , name = @name", sqlConn);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("pw", pw);
        cmd.Parameters.AddWithValue("name", name);
        try
        {
            cmd.ExecuteNonQuery();
            Debug.Log("插入成功");
            GameObject.Find("LoginScene").GetComponent<LoginScene>().ShowTip("注册成功");

        }
        catch
        {
            Debug.Log("插入失败");
            GameObject.Find("LoginScene").GetComponent<LoginScene>().ShowTip("注册失败");
        }
    }
}