using UnityEngine;

public class Challenge : MonoBehaviour
{
    [SerializeField] Element[] elements;

    private void OnEnable()
    {
        elements = transform.GetComponentsInChildren<Element>();
        foreach (Element element in elements)
        {
            element.UI_update();
        }
    }

    public void Get_reward(int reward)
    {
        //����ݾ�
        Debug.Log($"����� : {reward}");

        //���� �ݾ�
        int cur_money = SQLManager.instance.Item(SQLManager.instance.info.User_Id).money;
        Debug.Log($"���� �ݾ� : {cur_money}");

        //���� �ݾ� = ����ݾ�+ �����ݾ�
        SQLManager.instance.Updateitem("money", reward + cur_money);
        Debug.Log($"����� ���� �� �����ݾ� : {SQLManager.instance.Item(SQLManager.instance.info.User_Id).money}");
        Debug.Log($"����� ���� �� �����ݾ�2 : {SQLManager.instance.item.money}");
    }
}
