using LitJson;
/* SQL namespace */
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class User_info
{ // Database table info
    public string User_Id { get; private set; }
    public string User_Pw { get; private set; }
    public string User_NickName { get; set; }
    public char FirstConnect { get; set; }
    public char Connecting { get; set; }

    public User_info(string userId, string password, string nickname, char firstConnect, char conn)
    {
        User_Id = userId;
        User_Pw = password;
        User_NickName = nickname;
        FirstConnect = firstConnect;
        Connecting = conn;
    }
}

public class User_Character
{
    public string User_Id { get; private set; }
    public int User_Eyes { get; set; }
    public int User_Jummper { get; set; }
    public int User_Runners { get; set; }
    public int User_TShirt { get; set; }
    public int User_Trunk { get; set; }
    public int User_Skin { get; set; }
    public int User_Hat { get; set; }
    public int User_HairStyle { get; set; }
    public int User_Ride { get; set; }
    public char User_Riding { get; set; }
    public User_Character(string userId, int eyes, int jummper, int runners, int tshirt, int trunk, int skin, int hat, int hair, int ride, char riding)
    {
        User_Id = userId;
        User_Eyes = eyes;
        User_Jummper = jummper;
        User_Runners = runners;
        User_TShirt = tshirt;
        User_Trunk = trunk;
        User_Skin = skin;
        User_Hat = hat;
        User_HairStyle = hair;
        User_Ride = ride;
        User_Riding = riding;
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
    public static SQLManager instance = null;

    public User_info info;
    public User_Character character_info;

    public MySqlConnection connection;
    public MySqlDataReader reader;

    private string dbPath = string.Empty;

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

        string serverInfo = ServerSet();
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

    private string ServerSet()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (dbPath.Equals(string.Empty))
            {
                dbPath = Application.persistentDataPath + "/Database"; // ��θ� string�� ����
            }
            if (!File.Exists(dbPath)) // �ش� ��ο� ������ ���ٸ�
            { // folder �˻�
                Directory.CreateDirectory(dbPath); // Directory ����
            }
            if (!File.Exists(dbPath + "/config.json"))
            { // file �˻�
                DefaultData(dbPath);
            }
        }
        else
        { // window
            if (dbPath.Equals(string.Empty))
            {
                dbPath = Application.dataPath + "/Database"; // ��θ� string�� ����
            }
            if (!File.Exists(dbPath)) // �ش� ��ο� ������ ���ٸ�
            { // folder �˻�
                Directory.CreateDirectory(dbPath); // Directory ����
            }
            if (!File.Exists(dbPath + "/config.json"))
            { // file �˻�
                DefaultData(dbPath);
            }
        }

