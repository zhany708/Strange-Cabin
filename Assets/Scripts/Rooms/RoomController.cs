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
            CameraConfiner.position = transform.position;   //��ҽ��뷿�����������ײ���λ��

            /*
            for (int i = 0; i < DoorAnimators.Length; i++)
            {
                DoorAnimators[i].SetBool("IsOpen", false);
                DoorAnimators[i].SetBool("IsClose", true);      //��ɫ���뷿����Źر�
            }
            */
        }
    }
}
