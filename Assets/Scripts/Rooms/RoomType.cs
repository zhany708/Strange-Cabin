using UnityEngine;

[System.Serializable]
public class RoomType
{
    public GameObject RoomPrefab;
    public bool HasLeftDoor;
    public bool HasRightDoor;
    public bool HasUpDoor;
    public bool HasDownDoor;
  

    /* X��180��ת���£�Y��180��ת���ң�Z��180ͬʱ��ת���º�����
    0. ���ܶ����ţ�����Ҫ��ת��
    1. ���Ҷ����ţ�������ڴ��ã�������Ҫ��ת��
    2. ������һ���ţ��ɺ�����ת��Y��180��
    3. ���¶����ţ�����Ҫ��ת��
    4. ������һ���ţ���������ת��X��180��
    5. ���º����Ҹ���һ���ţ����������������ͺ�����ת��X��180��Y��180��
    6. ���¶��У�����ֻ��һ����������ת��ת��Y��180��
    7. ���Ҷ����ţ�����ֻ��һ����������ת��ת��X��180��
     */




    public string CheckRoomType()
    {
        string a = "";


        return a;
    }
}