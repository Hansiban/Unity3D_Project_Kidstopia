using System.Collections;
using UnityEngine;

public abstract class NPC_YG : MonoBehaviour
{
    [Header("NPC")]
    public Transform trans;
    public Animator ani;
    public Transform goal;
    public bool can_move;

    private Touch touch;
    private Vector3 touched_pos;
    private Vector3 mouse_pos;
    private LayerMask layer;

    virtual public void Awake()
    {
        //������Ʈ ��������
        TryGetComponent(out ani);
        TryGetComponent(out trans);

        //��������Ʈ ����ϱ�
        TalkManager.event_talkend += Turn_canmove;
    }

    private void Start()
    {
        StartCoroutine(Find_posttion());
        StartCoroutine(Set_position());
    }

    private void Update()
    {
        Input_touch();
    }
    private void Turn_canmove()
    {
        can_move = !can_move;
    }

    virtual public IEnumerator Find_posttion()
    {
        yield return null;
    }

    virtual public IEnumerator Set_position()
    {
        yield return null;
    }

    private void Input_touch()
    {
        Debug.Log("Input_touch");

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touched_pos = Camera.main.ScreenToWorldPoint(touch.position);
                    Try_raycast(touch.position);
                }
            }
        }

        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Input.GetMouseButtonDown(0)");
                mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Try_raycast(Input.mousePosition);
            }
        }
    }

    private void Try_raycast(Vector3 pos)
    {
        //Debug.Log("Try_raycast");
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            Debug.Log("���� ��");
            if (hit.collider.CompareTag("NPC"))
            {
                Interactive_NPC();
            }
        }
    }

    private void Interactive_NPC()
    {
        Debug.Log("NPCã��");
        TalkManager.instance.Print();
    }

}
