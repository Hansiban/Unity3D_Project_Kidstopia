using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Goods", menuName = "YG/Goods", order = 0)]
public class Goods : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] public int price;
    [SerializeField] private GameObject example;
    [SerializeField]
    public bool is_purchase
    {
        get 
        {
           bool tmp = char_purchase == 'T' ? true : false;
           return tmp;
        }
    }
    [SerializeField] private char char_purchase;
        /*
    {
         get
        {
            //SQL���� ������ char�� �޾ƿ���
        }0
         

    }
        */
}
