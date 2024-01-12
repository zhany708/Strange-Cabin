using System.Collections.Generic;
using UnityEngine;
using ZhangYu.Utilities;

public class DoorController : MonoBehaviour
{
    public Animator[] DoorAnimators;
    public GameObject[] EnemyObjects;

    public int EnemyCount {  get; private set; }



    RoomController m_MainRoom;
    RandomPosition m_EnemySpwanPos;

    bool m_IsRoomClean = false;     //表示房间中怪物是否以及清理干净
    



    private void Awake()
    {
        m_MainRoom = GetComponentInParent<RoomController>();
  

        //生成的x范围为房间坐标的x加减5，生成的y范围为房间坐标的y加减2
        m_EnemySpwanPos = new RandomPosition(new Vector2(m_MainRoom.transform.position.x - 5, m_MainRoom.transform.position.y - 2), new Vector2(m_MainRoom.transform.position.x + 5, m_MainRoom.transform.position.y + 2));
    }

    private void Start()
    {
        if (EnemyObjects.Length != 0)
        {
            List<Vector2> enemySpawnList = m_EnemySpwanPos.GenerateMultiRandomPos(EnemyObjects.Length);     //根据怪物数量生成随机坐标list

            for (int i = 0; i < EnemyObjects.Length; i++)
            {
                EnemyObjects[i].transform.position = enemySpawnList[i];     //将生成的随即坐标赋值给每个敌人
            }
        }

        EnemyCount = 0;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!m_IsRoomClean)
            {
                CloseDoors();
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

                    m_IsRoomClean = true;
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




        if (EnemyObjects.Length != 0)   //如果房间有怪物
        {
            for (int i = 0; i < EnemyObjects.Length; i++)
            {
                EnemyObjects[i].SetActive(true);    //玩家进入房间后激活怪物
            }
        }
    }



    public void IncrementEnemyCount()
    {
        EnemyCount++;
    }
}
