using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Friend_slot : MonoBehaviour
{
    [SerializeField] Friend_data data;
    [SerializeField] int index;

    [SerializeField] GameObject friend_o;
    [SerializeField] GameObject friend_x;

    [SerializeField] private TMP_Text name;
    [SerializeField] private Text onoff_text;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites; //O,X
    [SerializeField] private Button chase_btn;
    [SerializeField] private GameObject friend_obj;
    [SerializeField] private GameObject my_obj;

    private void OnEnable()
    {
        data = SQLManager.instance.Friend(SQLManager.instance.info.User_Id);
        friend_x = transform.GetChild(0).gameObject;
        friend_o = transform.GetChild(1).gameObject;
        Setting();
    }

    private void Setting()
    {
        data = SQLManager.instance.Friend(SQLManager.instance.info.User_Id);

        if (data.friends[index] == string.Empty)
        {
            friend_x.SetActive(true);
            friend_o.SetActive(false);
        }

        else
        {
            friend_o.SetActive(true);
            friend_x.SetActive(false);
            O_setting();
        }
    }

    private void O_setting()
    {
        name.text = data.friends[index];

        if (SQLManager.instance.PlayerInfo(data.friends[index]).Connecting == 'T')
        {
            onoff_text.text = "�¶���";
            image.sprite = sprites[0];
            chase_btn.interactable = true;
        }

        else
        {
            onoff_text.text = "��������";
            image.sprite = sprites[1];
            chase_btn.interactable = false;
        }
    }

    public void DeleteFriend()
    {
        SQLManager.instance.DeleteFriend(index);
        Setting();
    }

    public void Chase(int index) //0116 merge�� �� ���󰡱� ��ư�� �ް� �ε��� 0,1,2 
    {
        //ģ�� �÷��̾�, ���� �÷��̾� ã��
        var players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PlayerCreate>().info.User_NickName == SQLManager.instance.info.User_NickName)
            {
                my_obj = players[i];
                //Debug.Log($"�� �г��� : {players[i].GetComponent<PlayerCreate>().info.User_NickName}");
            }

            //ģ�� �г��� = �� �����Ϳ� ����� �г���
            if (players[i].GetComponent<PlayerCreate>().info.User_NickName == SQLManager.instance.Friend(SQLManager.instance.info.User_Id).friends[index])
            {
                friend_obj = players[i];
                //Debug.Log($"ģ�� �г��� : {players[i].GetComponent<PlayerCreate>().info.User_NickName}");
            }
        }

        //��ġ �̵�
        my_obj.transform.position = friend_obj.transform.position + Vector3.up;

    }
}
