using Cinemachine;
using UnityEngine;


public class RootRoomController : MonoBehaviour
{
    CinemachineVirtualCamera m_PlayerCamera;
    Collider2D m_CameraConfiner;

    DoorController m_DoorInsideThisRoom;
    RoomGenerator m_RoomManager;
    


    bool m_HasGeneratedRoom = false;

    




    private void Awake()
    {
        m_PlayerCamera = FindObjectOfType<CinemachineVirtualCamera>();
        m_CameraConfiner = transform.Find("CameraConfiner").GetComponent<Collider2D>();

        m_DoorInsideThisRoom = GetComponentInChildren<DoorController>();
        m_RoomManager = GameObject.Find("RoomManager").GetComponent<RoomGenerator>();   //Ѱ�ҳ�����������ֵ����壬�������Ӧ�����
    }

    private void OnEnable()
    {
        m_RoomManager.GeneratedRoomPos.Add(transform.position);     //ÿ�����伤��ʱ������ǰ���������ӽ�List
    }

    private void OnDisable()
    {   
        m_RoomManager.GeneratedRoomPos.Remove(transform.position);  //ÿ������ȡ������ʱ����List���Ƴ���ǰ���������
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered!");

            CinemachineConfiner2D confiner = m_PlayerCamera.GetComponent<CinemachineConfiner2D>();
            confiner.m_BoundingShape2D = m_CameraConfiner;      //��ҽ��뷿��������������������ײ��

            if (!m_HasGeneratedRoom)
            {
                m_RoomManager.GenerateRoom(transform);  //ÿ����ҽ��뷿�䣬���ڵ�ǰ������Χ�����µķ���
            }         
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_DoorInsideThisRoom.RoomTrigger.enabled = true;    //����뿪��������¼����ŵ���ײ�����Ӷ������֮���ٽ���ʱ���ɵ���

            if (m_DoorInsideThisRoom.HasGeneratedEvent)     //��鷿���Ƿ����ɹ��¼�
            {
                m_DoorInsideThisRoom.SetHasGeneratedEvent(false);       //����鷿�����������¼��Ĳ�������Ϊfalse
                m_DoorInsideThisRoom.EventManagerAtDoor.DeactivateEventObject();        //����뿪����������¼�����
            }            
        }
    }



    
    
    public void SetHasGeneratorRoom(bool isTrue)
    {
        m_HasGeneratedRoom = isTrue;
    }
}