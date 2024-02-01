using UnityEngine;


[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/player Data/Base Data")]

public class SO_PlayerData : ScriptableObject      //����ͨ���˽ű�����Asset
{
    [Header("Move State")]
    public float MovementVelocity = 2f;
    public float MaxHealth = 10f;
    public float Defense = 2f;
    public float HitResistance = 1f;
}
