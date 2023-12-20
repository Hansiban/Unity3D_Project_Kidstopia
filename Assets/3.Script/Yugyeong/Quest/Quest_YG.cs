using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest_data", order = 0)]
public class Quest_YG : ScriptableObject
{
    [Header("Info")]
    public string info;

    [Header("Reward")]
    //���߿� ������ ����� ���� �־����

    [Header("Completed")]
    //public List<QuestGoal_YG> goals;
    public QuestGoal_YG goal;
    public QuestCompletedEvent quest_completed;
    public bool iscompleted { get; private set; }

    public void Initalize()
    {
        iscompleted = false;
        quest_completed = new QuestCompletedEvent();
        goal.Initalize();
        goal.goal_completed.AddListener(delegate { Check_goals(); });
    }

    private void Check_goals()
    {
        if (iscompleted)
        {
            //���� �ֱ�
            quest_completed.Invoke(this);
            quest_completed.RemoveAllListeners();

            //�ӽ÷� �ھƳ��� ����Ʈ �Ϸ� debug
            Debug.Log("����Ʈ �Ϸ�");
        }
    }
}

[CreateAssetMenu(fileName = "Goal", menuName = "goal_data", order = 1)]
public class QuestGoal_YG : ScriptableObject //����Ʈ ��ǥ
{
    public int current_amount { get; protected set; }
    public int required_amount;
    public bool iscompleted { get; private set; }
    public UnityEvent goal_completed;

    public void Initalize()
    {
        iscompleted = false;
        goal_completed = new UnityEvent();
    }

    private void Evaluate()
    {
        if (current_amount >= required_amount)
        {
            Complete();
        }
    }

    private void Complete()
    {
        iscompleted = true;
        goal_completed.Invoke();
        goal_completed.RemoveAllListeners();
    }
}

public class QuestCompletedEvent : UnityEvent<Quest_YG> { }

[Serializable]
public class quest_state_YG //����Ʈ ����
{
    public Sprite sprite;
    public string item_name;
    //public int currency;
    //public int XP;
}