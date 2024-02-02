using System;
using UnityEngine;




public enum RoomTypeName
{
    None,

    AllDirection,   //四周都有门
    AllHorizontal,  //左右都有门
    AllVertical,    //上下都有门
    OneHorizontal,  //左右有一个门
    OneVertical,    //上下有一个门
    AllHorizontalAndOneVertical,    //左右都有门，上下只有一个
    OneHorizontalAndAllVertical,    //上下都有门，左右只有一个
    OneHorizontalAndOneVertical    //上下和左右各有一个门
}


[Flags]
public enum DoorFlags   //通过Bit Flag判断房间的种类
{
    None = 0,

    Left = 1 << 0,  //1
    Right = 1 << 1, //2
    Up = 1 << 2,    //4
    Down = 1 << 3  //8
}






[System.Serializable]
public class RoomType : MonoBehaviour
{
    //ToDo：后期需要更改Set范围
    public bool HasLeftDoor;
    public bool HasRightDoor;
    public bool HasUpDoor;
    public bool HasDownDoor;


 
    DoorFlags m_DoorFlags;


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
            SetFourDoorsBool();     //游戏开始时赋值四个布尔
        }

        
        m_DoorFlags = DoorFlags.None;       //给Bit Flag赋值
        if (HasLeftDoor) m_DoorFlags |= DoorFlags.Left;
        if (HasRightDoor) m_DoorFlags |= DoorFlags.Right;
        if (HasUpDoor) m_DoorFlags |= DoorFlags.Up;
        if (HasDownDoor) m_DoorFlags |= DoorFlags.Down;
        
    }

    private void OnEnable()
    {
        //Debug.Log(gameObject.name + "'s room type is " + GetRoomTypeName() );
    }



    


    public RoomTypeName GetRoomType()
    {
        
        bool HasLeftDoor = (m_DoorFlags & DoorFlags.Left) != 0;
        bool HasRightDoor = (m_DoorFlags & DoorFlags.Right) != 0;
        bool HasUpDoor = (m_DoorFlags & DoorFlags.Up) != 0;
        bool HasDownDoor = (m_DoorFlags & DoorFlags.Down) != 0;
        

        bool hasHorizontal = HasLeftDoor || HasRightDoor;
        bool hasVertical = HasUpDoor || HasDownDoor;

        if (HasLeftDoor && HasRightDoor && HasUpDoor && HasDownDoor)    //先检查是否都有门
        {
            m_CanRotate = false;
            return RoomTypeName.AllDirection;
        }

        else if (HasLeftDoor && HasRightDoor)   //再检查是否都有左右门
        {
            if (HasUpDoor || HasDownDoor)
            {
                m_CanRotate = true;
                return RoomTypeName.AllHorizontalAndOneVertical;
            }
            else
            {
                m_CanRotate = false;
                return RoomTypeName.AllHorizontal;
            }
        }

        else if (HasUpDoor && HasDownDoor)      //再检查是否都有上下门
        {
            if (HasLeftDoor || HasRightDoor)
            {
                m_CanRotate = true;
                return RoomTypeName.OneHorizontalAndAllVertical;
            }
            else
            {
                m_CanRotate = false;
                return RoomTypeName.AllVertical;
            }
        }

        else if (hasHorizontal && hasVertical)      //再检查是否两个方向都有门
        {
            m_CanRotate = true;
            return RoomTypeName.OneHorizontalAndOneVertical;
        }

        else if (hasHorizontal)     //再检查是否只有一个方向有门
        {
            m_CanRotate = true;
            return RoomTypeName.OneHorizontal;
        }

        else if (hasVertical)
        {
            m_CanRotate = true;
            return RoomTypeName.OneVertical;
        }

        return RoomTypeName.None;   //默认返回空
    }




    public Vector3 RotateRoom(string needDoor)
    {
        RoomTypeName currentRoomType = GetRoomType();
        //Debug.Log(currentRoomType);

        Vector3 newRotation = Vector3.zero;     //默认值

        if (!m_CanRotate)
        {
            return newRotation;
        }

        switch (currentRoomType)
        {
            case RoomTypeName.OneHorizontal:
            case RoomTypeName.OneHorizontalAndAllVertical:
                if (needDoor == "LeftDoor" || needDoor == "RightDoor")
                {
                    newRotation = new Vector3(0, 180f, 0);      //横向翻转
                    SwapBool(ref HasLeftDoor, ref HasRightDoor);    //将这两个布尔的值互换              
                }
                break;

            case RoomTypeName.OneVertical:
            case RoomTypeName.AllHorizontalAndOneVertical:
                if (needDoor == "UpDoor" || needDoor == "DownDoor")
                {
                    newRotation = new Vector3(180f, 0, 0);      //竖向翻转
                    SwapBool(ref HasUpDoor, ref HasDownDoor);         
                }
                break;

            case RoomTypeName.OneHorizontalAndOneVertical:
                if (needDoor == "LeftDoor" || needDoor == "RightDoor")
                {
                    newRotation = new Vector3(0, 180f, 0);
                    SwapBool(ref HasLeftDoor, ref HasRightDoor);             
                }

                else if (needDoor == "UpDoor" || needDoor == "DownDoor")
                {
                    newRotation = new Vector3(180f, 0, 0);
                    SwapBool(ref HasUpDoor, ref HasDownDoor);            
                }
                break;

            default:
                break;
        }

        return newRotation;
    }






    private void SetFourDoorsBool()
    {
        Transform doors = transform.Find("Doors");      //找到Doors子物体，然后逐一寻找是否有对应的侧门

        HasLeftDoor = IsDoorExist(doors, "LeftDoor");
        HasRightDoor = IsDoorExist(doors, "RightDoor");
        HasUpDoor = IsDoorExist(doors, "UpDoor");
        HasDownDoor = IsDoorExist(doors, "DownDoor");
    }

    private bool IsDoorExist(Transform doors, string checkDoor)
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




    private void SwapBool(ref bool a, ref bool b)       //互换两个布尔值
    {
        bool temp = a;
        a = b; 
        b = temp;
    }



    public void SetIsRotate(bool isTrue)
    {
        m_isRotate = isTrue;
    }
}