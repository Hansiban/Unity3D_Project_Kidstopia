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
        data = SQLManager.instance.Friend();
    }
    //���󰡱�
    //�¶��� �������� ����ϱ�
    //ģ�� �̸� ����
    //ģ�� �����ϱ�
}
