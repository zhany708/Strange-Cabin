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
            CameraConfiner.position = transform.position;   //��ҽ��뷿�����������ײ���λ��
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_DoorInsideThisRoom.HasGeneratedEvent)     //��鷿���Ƿ����ɹ��¼�
            {
                m_DoorInsideThisRoom.SetHasGeneratedEvent(false);       //����鷿�����������¼��Ĳ�������Ϊfalse
                m_DoorInsideThisRoom.EventManagerAtDoor.DeactivateEventObject();        //����뿪����������¼�����
            }            
        }
    }
}
