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
        //���ݿ�
        string connStr = "Database=net;Data Source=127.0.0.1;";
        connStr += "User Id=root;Password=root;port=3306";
        sqlConn = new MySqlConnection(connStr);
        try
        {
            Debug.Log("[���ݿ�]���ӳɹ�");
            sqlConn.Open();
        }
        catch (Exception e)
        {
            Debug.Log("[���ݿ�]����ʧ��" + e.Message);
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
                GameObject.Find("LoginScene").GetComponent<LoginScene>().ShowTip("��¼ʧ��");
            }
        }
        catch (Exception e)
        {
            reader.Close();
            print(e.ToString());
            GameObject.Find("LoginScene").GetComponent<LoginScene>().ShowTip("��¼ʧ��");
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
            Debug.Log("����ɹ�");
            GameObject.Find("LoginScene").GetComponent<LoginScene>().ShowTip("ע��ɹ�");

        }
        catch
        {
            Debug.Log("����ʧ��");
            GameObject.Find("LoginScene").GetComponent<LoginScene>().ShowTip("ע��ʧ��");
        }
    }
}