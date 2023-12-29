using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginChecker : MonoBehaviour
{
    [SerializeField] private GameObject signUpPanel;
    [SerializeField] private GameObject checkButton;
    [SerializeField] private TMP_Text log;

    [Header("Sign In")]
    public InputField idInput_in;
    public InputField pwInput_in;
    [Header("Sign Up")]
    public InputField idInput_up;
    public InputField pwInput_up;
    public InputField nickName_up;

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
            Debug.Log(info.User_Id + " | " + info.User_Pw);
            // ó�� ������ �ƴ� ��� CreateScene�� �ƴ϶� Start�� �Ѿ
            if (info.FirstConnect.Equals('T'))
            {
                info.Connecting = 'T';
                SQLManager.instance.UpdateUserInfo("firstConnect", 'T', info.User_Id);
                ServerChecker.instance.StartClient();
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
}
