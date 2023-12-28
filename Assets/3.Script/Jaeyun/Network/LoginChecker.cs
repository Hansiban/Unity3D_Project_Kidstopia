using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* SQL namespace */
using MySql.Data;
using MySql.Data.MySqlClient;
using LitJson;
using System;
using System.IO;

public class User_info
{ // Database table info
    public string User_Id { get; private set; }
    public string User_Pw { get; private set; }
    public string User_Nickname { get; set; }

    public User_info(string userId, string password, string userName)
    { // ������ ȣ��� ���� ������
        User_Id = userId;
        User_Pw = password;
        User_Nickname = userName;
    }
}

public class LoginChecker : MonoBehaviour
{
    public User_info info;

    private SQLManager sqlManager;
    private MySqlConnection connection;
    private MySqlDataReader reader;

    private void Start()
    {
        TryGetComponent(out sqlManager);
    }

    public bool SignIn(string id, string password, string nick)
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
            if (!sqlManager.ConnectionCheck(connection))
            {
                return false;
            }
            string sqlCommand = string.Format(@"SELECT id, pw, nickname FROM user_info WHERE id = '{0}' AND pw = '{1}' AND nickname = '{2}';", id, password, nick); // @: �ٹٲ��� �־ ���ٷ� �ν��Ѵٴ� �ǹ�
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
                    if (!name.Equals(string.Empty) || !pw.Equals(string.Empty) || !nickName.Equals(string.Empty))
                    { // ���������� Data�� �ҷ��� ��Ȳ
                        info = new User_info(name, pw, nickName);
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

    public bool SignUp(string id, string password, string nick)
    { // ȸ������
        try
        {
            if (!sqlManager.ConnectionCheck(connection))
            {
                return false;
            }
            string sqlCommand = string.Format(@"SELECT id, pw, nickname FROM user_info WHERE id = '{0}' OR nickname = '{1}';", id, nick); // @: �ٹٲ��� �־ ���ٷ� �ν��Ѵٴ� �ǹ�
            MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) // reader ���� ������ 1�� �̻� �����ϴ���?
            { // ȸ������ ����
                return false;
            }
            else
            {
                // ���� �����͸� �ϳ��� ����
                while (reader.Read())
                {
                    string name = (reader.IsDBNull(0)) ? string.Empty : (string)reader["id"].ToString();
                    string pw = (reader.IsDBNull(0)) ? string.Empty : (string)reader["pw"].ToString();
                    string nickName = (reader.IsDBNull(0)) ? string.Empty : (string)reader["nickname"].ToString();
                    if (!name.Equals(string.Empty))
                    { // ���������� Data�� �ҷ��� ��Ȳ
                        info = new User_info(name, pw, nickName);
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                        return true;
                    }
                    else
                    { // ȸ������ ����
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
