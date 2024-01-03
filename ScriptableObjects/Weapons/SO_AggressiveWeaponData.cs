using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAggressiveWeaponData", menuName = "Data/Weapon Data/AggressiveWeapon")]

public class SO_AggressiveWeaponData : SO_WeaponData
{
    [SerializeField] private WeaponAttackDetails[] attackDetails;


    public WeaponAttackDetails[] AttackDetails { get => attackDetails; private set => attackDetails = value; }      //用于其他脚本调用此脚本中的私有变量


    private void OnEnable()
    {
        AmountOfAttack = attackDetails.Length;      //传递武器的攻击次数

        movementSpeed = new float[AmountOfAttack];

        for (int i = 0; i < AmountOfAttack; i++)
        {
            movementSpeed[i] = attackDetails[i].MovementSpeed;      //将攻击性武器数据中的移动补偿速度传递给武器数据中的移动补偿速度
        }
    }
}






[System.Serializable]       //用于在Unity中显示且调整以下参数
public struct WeaponAttackDetails
{
    public string AttackName;
    public float MovementSpeed;
    public float DamageAmount;
}
