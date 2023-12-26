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
    //[SerializeField] private Rigidbody rigid;
    [SerializeField] private Transform trans;
    [SerializeField] private Animator ani;

    [SerializeField] private Transform[] goals;
    [SerializeField] private Transform goal;
    [SerializeField] private int index;
    [SerializeField] private float speed = 0.1f;

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

    IEnumerator Find_posttion()
    {
        index = 0;
        while (true)
        {
            index = Random.Range(0, goals.Length);
            if (goals[index] != goal)
            {
                index = Random.Range(0, goals.Length);
                Debug.Log($"�̹� ��ǥ / {goal.name}");
                Debug.Log($"��ǥ ��ġ / {goal.position}");
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
            Debug.Log(Vector3.Distance(trans.position, goal.position));

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
}
