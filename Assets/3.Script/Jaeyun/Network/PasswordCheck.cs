using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordCheck : MonoBehaviour
{
    public InputField pwInputField;
    private void Update()
    {
        if (pwInputField.isFocused)
        { // pw inputfield�� ��Ŀ�� �Ǿ����� ��
            Input.imeCompositionMode = IMECompositionMode.Off; // �ѱ� ��Ȱ��ȭ
        }
        else
        {
            Input.imeCompositionMode = IMECompositionMode.Auto;
        }
    }
}
