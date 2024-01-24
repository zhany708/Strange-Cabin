using UnityEngine;


[CreateAssetMenu(fileName = "newMeleeWeaponData", menuName = "Data/Weapon Data/MeleeWeapon")]

public class SO_MeleeWeaponData : SO_WeaponData
{
    [SerializeField] private MeleeWeaponAttackDetails[] attackDetails;


    public MeleeWeaponAttackDetails[] AttackDetails { get => attackDetails; private set => attackDetails = value; }      //���������ű����ô˽ű��е�˽�б���



    private void OnEnable()
    {
        AmountOfAttack = attackDetails.Length;      //���������Ĺ�������

        /*
        MovementSpeed = new float[AmountOfAttack];

        for (int i = 0; i < AmountOfAttack; i++)
        {
            MovementSpeed[i] = attackDetails[i].MovementSpeed;      //�����������������е��ƶ������ٶȴ��ݸ����������е��ƶ������ٶ�
        }
        */
    }
}






[System.Serializable]       //������Unity����ʾ�ҵ������²���
public struct MeleeWeaponAttackDetails
{
    public string AttackName;
    //public float MovementSpeed;

    public float DamageAmount;
    public float KnockbackStrength;

    public float CameraShakeIntensity;
    public float CameraShakeDuration;
}
