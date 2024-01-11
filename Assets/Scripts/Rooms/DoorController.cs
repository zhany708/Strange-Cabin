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

        EnemyObject.SetActive(true);    //��ҽ��뷿��󼤻����
    }
}
