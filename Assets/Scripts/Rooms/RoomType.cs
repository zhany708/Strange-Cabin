using UnityEngine;

[System.Serializable]
public class RoomType
{
    public GameObject RoomPrefab;
    public bool HasLeftDoor;
    public bool HasRightDoor;
    public bool HasUpDoor;
    public bool HasDownDoor;
  

    /* X轴180翻转上下，Y轴180翻转左右，Z轴180同时翻转上下和左右
    0. 四周都有门（不需要旋转）
    1. 左右都有门（比如入口大堂）（不需要旋转）
    2. 左右有一个门（可横向旋转，Y轴180）
    3. 上下都有门（不需要旋转）
    4. 上下有一个门（可竖向旋转，X轴180）
    5. 上下和左右各有一个门（比如厨房）（X轴180，Y轴180，Z轴180）
    6. 上下都有，左右只有一个（横向旋转，Y轴180）
    7. 左右都有门，上下只有一个（竖向旋转，X轴180）
     */




    public string CheckRoomType()
    {
        string a = "";


        return a;
    }
}
