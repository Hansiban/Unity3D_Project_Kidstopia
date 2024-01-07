using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_slot : MonoBehaviour
{
    [SerializeField] Goods goods;
    [SerializeField] Button purchase_btn;
    [SerializeField] Text purchase_text;
    [SerializeField] GameObject fail;

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
