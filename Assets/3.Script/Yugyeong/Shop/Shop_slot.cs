using UnityEngine;
using UnityEngine.UI;


public class Shop_slot : MonoBehaviour
{
    [SerializeField] Goods goods;
    [SerializeField] Button btn;
    [SerializeField] Text purchase_text;

    private void OnEnable()
    {
        btn = GetComponent<Button>();
        if (btn.onClick.GetPersistentEventCount() == 0)
        {
            btn.onClick.AddListener(Btn);
        }
        purchase_text = GetComponentInChildren<Text>();
        UI_update();
    }

    public void UI_update()
    {
        if (goods.is_purchase == 'T')
        {
            purchase_text.text = "���� �Ϸ�";
            btn.interactable = false;
        }
        else
        {
            purchase_text.text = $"�����ϱ�\n({goods.price}��)";
            btn.interactable = true;
        }
    }

    public void Btn()
    {
        Debug.Log(ShopManager.instance.cur_slot);
        ShopManager.instance.cur_slot = this;
        ShopManager.instance.Buy_item(goods);
    }
}
