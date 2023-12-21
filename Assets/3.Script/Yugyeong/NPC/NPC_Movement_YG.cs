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
public class NPC_Movement_YG : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Transform trans;

    [SerializeField] private Transform[] goals;
    [SerializeField] private Transform goal;
    [SerializeField] private float speed;
    [SerializeField] private bool is_;

    private void Awake()
    {
        //������Ʈ ��������
        TryGetComponent(out rigid);
        TryGetComponent(out trans);
        goal = trans;
    }

    private void Update()
    {
        if (trans.position == goal.position)
        {
            Debug.Log("��ǥ���� ����");
            StartCoroutine(Find_posttion());
        }

        Debug.Log("��ǥ�� ������");
        trans.position = Vector3.MoveTowards(trans.position, goal.position, Time.deltaTime);
    }

    IEnumerator Find_posttion()
    {
        while (true)
        {
            int index = Random.Range(0, goals.Length);
            if (goals[index] != goal)
            {
                goal = goals[index];
                Debug.Log($"�̹� ��ǥ / {goal.name}");
                break;
            }
        }
        Debug.Log("�ڷ�ƾ ��");
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.CompareTag("tran"))
        {
            rigid.velocity = Vector3.zero;

            StartCoroutine(Find_posttion());
        }
    }

}


