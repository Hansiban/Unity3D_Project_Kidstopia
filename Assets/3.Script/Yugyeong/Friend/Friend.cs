using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Friend_slot[] slots;
    private Friend_data data;

    //ģ�� ���� �ҷ�����
    private void OnEnable()
    {
        data = SQLManager.instance.Friend(SQLManager.instance.info.User_Id);
    }
    //���󰡱�
    //�¶��� �������� ����ϱ�
    //ģ�� �̸� ����
    //ģ�� �����ϱ�
}
