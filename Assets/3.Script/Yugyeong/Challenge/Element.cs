using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Element : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI nametext;
    [SerializeField] TextMeshProUGUI rewardtext;
    [SerializeField] TextMeshProUGUI progresstext; //0/0
    [SerializeField] Slider slider;
    [SerializeField] Button button;

    [Header("Data")]
    public Challenge_YG challange_data;//��������

    public void UI_update()
    {
        //�̸� ������Ʈ
        nametext.text = challange_data.info;
        rewardtext.text = $"{challange_data.reward_count}";

        if (challange_data.clear_count <= challange_data.cur_count) //�Ϸ�
        {
            button.gameObject.SetActive(true);
            slider.gameObject.SetActive(false);
            progresstext.gameObject.SetActive(false);
        }

        else //�Ϸ�X
        {
            button.gameObject.SetActive(false);
            slider.gameObject.SetActive(true);
            progresstext.gameObject.SetActive(true);
            progresstext.text = $"{challange_data.clear_count} / {challange_data.cur_count}";

            //�����̴� �� ����
            slider.maxValue = challange_data.clear_count;
            slider.value = challange_data.cur_count;
        }
    }

    public void Btn()
    {
        transform.GetComponentInParent<Challenge>().Get_reward(challange_data.reward_count);
        button.interactable = false;
    }
}
