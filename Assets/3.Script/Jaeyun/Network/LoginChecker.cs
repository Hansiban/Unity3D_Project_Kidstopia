using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Text; // encoding
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInItem
{ // Database login.json
    public string ID;
    public string PW;
    public string NickName;

    public SignInItem(string id, string pw, string nickName)
    {
        ID = id;
        PW = pw;
        NickName = nickName;
    }
}

public class LoginChecker : MonoBehaviour
{
    [SerializeField] private GameObject signInPanel;
    [SerializeField] private GameObject signUpPanel;
    [SerializeField] private GameObject deleteaccountPannel;
    [SerializeField] private GameObject checkButton;
    [SerializeField] private TMP_Text log;

    [SerializeField] private GameObject playerLogButton;
    [SerializeField] private TMP_Text id_userInfo;
    [SerializeField] private TMP_Text nickname_userInfo;

    [Header("Sign In")]
    public InputField idInput_in;
    public InputField pwInput_in;
    [Header("Sign Up")]
    public InputField idInput_up;
    public InputField pwInput_up;
    public InputField nickName_up;
    [Header("Delete")]
    public InputField idInput_delete;
    public InputField pwInput_delete;
    public InputField nickName_delete;

    private bool isLogin = false;
    private string dbPath = string.Empty;

    private void Start()
    {
        InfoButtonActive();
    }

    public void InfoButtonActive()
    { // �÷��̾� �α��� �� ���� ���� ��ư Ȱ��ȭ �� ���̵� �г��� ����
        if (Application.platform == RuntimePlatform.Android)
        {
            dbPath = Application.persistentDataPath + "/Database"; // ��θ� string�� ����
            if (!File.Exists(dbPath + "/UserInfo.json"))
            { // file �˻�
                isLogin = false;
                return;
            }
            else
            {
                playerLogButton.SetActive(true);
                isLogin = true;
            }
        }
        else
        { // window
            dbPath = Application.dataPath + "/Database"; // ��θ� string�� ����
            if (!File.Exists(dbPath + "/UserInfo.json"))
            { // file �˻�
                isLogin = false;
                return;
            }
            else
            {
                playerLogButton.SetActive(true);
                isLogin = true;
            }
        }

        // user info text
        string jsonString = File.ReadAllText(dbPath + "/UserInfo.json", Encoding.UTF8);
        JsonData ItemData = JsonMapper.ToObject(jsonString);
        id_userInfo.text = $"{ItemData[0]["ID"]}";
        nickname_userInfo.text = $"{ItemData[0]["NickName"]}";
    }

    private void DefaultData(string path, User_info info)
    {
        List<SignInItem> items = new List<SignInItem>();
        items.Add(new SignInItem($"{info.User_Id}", $"{info.User_Pw}", $"{info.User_NickName}")); // id, pw, nickName
        JsonData data = JsonMapper.ToJson(items);
        File.WriteAllText(path + "/UserInfo.json", data.ToString(), Encoding.UTF8);
    }

    public void SignInButton()
    { // �α��� ��ư
        if (idInput_in.text.Equals(string.Empty) || pwInput_in.text.Equals(string.Empty))
        {
            checkButton.SetActive(true);
            log.text = "��ĭ�� �Է��� �ּ���.";
            return;
        }
        if (SQLManager.instance.SignIn(idInput_in.text, pwInput_in.text))
        {
            // �α��� ����
            User_info info = SQLManager.instance.info;
            SQLManager.instance.isLogin = true;
            DefaultData(dbPath, info);
            // ó�� ������ �ƴ� ��� CreateScene�� �ƴ϶� Start�� �Ѿ
            if (info.FirstConnect.Equals('F'))
            {
                if (info.Connecting.Equals('F'))
                {
                    SQLManager.instance.UpdateUserInfo("connecting", 'T', info.User_Id);
                    ServerChecker.instance.StartClient();
                }
                else
                { // ���� ���� ���
                    checkButton.SetActive(true);
                    log.text = "�ٸ� ��⿡�� ���� ���Դϴ�.";
                    return;
                }
            }
            else
            { // ó�� ������ ���
                SceneManager.LoadScene("CreateScene");
            }
        }
        else
        {
            // �α��� ����
            checkButton.SetActive(true);
            log.text = "���̵�� ��й�ȣ�� Ȯ���� �ּ���.";
            return;
        }
    }

    public void SignUpButton()
    { // ȸ������ �Ϸ� ��ư
        if (idInput_up.text.Equals(string.Empty) || pwInput_up.text.Equals(string.Empty) || nickName_up.text.Equals(string.Empty))
        {
            checkButton.SetActive(true);
            log.text = "��ĭ�� �Է��� �ּ���.";
            return;
        }
        if (SQLManager.instance.SignUp(idInput_up.text, pwInput_up.text, nickName_up.text))
        {
            // ȸ������ ����
            signUpPanel.SetActive(false);
        }
        else
        {
            // ȸ������ ����
            checkButton.SetActive(true);
            log.text = "�ߺ��� ���̵� �ֽ��ϴ�.";
            return;
        }
    }

    public void SignOutButton()
    { // �α׾ƿ� ��ư
        if (Application.platform == RuntimePlatform.Android)
        {
            dbPath = Application.persistentDataPath + "/Database";
        }
        else
        { // window
            dbPath = Application.dataPath + "/Database";
        }

        File.Delete(dbPath + "/UserInfo.json");
        isLogin = false;
    }

    public void StartButton()
    { // �̹� �α��� �Ǿ��ִ� ���
        if (isLogin)
        {
            string jsonString = File.ReadAllText(dbPath + "/UserInfo.json", Encoding.UTF8);
            JsonData ItemData = JsonMapper.ToObject(jsonString);
            if (SQLManager.instance.SignIn(ItemData[0]["ID"].ToString(), ItemData[0]["PW"].ToString()))
            {
                User_info info = SQLManager.instance.info;
                SQLManager.instance.isLogin = true;
                SQLManager.instance.UpdateUserInfo("connecting", 'T', info.User_Id);
                ServerChecker.instance.StartClient();
            }
        }
        else
        {
            signInPanel.SetActive(true);
        }
    }

    public void DeleteAccount()
    {
        if (idInput_delete.text.Equals(string.Empty) || pwInput_delete.text.Equals(string.Empty) || nickName_delete.text.Equals(string.Empty))
        {
            checkButton.SetActive(true);
            log.text = "��ĭ�� �Է��� �ּ���.";
            return;
        }

        if (SQLManager.instance.SignOut(idInput_delete.text, pwInput_delete.text))
        {
            // ȸ��Ż�� ����
            deleteaccountPannel.SetActive(false);
        }
        else
        {
            // ȸ��Ż�� ����
            checkButton.SetActive(true);
            log.text = "�ش��ϴ� ���̵� �����ϴ�.";
            return;
        }
    }
}
