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
            confiner.m_BoundingShape2D = m_CameraConfiner;      //玩家进入房间后更改虚拟相机的相机碰撞框

            //GenerateRoom();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_DoorInsideThisRoom.RoomTrigger.enabled = true;    //玩家离开房间后重新激活门的碰撞器，从而让玩家之后再进入时生成敌人

            if (m_DoorInsideThisRoom.HasGeneratedEvent)     //检查房间是否生成过事件
            {
                m_DoorInsideThisRoom.SetHasGeneratedEvent(false);       //将检查房间有无生成事件的布尔设置为false
                m_DoorInsideThisRoom.EventManagerAtDoor.DeactivateEventObject();        //玩家离开房间后销毁事件物体
            }            
        }
    }



    
    private void GenerateRoom()
    {
        Transform doors = transform.Find("Doors");      //由于Transform.Find只能找到一层子物体，因此如果需要寻找子物体的子物体，则需要调用两次

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