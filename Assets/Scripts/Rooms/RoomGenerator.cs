using Cinemachine;
using UnityEngine;


public class RootRoomController
{
    public GameObject[] AllRooms;

    public List<Vector2> GeneratedRoomPos = new List<Vector2>();







    private void GenerateRoom(Transform currentRoom)
    {
        Transform doors = currentRoom.transform.Find("Doors");      //����Transform.Findֻ���ҵ�һ�������壬��������ҪѰ��������������壬����Ҫ��������

        GenerateRoomInDirection(doors, "LeftDoor", new Vector2(-19.2f, 0));
        GenerateRoomInDirection(doors, "RightDoor", new Vector2(19.2f, 0));
        GenerateRoomInDirection(doors, "UpDoor", new Vector2(0, 10.8f));
        GenerateRoomInDirection(doors, "DownDoor", new Vector2(0, -10.8f));

        m_HasGeneratedRoom = true;
    }

    private void GenerateRoomInDirection(Transform doors, string doorName, Vector2 offset)
    {
        Transform door = doors.Find(doorName);

        if (door != null)
        {
            Vector2 roomPos = (Vector2)transform.position + offset;     //�������ɷ����λ��

            //GameObject newRoom = ParticlePool.Instance.GetObject(AllRooms[0]);      //�Ӷ���������ɷ���
            //newRoom.transform.position = roomPos;

            foreach (var position in GeneratedRoomPos)
            {
                if (roomPos == position)
                {
                    return;
                }
            }
            //GeneratedRoomPos.Add(roomPos);  //如果坐标没有重复，则将生成的房间坐标加进List

            int randomRoomNum = Random.Range(0, AllRooms.Length);

            GameObject allRooms = GameObject.Find("AllRooms");

            //生成房间后根据参数决定坐标，旋转和父物体
            GameObject newRoom = Instantiate(AllRooms[randomRoomNum], roomPos, Quaternion.identity, allRooms.transform);
        }
    }
}