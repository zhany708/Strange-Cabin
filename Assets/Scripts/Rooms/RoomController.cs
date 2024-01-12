using UnityEngine;


public class RoomController : MonoBehaviour
{
    public Transform CameraConfiner;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraConfiner.position = transform.position;   //玩家进入房间后更改相机碰撞框的位置
        }
    }
}
