using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* SQL namespace */
using MySql.Data;
using MySql.Data.MySqlClient;
using LitJson;
using System;
using System.IO;

public class user_info
{ // Database table info
    public string id { get; private set; }
    public string pw { get; private set; }
    public string nickName { get; private set; }

    public user_info(string userId, string password, string userName)
    { // ������ ȣ��� ���� ������
        id = userId;
        pw = password;
        nickName = userName;
    }
}

public class ConfigItem
{ // Database config.json
    public string IP;
    public string TableName;
    public string ID;
    public string PW;
    public string PORT;

    public ConfigItem(string ipValue, string tableName, string userId, string userPw, string port)
    {
        IP = ipValue;
        this.TableName = tableName;
        this.ID = userId;
        this.PW = userPw;
        this.PORT = port;
    }
}

public class SQLManager : MonoBehaviour
{
    public user_info info;

    public MySqlConnection connection;
    public MySqlDataReader reader;

    private string dbPath = string.Empty;

    public static SQLManager instance = null;

    [Header("Input My IP")]
    public string serverIp = string.Empty;
    public string tableName = string.Empty;

    public bool isLogin = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // Application.dataPath�� Asset ���������� ����
        dbPath = Application.dataPath + "/Database"; // ��θ� string�� ����

        string serverInfo = ServerSet(dbPath);
        try
        {
            if (serverInfo.Equals(string.Empty))
            {
                Debug.Log("SQL Server Json Error");
                return;
            }
            connection = new MySqlConnection(serverInfo); // Connection create
            connection.Open(); // Connection open
            Debug.Log("SQL Server Open Complete");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private string ServerSet(string path)
    {
        if (!File.Exists(dbPath)) // �ش� ��ο� ������ ���ٸ�
        { // folder �˻�
            Directory.CreateDirectory(dbPath); // Directory ����
        }
        if (!File.Exists(dbPath + "/config.json"))
        { // file �˻�
            DefaultData(dbPath);
        }

        string jsonString = File.ReadAllText(path + "/config.json"); // json file�� string���� �޾ƿ�
        JsonData ItemData = JsonMapper.ToObject(jsonString); // string ���¸� json ���·� �ٲ���

        if (serverIp == string.Empty)
        {
            serverIp = $"{ItemData[0]["IP"]}";
        }
        if (tableName == string.Empty)
        {
            tableName = $"{ItemData[0]["TableName"]}";
        }

        string serverInfo = $"Server = {serverIp}; Database = {tableName}; Uid = {ItemData[0]["ID"]}; Pwd = {ItemData[0]["PW"]}; Port = {ItemData[0]["PORT"]}; CharSet = utf8;";
        return serverInfo;
    }

    private void DefaultData(string path)
    {
        List<ConfigItem> items = new List<ConfigItem>();
        items.Add(new ConfigItem("127.0.0.1", "kidstopia_player", "root", "1234", "3306")); // serverIP, tableName, id, pw, port
        JsonData data = JsonMapper.ToJson(items); // �ݵ�� �ڷᱸ���� �־�� ��
        File.WriteAllText(path + "/config.json", data.ToString());
    }

    private bool ConnectionCheck(MySqlConnection con)
    {
        // ���� MySqlConnection�� Open ���°� �ƴ϶��
        if (con.State != System.Data.ConnectionState.Open)
        {
            con.Open();
            if (con.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
        }
        return true;
    }

    public bool Login(string id, string password)
    {
        // ���������� DB���� �����͸� ������ ���� �޼ҵ�
        // ��ȸ�Ǵ� �����Ͱ� ���ٸ� false, ��ȸ�� �Ǵ� �����Ͱ� �ִٸ� true
        // ������ ������ info�� ���� ������ ������
        /*
            Data �������
            1. Connection Open���� Ȯ��
            2. Reader ���°� �а� �ִ� ��Ȳ���� Ȯ��(1quary 1reader)
            3. Data�� �� �о����� Reader�� ���¸� Ȯ�� �� Close
         */
        try
        {
            if (!ConnectionCheck(connection))
            {
                return false;
            }
            string sqlCommand = string.Format(@"SELECT User_name, User_Password FROM user_info WHERE User_Name = '{0}' AND User_Password = '{1}';", id, password); // @: �ٹٲ��� �־ ���ٷ� �ν��Ѵٴ� �ǹ�
            MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) // reader ���� ������ 1�� �̻� �����ϴ���?
            {
                // ���� �����͸� �ϳ��� ����
                while (reader.Read())
                {
                    string name = (reader.IsDBNull(0)) ? string.Empty : (string)reader["id"].ToString();
                    string pw = (reader.IsDBNull(0)) ? string.Empty : (string)reader["pw"].ToString();
                    string nickName = (reader.IsDBNull(0)) ? string.Empty : (string)reader["nickname"].ToString();
                    if (!name.Equals(string.Empty) || !pw.Equals(string.Empty))
                    { // ���������� Data�� �ҷ��� ��Ȳ
                        info = new user_info(name, pw, nickName);
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                        return true;
                    }
                    else
                    { // �α��� ����
                        break;
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return false;
        }
    }
}