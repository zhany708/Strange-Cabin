using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;


public class RoomGenerator : MonoBehaviour
{
    public GameObject[] AllRooms;

    public List<Vector2> GeneratedRoomPos = new List<Vector2>();








    public void GenerateRoom(Transform currentRoom, RoomType currentRoomType)
    {
        GenerateRoomInDirection(currentRoom, currentRoomType.HasLeftDoor, new Vector2(-19.2f, 0), "RightDoor");
        GenerateRoomInDirection(currentRoom, currentRoomType.HasRightDoor, new Vector2(19.2f, 0), "LeftDoor");
        GenerateRoomInDirection(currentRoom, currentRoomType.HasUpDoor, new Vector2(0, 10.8f), "DownDoor");
        GenerateRoomInDirection(currentRoom,  currentRoomType.HasDownDoor, new Vector2(0, -10.8f), "UpDoor");

        currentRoom.GetComponent<RootRoomController>().SetHasGeneratorRoom(true);
    }

    private void GenerateRoomInDirection(Transform currentPos, bool hasDoor, Vector2 offset, string requiredDoor)
    {

        if (hasDoor)
        {
            Vector2 roomPos = (Vector2)currentPos.transform.position + offset;

            if (CheckOverlapPosition(roomPos))  return;     //如果有坐标重复，则返回（不生成房间）

            //if (!CheckSuitableRoom(requiredDoor)) return;   //如果没有合适的房间可以生成，则返回

            GenerateSuitableRoom(roomPos, requiredDoor);    //如果所有条件都满足，则生成合适的房间
        }
    }


    private bool CheckOverlapPosition(Vector2 checkPos)
    {
        foreach (var position in GeneratedRoomPos)
        {
            if (checkPos == position)
            {
                return true;     //如果坐标重复，则返回（不生成房间）
            }
        }

        return false;
    }


    private bool CheckSuitableRoom(string checkRequiredDoor)
    {
        int notFitRoomNum = 0;

        foreach (var room in AllRooms)
        {
            Transform checkRoomDoors = room.transform.Find("Doors");
            Transform checkdoor = checkRoomDoors.Find(checkRequiredDoor);

            if (checkdoor != null)
            {
                return true;
            }
            else
            {
                notFitRoomNum++;
                //Debug.Log(notFitRoomNum);

                if (notFitRoomNum >= AllRooms.Length)
                {
                    Debug.Log("No suitable room left!");    //当没有房间符合条件时，返回
                    return false;
                }
            }
        }

        return false;
    }


    private void GenerateSuitableRoom(Vector2 newRoomPos, string neededDoorName)
    {
        int randomRoomNum = Random.Range(0, AllRooms.Length);

        GameObject allRooms = GameObject.Find("AllRooms");      //所有生成的房间的父物体，为了整洁美观

        GameObject newRoom = Instantiate(AllRooms[randomRoomNum], newRoomPos, Quaternion.identity, allRooms.transform);
        RoomType newRoomType = newRoom.GetComponent<RoomType>();





        bool checkDoor;

        if (neededDoorName == "LeftDoor")
        {
            checkDoor = newRoomType.HasLeftDoor;
        }

        else if (neededDoorName == "RightDoor")
        {
            checkDoor = newRoomType.HasRightDoor;
        }

        else if (neededDoorName == "UpDoor")
        {
            checkDoor = newRoomType.HasUpDoor;
        }

        else
        {
            checkDoor = newRoomType.HasDownDoor;
        }




        bool done = false;
        int loopNum = 0;

        while (!done)
        {
            if (checkDoor)    //当有不需要旋转就合适的房间时，直接返回
            {
                return;
            }
            else
            {
                Vector3 newRoomRotation = newRoomType.RotateRoom(neededDoorName);

                //进行旋转之后，再次检查门的布尔
                if (neededDoorName == "LeftDoor")
                {
                    if (newRoomType.HasLeftDoor)
                    {
                        newRoomType.SetIsRotate(true);      //设置isRotate布尔为真，防止房间的Awake函数重新设置4个门的布尔


                        newRoom.transform.rotation = Quaternion.Euler(newRoomRotation);     //只改旋转角度
                        return;
                    }

                    Destroy(newRoom);
                }

                else if (neededDoorName == "RightDoor")
                {
                    if (newRoomType.HasRightDoor)
                    {
                        newRoomType.SetIsRotate(true);


                        newRoom.transform.rotation = Quaternion.Euler(newRoomRotation);
                        return;
                    }

                    Destroy(newRoom);
                }

                else if (neededDoorName == "UpDoor")
                {
                    if (newRoomType.HasUpDoor)
                    {
                        newRoomType.SetIsRotate(true);


                        newRoom.transform.rotation = Quaternion.Euler(newRoomRotation);
                        return;
                    }

                    Destroy(newRoom);
                }

                else
                {
                    if (newRoomType.HasDownDoor)
                    {
                        newRoomType.SetIsRotate(true);


                        newRoom.transform.rotation = Quaternion.Euler(newRoomRotation);
                        return;
                    }

                    Destroy(newRoom);
                }





                loopNum++;

                if (loopNum >= 100)
                {
                    Debug.Log("Loop too many times!");
                    return;     //生成房间次数大于10次后强制返回，防止出现无限循环
                }
            }

            done = false;
        }
    }
}