        string jsonString = File.ReadAllText(dbPath + "/config.json"); // json file�� string���� �޾ƿ�
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
        items.Add(new ConfigItem("13.124.181.154", "kidstopia_player", "root", "1234", "3306")); // serverIP, tableName, id, pw, port
        JsonData data = JsonMapper.ToJson(items); // �ݵ�� �ڷᱸ���� �־�� ��
        File.WriteAllText(path + "/config.json", data.ToString());
    }

    public bool ConnectionCheck(MySqlConnection con)
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

    public void UpdateUserInfo(string column, char content, string userId)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }
            string selectCommand = string.Format(@"UPDATE user_info SET {0} = '{1}' WHERE id = '{2}';", column, content, userId);
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }

    public void UpdateData(string selectMenu, int select)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }
            string selectCommand = string.Format(@"UPDATE user_character_info SET {0} = '{1}' WHERE user_id = '{2}';", selectMenu, select, info.User_Id);
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }

    public void UpdateRiding(char riding, int select)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }
            string selectCommand = string.Format(@"UPDATE user_character_info SET Ride = {0}, Riding = {1} WHERE user_id = '{2}';", select, riding, info.User_Id); // @: �ٹٲ��� �־ ���ٷ� �ν��Ѵٴ� �ǹ�
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }

    public User_info PlayerInfo(string user_id)
    {
        try
        {
            if (ConnectionCheck(connection))
            {
                string selectCommand = string.Format(@"SELECT * FROM user_info A JOIN user_character_info B ON A.id = B.user_id WHERE A.id = '{0}';", user_id);
                MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) // reader ���� ������ 1�� �̻� �����ϴ���?
                {
                    // ���� �����͸� �ϳ��� ����
                    while (reader.Read())
                    {
                        // user_info
                        string id = (reader.IsDBNull(0)) ? string.Empty : reader["id"].ToString();
                        string pw = (reader.IsDBNull(0)) ? string.Empty : reader["pw"].ToString();
                        string nickName = reader["nickname"].ToString();
                        char firstConnect = reader["firstConnect"].ToString()[0];
                        char connecting = reader["connecting"].ToString()[0];
                        if (!id.Equals(string.Empty))
                        { // ���������� Data�� �ҷ��� ��Ȳ
                            User_info user_Info = new User_info(id, pw, nickName, firstConnect, connecting);
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            return user_Info;
                        }
                        else
                        { // �α��� ����
                            break;
                        }
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
    }

    public User_Character CharacterInfo(string user_id)
    {
        try
        {
            if (ConnectionCheck(connection))
            {
                string selectCommand = string.Format(@"SELECT * FROM user_info A JOIN user_character_info B ON A.id = B.user_id WHERE A.id = '{0}';", user_id); // @: �ٹٲ��� �־ ���ٷ� �ν��Ѵٴ� �ǹ�
                MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) // reader ���� ������ 1�� �̻� �����ϴ���?
                {
                    // ���� �����͸� �ϳ��� ����
                    while (reader.Read())
                    {
                        string id_c = reader["user_id"].ToString();
                        int eyes = reader["Eyes"].ToString()[0] - '0';
                        int jummper = reader["Jummper"].ToString()[0] - '0';
                        int runners = reader["Runners"].ToString()[0] - '0';
                        int tshirt = reader["TShirts"].ToString()[0] - '0';
                        int trunk = reader["Trunks"].ToString()[0] - '0';
                        int skin = reader["Skin"].ToString()[0] - '0';
                        int hat = reader["Hat"].ToString()[0] - '0';
                        int ride = reader["Riding"].ToString()[0] - '0';
                        int hair = reader["HairStyle"].ToString()[0] - '0';
                        char riding = reader["Ride"].ToString()[0];
                        if (!id_c.Equals(string.Empty))
                        { // ���������� Data�� �ҷ��� ��Ȳ
                            User_Character character_info = new User_Character(id_c, eyes, jummper, runners, tshirt, trunk, skin, hat, hair, ride, riding);
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            return character_info;
                        }
                        else
                        { // �α��� ����
                            break;
                        }
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
    }

    public bool SignIn(string user_id, string user_pw)
    { // �α���
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
            string selectCommand = string.Format(@"SELECT * FROM user_info A JOIN user_character_info B ON A.id = B.user_id WHERE A.id = '{0}' AND A.pw = '{1}';", user_id, user_pw); // @: �ٹٲ��� �־ ���ٷ� �ν��Ѵٴ� �ǹ�
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) // reader ���� ������ 1�� �̻� �����ϴ���?
            {
                // ���� �����͸� �ϳ��� ����
                while (reader.Read())
                {
                    // user_info
                    string id = (reader.IsDBNull(0)) ? string.Empty : reader["id"].ToString();
                    string pw = (reader.IsDBNull(0)) ? string.Empty : reader["pw"].ToString();
                    string nickName = reader["nickname"].ToString();
                    char firstConnect = reader["firstConnect"].ToString()[0];
                    char connecting = reader["connecting"].ToString()[0];
                    // user_character_info
                    string id_c = reader["user_id"].ToString();
                    int eyes = reader["Eyes"].ToString()[0] - '0';
                    Debug.Log(id_c);
                    int jummper = reader["Jummper"].ToString()[0] - '0';
                    int runners = reader["Runners"].ToString()[0] - '0';
                    int tshirt = reader["TShirts"].ToString()[0] - '0';
                    int trunk = reader["Trunks"].ToString()[0] - '0';
                    int skin = reader["Skin"].ToString()[0] - '0';
                    int hat = reader["Hat"].ToString()[0] - '0';
                    int ride = reader["Riding"].ToString()[0] - '0';
                    int hair = reader["HairStyle"].ToString()[0] - '0';
                    char riding = reader["Ride"].ToString()[0];
                    if (!id.Equals(string.Empty) || !pw.Equals(string.Empty) || !nickName.Equals(string.Empty))
                    { // ���������� Data�� �ҷ��� ��Ȳ
                        info = new User_info(id, pw, nickName, firstConnect, connecting);
                        character_info = new User_Character(id_c, eyes, jummper, runners, tshirt, trunk, skin, hat, hair, ride, riding);
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

    public bool SignUp(string user_id, string user_pw, string user_nick)
    { // ȸ������
        try
        {
            if (!ConnectionCheck(connection))
            {
                return false;
            }
            string sqlCommand = string.Format(@"SELECT id, nickname FROM user_info WHERE id = '{0}' OR nickname = '{1}';", user_id, user_nick);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) // reader ���� ������ 1�� �̻� �����ϴ���?
            { // ȸ������ ����
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                return false;
            }
            else
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                string insertCommand = string.Format(@"INSERT INTO user_info (id, pw, nickname) VALUES ('{0}', '{1}', '{2}');
                                                       INSERT INTO user_character_info (user_id) VALUES ('{0}');", user_id, user_pw, user_nick);
                MySqlCommand insertCmd = new MySqlCommand(insertCommand, connection);
                reader = insertCmd.ExecuteReader();
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                return true;
            }
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