using UnityEngine;


public class RoomController : MonoBehaviour
{
    public Transform CameraConfiner;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraConfiner.position = transform.position;   //��ҽ��뷿�����������ײ���λ��
        }
    }
}
