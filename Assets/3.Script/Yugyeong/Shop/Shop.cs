using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Shopname
{
    hair = 0,
    riding,
    clothes,
    acc
}

public class Shop : MonoBehaviour
{
    [SerializeField] private Text name_text; //���� �̸�

    [SerializeField] private GameObject inapp_obj;//�ξ۰���
    [SerializeField] private Text inapp_text; 
    [SerializeField] private Text buy_text;

    [SerializeField] private Text money_text;
    [SerializeField] Shop_info shop_Info;
    private int money_;
    public int money
    {
        set
        {
            Update_moneytext();
        }
        get 
        {
            money_ = SQLManager.instance.Item().money;
            Update_moneytext();
            return money_;
        }
    }

    private void OnEnable()
    {
        name_text.text = shop_Info.name;
    }
    private void Buy_item(int price)
    {
        if (price < money)
        {
            //�ܾ׻���
            SQLManager.instance.Updateitem("money",money-price);
            // ��ư ������� Ǯ��(������ ���� �� õ���)
            //���ܾ� �ȳ�
            buy_text.text = $"���ſ� �����߽��ϴ�.\n �����ܾ� : {money}";
        }
        else
        {
            //��������
            
        }
    }

    #region �ξ۰���
    private void Complete_purchase()
    {
        inapp_text.text = "�ξ۰����� �����߽��ϴ�.";
    }

    private void Failed_purchase()
    {
        inapp_text.text = "������ ��ҵǾ����ϴ�.";
    }

    private void Buy_Gold(int num)
    {
        SQLManager.instance.Updateitem("money", money + num);
    }

    private void Update_moneytext()
    {
        money_text.text = $"{money}";
    }
    #endregion
}
