using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomController : MonoBehaviour
{
    public Transform CameraConfiner;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraConfiner.position = transform.position;   //玩家进入房间后更改相机碰撞框的位置

            /*
            for (int i = 0; i < DoorAnimators.Length; i++)
            {
                DoorAnimators[i].SetBool("IsOpen", false);
                DoorAnimators[i].SetBool("IsClose", true);      //角色进入房间后将门关闭
            }
            */
        }
    }
}
