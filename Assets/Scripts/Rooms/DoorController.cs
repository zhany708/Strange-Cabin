using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ZhangYu.Utilities;

public class DoorController : MonoBehaviour
{
    public Animator[] DoorAnimators;
    public GameObject[] EnemyObjects;

    public int EnemyCount {  get; private set; }
    public bool IsRoomClean = false;     //表示房间中怪物是否以及清理干净



    RoomController m_MainRoom;
    RandomPosition m_EnemySpwanPos;

    



    private void Awake()
    {
        m_MainRoom = GetComponentInParent<RoomController>();

        /*  当需要房间的两个点时再使用这两个变量
        LeftDownPatrolPoint = new Vector2(m_MainRoom.transform.position.x - 5, m_MainRoom.transform.position.y - 2);
        RightTopPatrolPoint = new Vector2(m_MainRoom.transform.position.x + 5, m_MainRoom.transform.position.y + 2);
        */

        //生成的x范围为房间坐标的x加减5，生成的y范围为房间坐标的y加减2
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
        if (EnemyObjects.Length != 0)   //如果房间有怪物
        {
            if (EnemyCount >= EnemyObjects.Length) 
            {
                for (int i = 0; i < DoorAnimators.Length; i++)
                {
                    DoorAnimators[i].SetBool("IsOpen", true);      //怪物全部死亡后将门打开
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
            DoorAnimators[i].SetBool("IsOpen", false);      //角色进入房间后将门关闭
            DoorAnimators[i].SetBool("IsClose", true);
        }
    }



    private void GenerateEnemy()
    {
        if (EnemyObjects.Length != 0)   //如果房间有怪物
        {
            List<Vector2> enemySpawnList = m_EnemySpwanPos.GenerateMultiRandomPos(EnemyObjects.Length);     //根据怪物数量生成随机坐标list

            for (int i = 0; i < EnemyObjects.Length; i++)
            {
                GameObject enemy = EnemyPool.Instance.GetObject(EnemyObjects[i], enemySpawnList[i]);     //从敌人对象池中生成敌人
                enemy.transform.position = enemySpawnList[i];
                enemy.GetComponentInChildren<EnemyDeath>().SetDoorController(this);
                enemy.GetComponentInChildren<Stats>().SetCurrentHealth(enemy.GetComponentInChildren<Stats>().MaxHealth);    //生成敌人后重置生命，否则重新激活的敌人生命依然为0
            }
        }
    }







    public void IncrementEnemyCount()
    {
        EnemyCount++;
    }
}
