using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public enum Shopname
{
    hair = 0,
    riding,
    clothes,
    acc
}

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance = null;
    [SerializeField] private Text name_text; //���� �̸�
    [SerializeField] private Shopname shopname;
    [SerializeField] private GameObject shop_pannel;
    [SerializeField] private GameObject[] shop_obj;
    [SerializeField] private Shop_slot[] shop_Slots;

    [SerializeField] private GameObject inapp_obj;//�ξ۰���
    [SerializeField] private GameObject can_inapp;
    [SerializeField] private Text inapp_text;

    [SerializeField] private GameObject buy_obj;
    [SerializeField] private Text buy_text;

    [SerializeField] private Text money_text;
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
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        shop_pannel.SetActive(false);

        Test();
    }

    private void Test() //�»߰� �� ����
    {
        shopname = Shopname.hair;
        Set_shop(shopname);
    }

    private void Set_shop(Shopname shopname)
    {
        //shop_pannel �ѱ�
        shop_pannel.SetActive(true);

        //shop_obj �ѱ�
        for (int i = 0; i < shop_obj.Length; i++)
        {
            if (i == (int)shopname)
            {
                shop_obj[i].SetActive(true);
            }
            else
            {
                shop_obj[i].SetActive(false);
            }
        }

        //shop_slots�� �Ҵ�
        shop_Slots = FindObjectsOfType<Shop_slot>();

        //slot UI update
        for (int i = 0; i < shop_Slots.Length; i++)
        {
            shop_Slots[i].UI_update();
        }
    }

    public void Buy_item(int price)
    {

        if (price < money)
        {
            SQLManager.instance.Updateitem("money", money - price);
            // ��ư ������� Ǯ��(������ ���� �� õ���)
            buy_text.text = $"���ſ� �����߽��ϴ�.\n ������ : {money}";
        }
        else
        {
            CanPurchase();
        }
    }

    private IEnumerator text_setting(Text text, string str)
    {
        text.text = $"{str}";
        yield return new WaitForSeconds(3f);
        text.enabled = false;
    }
    

    #region btn
    public void CanPurchase()//��������
    {
        can_inapp.SetActive(true);
    }

    public void Can_Yes()
    {
        can_inapp.SetActive(false);
        inapp_obj.SetActive(true);
    }

    public void Can_No()
    {
        can_inapp.SetActive(false);
    }
    #endregion

    #region �ξ۰���
    public void Complete_purchase()
    {
        text_setting(inapp_text, "������ �����߽��ϴ�.\n���� ��� : {000}");
    }

    public void Failed_purchase()
    {
        text_setting(inapp_text, "������ ��ҵǾ����ϴ�.");
    }

    public void Buy_Gold(int num)
    {
        SQLManager.instance.Updateitem("money", money + num);
    }

    public void Update_moneytext()
    {
        money_text.text = $"��� : {money}";
    }
    #endregion
}
