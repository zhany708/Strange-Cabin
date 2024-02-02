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
            SetFourDoors();     //��Ϸ��ʼʱ��ֵ�ĸ�����
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
                    m_RoomType = "AllDirection";  //���ܶ�����
                    m_CanRotate = false;
                }

                else if (HasUpDoor || HasDownDoor) 
                {
                    m_RoomType = "AllHorizontalAndOneVertical";   //���Ҷ����ţ�����ֻ��һ��
                    m_CanRotate = true;
                }

                else
                {
                    m_RoomType = "AllHorizontal";     //���Ҷ�����
                    m_CanRotate = false;
                }
            }

            else
            {
                if (HasUpDoor && HasDownDoor)
                {
                    m_RoomType = "OneHorizontalAndAllVertical";   //���¶��У�����ֻ��һ��
                    m_CanRotate = true;
                }

                else if (HasUpDoor || HasDownDoor)
                {
                    m_RoomType = "OneHorizontalAndOneVertical";   //���º����Ҹ���һ����
                    m_CanRotate = true;
                }

                else
                {
                    m_RoomType = "OneHorizontal";     //������һ����
                    m_CanRotate = true;
                }             
            }
        }


        else
        {
            if (HasUpDoor && HasDownDoor)
            {

                m_RoomType = "AllVertical";     //���¶�����
                m_CanRotate = false;

            }

            else
            {
                m_RoomType = "OneVertical";     //������һ����
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
                if (m_RoomType == "OneHorizontal" || m_RoomType == "OneHorizontalAndAllVertical" || m_RoomType == "OneHorizontalAndOneVertical")    //ֻ�������ֿ��Է�ת
                {
                    newRotation = new Vector3(0, 180f, 0);    //��Y�ᷭת

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
                if (m_RoomType == "OneVertical" || m_RoomType == "AllHorizontalAndOneVertical" || m_RoomType == "OneHorizontalAndOneVertical")  //ֻ�������ֿ��Է�ת
                {
                    newRotation = new Vector3(180f, 0, 0);    //��X�ᷭת

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