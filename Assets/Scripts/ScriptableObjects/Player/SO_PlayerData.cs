using UnityEngine;


[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/player Data/Base Data")]

public class SO_PlayerData : ScriptableObject      //����ͨ���˽ű�����Asset
{
    [Header("Move State")]
    public float MovementVelocity = 2f;
}
