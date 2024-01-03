using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAggressiveWeaponData", menuName = "Data/Weapon Data/AggressiveWeapon")]

public class SO_AggressiveWeaponData : SO_WeaponData
{
    [SerializeField] private WeaponAttackDetails[] attackDetails;


    public WeaponAttackDetails[] AttackDetails { get => attackDetails; private set => attackDetails = value; }      //���������ű����ô˽ű��е�˽�б���


    private void OnEnable()
    {
        AmountOfAttack = attackDetails.Length;      //���������Ĺ�������

        movementSpeed = new float[AmountOfAttack];

        for (int i = 0; i < AmountOfAttack; i++)
        {
            movementSpeed[i] = attackDetails[i].MovementSpeed;      //�����������������е��ƶ������ٶȴ��ݸ����������е��ƶ������ٶ�
        }
    }
}






[System.Serializable]       //������Unity����ʾ�ҵ������²���
public struct WeaponAttackDetails
{
    public string AttackName;
    public float MovementSpeed;
    public float DamageAmount;
}
