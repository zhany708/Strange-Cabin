using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ZhangYu.Utilities;

public class DoorController : MonoBehaviour
{
    public Animator[] DoorAnimators;
    public GameObject[] EnemyObjects;

    public int EnemyCount {  get; private set; }
    public bool IsRoomClean = false;     //��ʾ�����й����Ƿ��Լ�����ɾ�



    RoomController m_MainRoom;
    RandomPosition m_EnemySpwanPos;

    



    private void Awake()
    {
        m_MainRoom = GetComponentInParent<RoomController>();

        /*  ����Ҫ�����������ʱ��ʹ������������
        LeftDownPatrolPoint = new Vector2(m_MainRoom.transform.position.x - 5, m_MainRoom.transform.position.y - 2);
        RightTopPatrolPoint = new Vector2(m_MainRoom.transform.position.x + 5, m_MainRoom.transform.position.y + 2);
        */

        //���ɵ�x��ΧΪ���������x�Ӽ�5�����ɵ�y��ΧΪ���������y�Ӽ�2
        m_EnemySpwanPos = new RandomPosition(new Vector2(m_MainRoom.transform.position.x - 5, m_MainRoom.transform.position.y - 2), new Vector2(m_MainRoom.transform.position.x + 5, m_MainRoom.transform.position.y + 2));
    }

    private void Start()
    {
        EnemyCount = 0;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsRoomClean)
            {
                CloseDoors();
                GenerateEnemy();
            }        
        }
    }




    public void OpenDoors()
    {
        if (EnemyObjects.Length != 0)   //��������й���
        {
            if (EnemyCount >= EnemyObjects.Length) 
            {
                for (int i = 0; i < DoorAnimators.Length; i++)
                {
                    DoorAnimators[i].SetBool("IsOpen", true);      //����ȫ���������Ŵ�
                    DoorAnimators[i].SetBool("IsClose", false);

                    IsRoomClean = true;
                }
            }
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







    public void IncrementEnemyCount()
    {
        EnemyCount++;
    }
}
