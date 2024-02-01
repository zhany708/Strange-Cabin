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
        m_RoomManager = GameObject.Find("RoomManager").GetComponent<RoomGenerator>();   //寻找场景中这个名字的物体，并获得相应的组件
    }

    private void OnEnable()
    {
        m_RoomManager.GeneratedRoomPos.Add(transform.position);     //每当房间激活时，将当前房间的坐标加进List
    }

    private void OnDisable()
    {   
        m_RoomManager.GeneratedRoomPos.Remove(transform.position);  //每当房间取消激活时，从List中移除当前房间的坐标
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered!");

            CinemachineConfiner2D confiner = m_PlayerCamera.GetComponent<CinemachineConfiner2D>();
            confiner.m_BoundingShape2D = m_CameraConfiner;      //玩家进入房间后更改虚拟相机的相机碰撞框

            if (!m_HasGeneratedRoom)
            {
                m_RoomManager.GenerateRoom(transform);  //每当玩家进入房间，则在当前房间周围生成新的房间
            }         
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



    
    
    public void SetHasGeneratorRoom(bool isTrue)
    {
        m_HasGeneratedRoom = isTrue;
    }
}