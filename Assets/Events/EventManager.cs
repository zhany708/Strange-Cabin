using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public List<SO_EventData> AllEvents;



    public int EventCount { get; private set; }
    public int EnterSecondStageCount { get; private set; }
    public bool IsSecondStage {  get; private set; }


    GameObject m_EventPrefab;






    private void Start()
    {
        EventCount = 0;
        EnterSecondStageCount = 1;
        IsSecondStage = false;
    }






    public void GenerateRandomEvent(Vector2 position, DoorController thisDoor)
    {
        int index = Random.Range(0, AllEvents.Count);       //��������¼�
        SO_EventData eventData = AllEvents[index];

        m_EventPrefab = ParticlePool.Instance.GetObject(eventData.EventPrefab);        //�����¼�Ԥ�Ƽ�


        m_EventPrefab.transform.parent.position = position;      //��ֵ�¼������ķ���������¼��ĸ����壨��Ϊ����ص�Ե�ʣ�
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
            IsSecondStage = true;
        }
    }


    public void SetIsSecondStage(bool isTrue)
    {
        IsSecondStage = isTrue;
    }

    public void IncrementEventCount()
    {
        EventCount++;

        CheckIfTranstionToSecondStage();    //ÿ���¼��������Ӻ����Ƿ����������׶�
    }
}
