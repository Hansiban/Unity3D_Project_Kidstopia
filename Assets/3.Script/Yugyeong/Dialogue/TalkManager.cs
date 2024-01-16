using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TalkManager : MonoBehaviour
{
    static public TalkManager instance = null;

    [Header("Talk Panel")]
    [SerializeField] private GameObject staticMenu;
    public GameObject chatPanel;
    [SerializeField] TextMeshProUGUI dialog_text;
    public GameObject talk_pannel;
    [SerializeField] TextMeshProUGUI name_text;
    private int dialog_index;

    [Header("Button in Talk Panel")]
    [SerializeField] private GameObject yes_button;
    [SerializeField] private TextMeshProUGUI yes_button_text;
    [SerializeField] private TextMeshProUGUI no_button_text;
    [SerializeField] private GameObject micButton_panel; // click ����
    [SerializeField] private GameObject micButton; // audio button

    public delegate void del_talkend();
    public static event del_talkend event_talkend;

    [SerializeField] private Touch touch;
    [SerializeField] private LayerMask layer;

    [Header("data")]
    List<Dictionary<string, object>> data_Dialog;

    [Header("NPC")]
    public NPCInfoSetting npcInfoSet; // Ŭ���� NPC ������� ���� ����, ���� 24. 01. 05
    public Transform npc;
    public GameObject mainCamera; // player�� ���ߴ� ī�޶�
    public GameObject npcCamera; // npc�� ��ȭ�� ���� ī�޶�
    [SerializeField] private Transform goppiTrans;
    [SerializeField] private NPCInfoSetting goppiInfo; // npc goppi
    [SerializeField] private NPCCameraController goppiCamera;
    [SerializeField] private NPC_chaseplayer goppiMovescript;

    [SerializeField] private ChatGPT chatGPT; // npc�� ���� ��û�ϴ� response
    public string responseText = string.Empty; // gpt ��� text
    public bool isGuide = false;

    public GameObject playerModel = null;

    // user touch position
    private Vector3 touched_pos;
    private Vector3 mouse_pos;

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
            return;
        }
    }
    private void Start()
    {
        data_Dialog = CSVReader.Read("NpcDialog");
        Close_dialog();
    }

    private void Update()
    {
        Input_touch();
    }

    #region Dialog Pannel Active
    public void Open_dialog()
    {
        if (playerModel != null)
        {
            playerModel.SetActive(false);
        }
        talk_pannel.SetActive(true);
        staticMenu.SetActive(false);
        chatPanel.SetActive(false);

        micButton_panel.SetActive(false); // �� ó�� talkpanel�� ������ ���� micbutton ����

        // camera switch
        npcCamera.SetActive(true);
        mainCamera.SetActive(false);

        // npc info setting, button text
        no_button_text.text = $"{npcInfoSet.npcInfo.noButtonText}";
        yes_button_text.text = $"{npcInfoSet.npcInfo.yesButtonText}";
        isGuide = true;

        if (event_talkend != null)
        {
            event_talkend();
        }
    }

    public void NPC_GoppiButton()
    {
        // npc info setting
        npcInfoSet = goppiInfo;
        goppiCamera.npc = goppiTrans;
        DialogText_Print();
    }

    public void Close_dialog()
    { // No_Button Click method
        if (playerModel != null)
        {
            playerModel.SetActive(true);
        }
        talk_pannel.SetActive(false);
        staticMenu.SetActive(true);
        goppiMovescript.Turn_canmove();

        // camera switch, player setting
        npcCamera.SetActive(false);
        if (mainCamera != null)
        {
            mainCamera.SetActive(true);
        }
        if (chatPanel != null)
        {
            chatPanel.SetActive(true);
        }

        if (event_talkend != null)
        {
            event_talkend();
        }

        // ��ư Ȱ��ȭ �ʱ�ȭ
        yes_button.SetActive(false);
        micButton_panel.SetActive(false);
        micButton.SetActive(false);

        //����
        //����϶� ������ ���� �����ϱ�
        if (npcInfoSet == goppiInfo)
        {
            goppiMovescript.Turn_canmove();
        }
    }
    #endregion

    public void DialogText_Print()
    { // dialog pannel 
        if (!talk_pannel.activeSelf)
        {
            Open_dialog();
        }

        // npc info�� ���� text ���
        for (int i = 0; i < data_Dialog.Count; i++)
        {
            if (data_Dialog[i]["Character_name"].ToString().Trim().Equals(npcInfoSet.npcInfo.npcName.Trim()))
            {
                dialog_index = i;
                break;
            }
        }
        name_text.text = data_Dialog[dialog_index]["Character_name"].ToString();
        string[] array = data_Dialog[dialog_index]["Dialog"].ToString().Split(']');
        dialog_text.text = array[Random.Range(0, array.Length)];
    }

    #region NPC ����
    private void Input_touch()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    if (!talk_pannel.activeSelf)
                    {
                        touched_pos = Camera.main.ScreenToWorldPoint(touch.position);
                        Try_raycast(touch.position);
                    }
                }
            }
        }

        else
        { // Window
            if (Input.GetMouseButtonUp(0))
            {
                if (!talk_pannel.activeSelf)
                {
                    mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Try_raycast(Input.mousePosition);
                }
            }
        }
    }
    #region Dialog Panel Text Print
    public void InputNextText()
    { // panel click method
        if (isGuide)
        { // npc info text
            dialog_text.text = $"<#ABABAB>{npcInfoSet.npcInfo.npcText}</color>";
            yes_button.SetActive(npcInfoSet.npcInfo.npcYesButton); // npc�� ���� talk panel button ����
            micButton_panel.SetActive(npcInfoSet.npcInfo.micButton);
            micButton.SetActive(npcInfoSet.npcInfo.micButton); // audio button�� ����ϴ� ��
        }
        else
        {
            if (!npcInfoSet.npcInfo.npcYesButton)
            {
                name_text.text = data_Dialog[dialog_index]["Character_name"].ToString();
                dialog_text.text = responseText;
                micButton_panel.SetActive(true);
                StartCoroutine(MicButtonActive_Co());
            }
        }
    }

    private IEnumerator MicButtonActive_Co()
    {
        yield return new WaitForSeconds(1.5f);
        micButton.SetActive(true);
    }

    public void PlayerRequestText(string request)
    { // mic button click method
        // 1. name.text�� player nickname, dialog_text.text�� response text
        // 2. response gpt�� ��û �� ���
        // 3. ��� �� panal click �� micButton active
        name_text.text = SQLManager.instance.info.User_NickName;
        dialog_text.text = request;
        micButton_panel.SetActive(false);
        micButton.SetActive(false);
        isGuide = false;
        chatGPT.NpcResponse(request);
    }
    #endregion

    private void Try_raycast(Vector3 pos)
    { // NPC ã�� raycast
        if (!mainCamera.activeSelf) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
        {
            if (hit.collider.CompareTag("NPC"))
            { // npc�� ��
                npc = hit.collider.gameObject.transform;
                npcInfoSet = hit.collider.gameObject.GetComponent<NPCInfoSetting>();
                QuizManager.instance.npcInfoSet = npcInfoSet; // npc info Quiz manager���� ����
                if (Vector3.Distance(playerModel.transform.position, hit.collider.transform.position) < 5f)
                {
                    DialogText_Print();
                }
            }
            else if (hit.collider.CompareTag("Animal"))
            {
                StudyManager.instance.animal_data = hit.collider.gameObject.GetComponent<Nonplayer_YG>().data;
                if (SQLManager.instance.Collection(SQLManager.instance.info.User_Id, StudyManager.instance.animal_data.table_name).is_open == 'T')
                {
                    if (Vector3.Distance(playerModel.transform.position, hit.collider.transform.position) < 5f)
                    {
                        StudyManager.instance.Interactive_Nonplayer();
                    }
                }
                else
                {
                    Debug.Log("������ �������ʾ� ������ ���X");
                }
            }
        }
    }

    public void YesButton()
    {
        if (npcInfoSet.npcInfo.npcSetting.Equals(NPCSetting.Animal))
        { // Quiz
            Close_dialog();
            QuizManager.instance.quizCanvas.SetActive(true);
            QuizManager.instance.mainMenu.SetActive(true);
            QuizManager.instance.AnimalsQuiz_Print();
        }
        else if (npcInfoSet.npcInfo.npcSetting.Equals(NPCSetting.Shop))
        { // Shop
            Close_dialog();
            // ���߿� �������� ���� �߰����ָ��,,, todo
            // Shop UI Active ���ֱ�
            ShopManager.instance.Set_shop(npcInfoSet.npcInfo.shopInfo);
        }
    }
    #endregion
}

[System.Serializable]
public struct TalkData
{
    public string name;
    public string[] contexts;
}
