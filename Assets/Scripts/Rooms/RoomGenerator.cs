using System.Collections.Generic;
using UnityEngine;


public class RoomGenerator : MonoBehaviour
{
    public GameObject[] AllRooms;

    public List<Vector2> GeneratedRoomPos = new List<Vector2>();







    public void GenerateRoom(Transform currentRoom)
    {
        Transform doors = currentRoom.transform.Find("Doors");      

        GenerateRoomInDirection(doors, "LeftDoor", new Vector2(-19.2f, 0));
        GenerateRoomInDirection(doors, "RightDoor", new Vector2(19.2f, 0));
        GenerateRoomInDirection(doors, "UpDoor", new Vector2(0, 10.8f));
        GenerateRoomInDirection(doors, "DownDoor", new Vector2(0, -10.8f));

        currentRoom.GetComponent<RootRoomController>().SetHasGeneratorRoom(true);
    }

    private void GenerateRoomInDirection(Transform doors, string doorName, Vector2 offset)
    {
        Transform door = doors.Find(doorName);

        if (door != null)
        {
            Vector2 roomPos = (Vector2)doors.transform.position + offset;     

            //使用对象池生成房间
            //GameObject newRoom = ParticlePool.Instance.GetObject(AllRooms[0]);
            //newRoom.transform.position = roomPos;

            foreach (var position in GeneratedRoomPos)
            {
                if (roomPos == position)
                {
                    return;     //如果坐标重复，则返回（不生成房间）
                }
            }


            int randomRoomNum = Random.Range(0, AllRooms.Length);

            GameObject allRooms = GameObject.Find("AllRooms");

            //生成房间后根据参数决定坐标，旋转和父物体
            Instantiate(AllRooms[randomRoomNum], roomPos, Quaternion.identity, allRooms.transform);
        }
    }
}