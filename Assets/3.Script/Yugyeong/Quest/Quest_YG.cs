using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Quest_YG", menuName = "YG/Quest_YG/quest_data", order = 0)]
public class Quest_YG : ScriptableObject
{
    [Header("Info")]
    public string info;

    [Header("Reward")]
    //���߿� ������ ����� ���� �־����

    [Header("Completed")]
    //public List<QuestGoal_YG> goals;
    public Goal_YG goal;
    public QuestCompletedEvent quest_completed;
    public bool iscompleted { get; private set; }

    private void OnEnable()
    {
        Initalize();
    }

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

public class QuestCompletedEvent : UnityEvent<Quest_YG> { }

[Serializable]
public class quest_state_YG //����Ʈ ���� ������ ���� ����� �ٽ��ҿ���
{
    public Sprite sprite;
    public string item_name;
    //public int currency;
    //public int XP;
}