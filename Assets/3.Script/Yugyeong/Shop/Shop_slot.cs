using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop_slot : MonoBehaviour
{
    [SerializeField] Goods goods;
    [SerializeField] Button btn;
    [SerializeField] bool is_purchase;
    [SerializeField] Text purchase_text;

    private void UI_update()
    {
        if (goods.is_purchase)
        {
            purchase_text.text = "���� �Ϸ�";
            btn.interactable = false;
        }
        else
        {
            purchase_text.text = "�����ϱ�";
            btn.interactable = true;
        }
    }
}
