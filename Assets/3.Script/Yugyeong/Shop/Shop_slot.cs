using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class Shop_slot : MonoBehaviour
{
    [SerializeField] Goods goods;
    [SerializeField] bool is_purchase;
    [SerializeField] Text purchase_text;

    [Header("Cache")]
    private IStoreController storeController; //���� ������ �����ϴ� �Լ� ������
    private IExtensionProvider storeExtensionProvider; //���� �÷����� ���� Ȯ�� ó�� ������

    private void OnEnable()
    {
        
    }

    private void UI_update()
    {
        if (goods.is_purchase)
        {
            purchase_btn.interactable = false;
            purchase_text.text = "���� �Ϸ�";
        }
        else
        {
            purchase_btn.interactable = true;
            purchase_text.text = "���� ����";
        }
    }
    public void Buy_goods() //btn���
    {

    }
}

public class Shop_btn : MonoBehaviour
{

}
