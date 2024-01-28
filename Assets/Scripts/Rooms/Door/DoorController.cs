using System.Collections.Generic;
using UnityEngine;
using ZhangYu.Utilities;

public class DoorController : MonoBehaviour
{
    public Animator[] DoorAnimators;
    public GameObject[] EnemyObjects;
    public Collider2D RoomTrigger {  get; private set; }


    public EventManager EventManagerAtDoor {  get; private set; }


    public int EnemyCount {  get; private set; }
    public bool HasGeneratedEvent { get; private set; }
    public bool IsRoomClean { get; private set; }     //��ʾ�����й����Ƿ�����ɾ�




    RootRoomController m_MainRoom; 
    RandomPosition m_EnemySpwanPos;


    bool m_IsRootRoom;









    private void Awake()
    {
        RoomTrigger = GetComponent<Collider2D>();

        m_MainRoom = GetComponentInParent<RootRoomController>();
        EventManagerAtDoor = FindObjectOfType<EventManager>();      //Ѱ���¼�������

        /*  ����Ҫ�����������ʱ��ʹ������������
        LeftDownPatrolPoint = new Vector2(m_MainRoom.transform.position.x - 5, m_MainRoom.transform.position.y - 2);
        RightTopPatrolPoint = new Vector2(m_MainRoom.transform.position.x + 5, m_MainRoom.transform.position.y + 2);
        */

        //���ɵ�x��ΧΪ���������x�Ӽ�5�����ɵ�y��ΧΪ���������y�Ӽ�2
        m_EnemySpwanPos = new RandomPosition(new Vector2(m_MainRoom.transform.position.x - 5, m_MainRoom.transform.position.y - 2), new Vector2(m_MainRoom.transform.position.x + 5, m_MainRoom.transform.position.y + 2));
    }

    private void Start()
    {
        if (m_MainRoom.GetType() == typeof(RootRoomController))     //��鵱ǰ�����Ƿ�Ϊ��ʼ���
        {
            IsRoomClean = true;
            m_IsRootRoom = true;        
        }
        else
        {
            IsRoomClean = false;
        }
        
        HasGeneratedEvent = false;
        EnemyCount = 0;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RoomTrigger.enabled = false;    //��ҽ��뷿���ȡ�������ŵ���ײ������ֹ��ҷ����������䵼�¶��������¼������

            if (!IsRoomClean)
            {
                CloseDoors();

                if (!m_IsRootRoom)    
                {
                    if (!EventManagerAtDoor.IsSecondStage)
                    {
                        //Debug.Log(transform.position);
                        EventManagerAtDoor.GenerateRandomEvent(transform.position, this);   //��һ�׶�ʱ�����¼�
                        HasGeneratedEvent = true;
                    }
                    else
                    {
                        GenerateEnemy();    //ֻ�н�����׶κ�Ż����ɵ���
                    }
                }
            }              
        }
    }






    public void OpenDoors()
    {
        for (int i = 0; i < DoorAnimators.Length; i++)
        {
            DoorAnimators[i].SetBool("IsOpen", true);      //����ȫ���������Ŵ�
            DoorAnimators[i].SetBool("IsClose", false);
        }
    }

    private void CloseDoors()
    {
        for (int i = 0; i < DoorAnimators.Length; i++)
        {
            DoorAnimators[i].SetBool("IsOpen", false);      //��ɫ���뷿����Źر�
            DoorAnimators[i].SetBool("IsClose", true);
        }
    }





    public void CheckIfOpenDoors()      //��������ʱ����
    {
        if (EnemyObjects.Length != 0)   //��������й���
        {
            if (EnemyCount >= EnemyObjects.Length)
            {
                for (int i = 0; i < DoorAnimators.Length; i++)
                {
                    IsRoomClean = true;
                    OpenDoors();
                }
            }
        }
    }





    private void GenerateEnemy()
    {
        if (EnemyObjects.Length != 0)   //��������й���
        {
            List<Vector2> enemySpawnList = m_EnemySpwanPos.GenerateMultiRandomPos(EnemyObjects.Length);     //���ݹ������������������list

            for (int i = 0; i < EnemyObjects.Length; i++)
            {
                GameObject enemy = EnemyPool.Instance.GetObject(EnemyObjects[i], enemySpawnList[i]);     //�ӵ��˶���������ɵ���
                enemy.transform.position = enemySpawnList[i];
                enemy.GetComponentInChildren<EnemyDeath>().SetDoorController(this);
                enemy.GetComponentInChildren<Stats>().SetCurrentHealth(enemy.GetComponentInChildren<Stats>().MaxHealth);    //���ɵ��˺������������������¼���ĵ���������ȻΪ0
            }
        }
    }




    public void SetHasGeneratedEvent(bool isTrue)
    {
        HasGeneratedEvent = isTrue;
    }


    public void IncrementEnemyCount()
    {
        EnemyCount++;
    }
}
