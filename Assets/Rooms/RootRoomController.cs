using UnityEngine;


public class RootRoomController : MonoBehaviour
{
    public Transform CameraConfiner;



    DoorController m_DoorInsideThisRoom;


    private void Awake()
    {
        m_DoorInsideThisRoom = GetComponentInChildren<DoorController>();
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraConfiner.position = transform.position;   //玩家进入房间后更改相机碰撞框的位置
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_DoorInsideThisRoom.HasGeneratedEvent)     //检查房间是否生成过事件
            {
                m_DoorInsideThisRoom.SetHasGeneratedEvent(false);       //将检查房间有无生成事件的布尔设置为false
                m_DoorInsideThisRoom.EventManagerAtDoor.DeactivateEventObject();        //玩家离开房间后销毁事件物体
            }            
        }
    }
}
