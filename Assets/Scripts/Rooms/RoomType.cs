using System;
using UnityEngine;




public enum RoomTypeName
{
    None,

    AllDirection,   //���ܶ�����
    AllHorizontal,  //���Ҷ�����
    AllVertical,    //���¶�����
    OneHorizontal,  //������һ����
    OneVertical,    //������һ����
    AllHorizontalAndOneVertical,    //���Ҷ����ţ�����ֻ��һ��
    OneHorizontalAndAllVertical,    //���¶����ţ�����ֻ��һ��
    OneHorizontalAndOneVertical    //���º����Ҹ���һ����
}


[Flags]
public enum DoorFlags   //ͨ��Bit Flag�жϷ��������
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
    //ToDo��������Ҫ����Set��Χ
    public bool HasLeftDoor;
    public bool HasRightDoor;
    public bool HasUpDoor;
    public bool HasDownDoor;


 
    DoorFlags m_DoorFlags;


    bool m_CanRotate;
    bool m_isRotate = false;

    /* X��180��ת���£�Y��180��ת���ң�Z��180ͬʱ��ת���º�����
    1. ���ܶ����ţ�����Ҫ��ת��
    2. ���Ҷ����ţ�������ڴ��ã�������Ҫ��ת��
    3. ������һ���ţ��ɺ�����ת��Y��180��
    4. ���¶����ţ�����Ҫ��ת��
    5. ������һ���ţ���������ת��X��180��
    6. ���º����Ҹ���һ���ţ����������������ͺ�����ת��X��180��Y��180��
    7. ���¶��У�����ֻ��һ����������ת��Y��180��
    8. ���Ҷ����ţ�����ֻ��һ����������ת��X��180��
     */




    private void Awake()
    {
        if (!m_isRotate)
        {
            SetFourDoorsBool();     //��Ϸ��ʼʱ��ֵ�ĸ�����
        }

        
        m_DoorFlags = DoorFlags.None;       //��Bit Flag��ֵ
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

        if (HasLeftDoor && HasRightDoor && HasUpDoor && HasDownDoor)    //�ȼ���Ƿ�����
        {
            m_CanRotate = false;
            return RoomTypeName.AllDirection;
        }

        else if (HasLeftDoor && HasRightDoor)   //�ټ���Ƿ���������
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

        else if (HasUpDoor && HasDownDoor)      //�ټ���Ƿ���������
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

        else if (hasHorizontal && hasVertical)      //�ټ���Ƿ�������������
        {
            m_CanRotate = true;
            return RoomTypeName.OneHorizontalAndOneVertical;
        }

        else if (hasHorizontal)     //�ټ���Ƿ�ֻ��һ����������
        {
            m_CanRotate = true;
            return RoomTypeName.OneHorizontal;
        }

        else if (hasVertical)
        {
            m_CanRotate = true;
            return RoomTypeName.OneVertical;
        }

        return RoomTypeName.None;   //Ĭ�Ϸ��ؿ�
    }




    public Vector3 RotateRoom(string needDoor)
    {
        RoomTypeName currentRoomType = GetRoomType();
        //Debug.Log(currentRoomType);

        Vector3 newRotation = Vector3.zero;     //Ĭ��ֵ

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
                    newRotation = new Vector3(0, 180f, 0);      //����ת
                    SwapBool(ref HasLeftDoor, ref HasRightDoor);    //��������������ֵ����              
                }
                break;

            case RoomTypeName.OneVertical:
            case RoomTypeName.AllHorizontalAndOneVertical:
                if (needDoor == "UpDoor" || needDoor == "DownDoor")
                {
                    newRotation = new Vector3(180f, 0, 0);      //����ת
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
        Transform doors = transform.Find("Doors");      //�ҵ�Doors�����壬Ȼ����һѰ���Ƿ��ж�Ӧ�Ĳ���

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




    private void SwapBool(ref bool a, ref bool b)       //������������ֵ
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