using Cinemachine;
using UnityEngine;


public class RootRoomController : MonoBehaviour
{
    public GameObject[] AllRooms;




    CinemachineVirtualCamera playerCamera;
    Collider2D m_CameraConfiner;

    DoorController m_DoorInsideThisRoom;









    private void Awake()
    {
        playerCamera = FindObjectOfType<CinemachineVirtualCamera>();
        m_CameraConfiner = transform.Find("CameraConfiner").GetComponent<Collider2D>();

        m_DoorInsideThisRoom = GetComponentInChildren<DoorController>();
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered!");

            CinemachineConfiner2D confiner = playerCamera.GetComponent<CinemachineConfiner2D>();
            confiner.m_BoundingShape2D = m_CameraConfiner;      //��ҽ��뷿��������������������ײ��

            //GenerateRoom();
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



    
    private void GenerateRoom()
    {
        Transform doors = transform.Find("Doors");      //����Transform.Findֻ���ҵ�һ�������壬��������ҪѰ��������������壬����Ҫ��������

        Transform leftDoor = doors.transform.Find("LeftDoor");
        Transform rightDoor = doors.transform.Find("RightDoor");
        Transform upDoor = doors.transform.Find("UpDoor");
        Transform downDoor = doors.transform.Find("DownDoor");


        if (leftDoor != null)
        {
            //Debug.Log("LeftDoor is here!");
            Vector2 roomPos = new Vector2(transform.position.x - 19.2f, transform.position.y);

            GameObject newRoom = ParticlePool.Instance.GetObject(AllRooms[0]);
            newRoom.transform.position = roomPos;

        }
        if (rightDoor != null)
        {
            Vector2 roomPos = new Vector2(transform.position.x + 19.2f, transform.position.y);

            GameObject newRoom = ParticlePool.Instance.GetObject(AllRooms[0]);
            newRoom.transform.position = roomPos;
        }
        if (upDoor != null)
        {
            Vector2 roomPos = new Vector2(transform.position.x, transform.position.y + 10.8f);

            GameObject newRoom = ParticlePool.Instance.GetObject(AllRooms[0]);
            newRoom.transform.position = roomPos;
        }
        if (downDoor != null)
        {
            Vector2 roomPos = new Vector2(transform.position.x, transform.position.y - 10.8f);

            GameObject newRoom = ParticlePool.Instance.GetObject(AllRooms[0]);
            newRoom.transform.position = roomPos;
        }
    }
}