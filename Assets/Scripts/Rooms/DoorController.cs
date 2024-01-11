using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator[] DoorAnimators;
    public GameObject EnemyObject;




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CloseDoors();          
        }
    }




    public void OpenDoors()
    {
        for (int i = 0; i < DoorAnimators.Length; i++)
        {
            DoorAnimators[i].SetBool("IsOpen", true);      //怪物全部死亡后将门打开
            DoorAnimators[i].SetBool("IsClose", false);
        }
    }

    private void CloseDoors()
    {
        for (int i = 0; i < DoorAnimators.Length; i++)
        {
            DoorAnimators[i].SetBool("IsOpen", false);      //角色进入房间后将门关闭
            DoorAnimators[i].SetBool("IsClose", true);
        }

        EnemyObject.SetActive(true);    //玩家进入房间后激活怪物
    }
}
