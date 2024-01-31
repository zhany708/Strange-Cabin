using Cinemachine;
using UnityEngine;


public class RootRoomController : MonoBehaviour
{
    public GameObject[] AllRooms;

    public bool CanGenerateLeftDoor;
    public bool CanGenerateRightDoor;
    public bool CanGenerateUpDoor;
    public bool CanGenerateDownDoor;


    CinemachineVirtualCamera m_PlayerCamera;
    Collider2D m_CameraConfiner;

    DoorController m_DoorInsideThisRoom;
    RoomGenerator m_RoomGenerator;


    bool m_HasGeneratedRoom = false;

    




    private void Awake()
    {
        m_PlayerCamera = FindObjectOfType<CinemachineVirtualCamera>();
        m_CameraConfiner = transform.Find("CameraConfiner").GetComponent<Collider2D>();

        m_DoorInsideThisRoom = GetComponentInChildren<DoorController>();
        m_RoomGenerator = GameObject.Find("RoomGenerator").GetComponent<RoomGenerator>();
    }

    private void Start()
    {
        /*
        CanGenerateLeftDoor = true;
        CanGenerateRightDoor = true;
        CanGenerateUpDoor = true;
        CanGenerateDownDoor = true;
        */
    }

    private void OnEnable()
    {
        m_RoomGenerator.GeneratedRoomPos.Add(transform.position);   //每当房间激活后将当前坐标加进List
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
                GenerateRoom();
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
}