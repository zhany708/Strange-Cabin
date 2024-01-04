using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/player Data/Base Data")]

public class PlayerData : ScriptableObject      //����ͨ���˽ű�����Asset
{
    [Header("Move State")]
    public float MovementVelocity = 2f;
    public float MaxHealth = 999f;
    public float CurrentHealth;
}
