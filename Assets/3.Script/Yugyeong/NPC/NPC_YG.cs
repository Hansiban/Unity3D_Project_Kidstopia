using System.Collections;
using UnityEngine;

public class Nonplayer_YG : MonoBehaviour
{
    // Nonplayer : �������� �����̰� �ϱ�

    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Transform trans;
    [SerializeField] private Animation ani;

    [SerializeField] private float pos_speed = 1;

    [SerializeField] private float max_force = 5;
    [SerializeField] private float min_force = 0;

    [SerializeField] private float max_time = 2;
    [SerializeField] private float min_time = 1;


    private void Awake()
    {
        //������Ʈ ��������
        TryGetComponent(out rigid);
        TryGetComponent(out trans);
        TryGetComponent(out ani);
    }

    private void Start()
    {
        StartCoroutine(Random_Movement());
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = trans.forward;
        rigid.velocity = Vector3.forward * pos_speed;
        Debug.Log($"rigid.velocity: {rigid.velocity} / pos_speed : {pos_speed}");
    }

    IEnumerator Random_Movement()
    {
        while (true)
        {
            //pos_speed = 1f; //ó���� ������ �ֱ�
            float rot_angle = Random.Range(0, 360); //0~360������ ���� ����
            float wait_num = Random.Range(min_time, max_time);
            Debug.Log("rot_angle :" + rot_angle + "/ wait_num : " + wait_num);
            trans.Rotate(new Vector3(0, rot_angle, 0));

            yield return new WaitForSeconds(1f);
            Debug.Log("1�� ��ٸ� �Ϸ�");

            pos_speed = Random.Range(min_force, max_force); //������ �� ����
            yield return new WaitForSeconds(wait_num);
            Debug.Log($"{wait_num}�� ��ٸ� �Ϸ�");

            //rigid.velocity = Vector3.forward * pos_speed;
        }
    }

}

public enum NPC_state
{

}

public enum Test_state
{
    mobile = 0,
    pc
}

public class NPC_YG : MonoBehaviour
{
    [SerializeField] private Transform trans;
    [SerializeField] private Animator ani;

    [SerializeField] private Transform[] goals;
    [SerializeField] private Transform goal;
    [SerializeField] private int index;
    [SerializeField] private float speed = 0.1f;

    [SerializeField] private Test_state test_state;
    [SerializeField] private Touch touch;
    [SerializeField] private Vector3 touched_pos;
    [SerializeField] private Vector3 mouse_pos;
    [SerializeField] private bool touch_on;
    [SerializeField] private LayerMask layer;

    private void Awake()
    {
        //������Ʈ ��������
        TryGetComponent(out ani);
        TryGetComponent(out trans);
        goal = goals[0];
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

    IEnumerator Find_posttion()
    {
        index = 0;
        while (true)
        {
            index = Random.Range(0, goals.Length);
            if (goals[index] != goal)
            {
                index = Random.Range(0, goals.Length);
                //Debug.Log($"�̹� ��ǥ / {goal.name}");
                //Debug.Log($"��ǥ ��ġ / {goal.position}");
                break;
            }
            yield return null;
        }
        yield return null;
    }

    IEnumerator Set_position()
    {
        while (true)
        {
            //Debug.Log(Vector3.Distance(trans.position, goal.position));

            if (Vector3.Distance(trans.position, goal.position) >= 0.75f)
            {
                ani.SetBool("is_walk", true);
                Vector3 tmprot = goal.position - transform.position;
                tmprot.y = 0;
                tmprot.Normalize();
                transform.rotation = Quaternion.LookRotation(tmprot);
                transform.position = Vector3.MoveTowards(transform.position, goal.position, Time.deltaTime);
            }

            else
            {
                ani.SetBool("is_walk", false);
                yield return new WaitForSeconds(3f);
                index = Random.Range(0, goals.Length);
                goal = goals[index];
                break;
            }
            yield return null;
        }
        StartCoroutine(Set_position());
    }

    private void Input_touch()
    {
        switch (test_state)
        {
            case Test_state.mobile:
                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        touch_on = true;
                        touched_pos = Camera.main.ScreenToWorldPoint(touch.position);
                        Try_raycast(touch.position);
                    }
                }
                break;

            case Test_state.pc:
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Input.GetMouseButtonDown(0)");
                    mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    touch_on = true;
                    Try_raycast(Input.mousePosition);
                }
                break;
        }
    }

    private void Try_raycast(Vector3 pos)
    {
        Debug.Log("Try_raycast");
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            Debug.Log("���� ��");
            if (hit.collider.CompareTag("NPC"))
            {
                Debug.Log("NPCã��");
                TalkManager.instance.Print();
            }
        }


    }

}
