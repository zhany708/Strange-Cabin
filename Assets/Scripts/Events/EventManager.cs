using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public List<SO_EventData> AllEvents;



    public int EventCount { get; private set; }
    public int EnterSecondStageCount { get; private set; }
    public bool IsSecondStage {  get; private set; }


    Animator m_Animator;
    GameObject m_EventPrefab;
    Vector2 m_RoomPosition;

    UIManager m_UIManager;





    private void Awake()
    {
        m_Animator = GetComponent<Animator>();

        m_UIManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        EventCount = 0;
        EnterSecondStageCount = 1;
        IsSecondStage = false;
    }

    




    public void GenerateRandomEvent(Vector2 position, DoorController thisDoor)
    {
        m_RoomPosition = position;

        int index = Random.Range(0, AllEvents.Count);       //��������¼�
        SO_EventData eventData = AllEvents[index];

        m_EventPrefab = ParticlePool.Instance.GetObject(eventData.EventPrefab);        //�����¼�Ԥ�Ƽ�


        m_EventPrefab.transform.parent.position = m_RoomPosition;      //��ֵ�¼������ķ���������¼��ĸ����壨��Ϊ����ص�Ե�ʣ�
        m_EventPrefab.GetComponent<Event>().SetDoor(thisDoor);        //���¼������ķ��䴫��ȥ
        m_EventPrefab.GetComponent<Event>().SetEventManager(this);       //����ǰ�ű�����Event�ű�
        m_EventPrefab.GetComponent<Event>().StartEvent();       //��ʼ�¼�

        AllEvents.RemoveAt(index);      //��ʼ�¼����List���Ƴ��¼�����ֹ֮���ظ������¼�
    }

    public void DeactivateEventObject()
    {
        ParticlePool.Instance.PushObject(m_EventPrefab);    //���¼��Żض����
    }









    private void CheckIfTranstionToSecondStage()
    {
        if (EventCount >= EnterSecondStageCount)
        {
            transform.position = m_RoomPosition;        //���¼��������������Ƶ���ǰ����
            m_Animator.SetTrigger("TranstionSecondStage");  //��󲥷Ź��ɽ׶εĶ���

            IsSecondStage = true;
        }
    }

    #region AnimationEvents
    private void DisplayTransitionStageText()       //���ڽ׶ζ����о�����ʱ��ʾ����
    {
        if (m_UIManager != null)
        {
            m_UIManager.DisplayText(m_UIManager.TransitionStageTextBG);
        }
    }
    #endregion

    #region Setters
    public void SetIsSecondStage(bool isTrue)
    {
        IsSecondStage = isTrue;
    }

    public void IncrementEventCount()
    {
        EventCount++;

        CheckIfTranstionToSecondStage();    //ÿ���¼��������Ӻ����Ƿ����������׶�
    }
    #endregion
}
