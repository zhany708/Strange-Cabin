using UnityEngine;


//[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Weapon")]

public class SO_WeaponData : ScriptableObject
{
    public int AmountOfAttack { get; protected set; }       //�����ڹ�����������ֵ�ű����޸�

    //��������ʱ�Ĳ����ƶ��ٶȡ�����ֻ���ڲ��������Ӷ���ֹUnity���ظ���Ҫ���ǵ�����ֵ������������������Ҳ���ƶ������ٶ���Ҫ���ã�
    //public float[] MovementSpeed { get; protected set; }
}
