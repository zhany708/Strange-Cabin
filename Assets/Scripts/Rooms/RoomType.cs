using System;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class RoomType : MonoBehaviour
{
    public bool HasLeftDoor;
    public bool HasRightDoor;
    public bool HasUpDoor;
    public bool HasDownDoor;

    string m_RoomType = null;
    bool m_CanRotate;
    bool m_isRotate = false;

    /* X轴180翻转上下，Y轴180翻转左右，Z轴180同时翻转上下和左右
    1. 四周都有门（不需要旋转）
    2. 左右都有门（比如入口大堂）（不需要旋转）
    3. 左右有一个门（可横向旋转，Y轴180）
    4. 上下都有门（不需要旋转）
    5. 上下有一个门（可竖向旋转，X轴180）
    6. 上下和左右各有一个门（比如厨房）（竖向和横向旋转，X轴180，Y轴180）
    7. 上下都有，左右只有一个（横向旋转，Y轴180）
    8. 左右都有门，上下只有一个（竖向旋转，X轴180）
     */




    private void Awake()
    {
        if (!m_isRotate)
        {
            SetFourDoors();     //游戏开始时赋值四个布尔
        }
    }

    private void OnEnable()
    {
        //Debug.Log(gameObject.name + "'s room type is " + CheckRoomType());
    }



    public string CheckRoomType()
    {
        if (HasLeftDoor || HasRightDoor)
        {
            if (HasLeftDoor && HasRightDoor)
            {
                if (HasUpDoor && HasDownDoor)   
                {
                    m_RoomType = "AllDirection";  //四周都有门
                    m_CanRotate = false;
                }

                else if (HasUpDoor || HasDownDoor) 
                {
                    m_RoomType = "AllHorizontalAndOneVertical";   //左右都有门，上下只有一个
                    m_CanRotate = true;
                }

                else
                {
                    m_RoomType = "AllHorizontal";     //左右都有门
                    m_CanRotate = false;
                }
            }

            else
            {
                if (HasUpDoor && HasDownDoor)
                {
                    m_RoomType = "OneHorizontalAndAllVertical";   //上下都有，左右只有一个
                    m_CanRotate = true;
                }

                else if (HasUpDoor || HasDownDoor)
                {
                    m_RoomType = "OneHorizontalAndOneVertical";   //上下和左右各有一个门
                    m_CanRotate = true;
                }

                else
                {
                    m_RoomType = "OneHorizontal";     //左右有一个门
                    m_CanRotate = true;
                }             
            }
        }


        else
        {
            if (HasUpDoor && HasDownDoor)
            {

                m_RoomType = "AllVertical";     //上下都有门
                m_CanRotate = false;

            }

            else
            {
                m_RoomType = "OneVertical";     //上下有一个门
                m_CanRotate = true;
            }
        }

        //Debug.Log(m_RoomType);
        return m_RoomType;
    }


    public Vector3 RotateRoom(string needDoor)
    {
        CheckRoomType();

        Vector3 newRotation;

        if (m_CanRotate)
        {
            if (needDoor == "LeftDoor" || needDoor == "RightDoor")
            {
                if (m_RoomType == "OneHorizontal" || m_RoomType == "OneHorizontalAndAllVertical" || m_RoomType == "OneHorizontalAndOneVertical")    //只有这三种可以翻转
                {
                    newRotation = new Vector3(0, 180f, 0);    //沿Y轴翻转

                    if (HasLeftDoor)
                    {
                        HasLeftDoor = false;
                        HasRightDoor = true;
                    }
                    else
                    {
                        HasLeftDoor = true;
                        HasRightDoor = false;
                    }

                    return newRotation;
                }
            }


            else if (needDoor == "UpDoor" || needDoor == "DownDoor")
            {
                if (m_RoomType == "OneVertical" || m_RoomType == "AllHorizontalAndOneVertical" || m_RoomType == "OneHorizontalAndOneVertical")  //只有这三种可以翻转
                {
                    newRotation = new Vector3(180f, 0, 0);    //沿X轴翻转

                    if (HasUpDoor)
                    {
                        HasUpDoor = false;
                        HasDownDoor = true;
                    }
                    else
                    {
                        HasUpDoor = true;
                        HasDownDoor = false;
                    }

                    return newRotation;
                }
            }

            else
            {
                Debug.Log("NeedDoor parameter is incorrect!");
            }
        }

        return Vector3.zero;
    }


    private void SetFourDoors()
    {
        Transform doors = transform.Find("Doors");

        HasLeftDoor = CheckExistDoor(doors, "LeftDoor");
        HasRightDoor = CheckExistDoor(doors, "RightDoor");
        HasUpDoor = CheckExistDoor(doors, "UpDoor");
        HasDownDoor = CheckExistDoor(doors, "DownDoor");
    }

    private bool CheckExistDoor(Transform doors, string checkDoor)
    {
        Transform door = doors.Find(checkDoor);

        if (door != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public void SetIsRotate(bool isTrue)
    {
        m_isRotate = isTrue;
    }
}