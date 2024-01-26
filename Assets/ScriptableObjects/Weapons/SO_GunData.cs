using UnityEngine;


[CreateAssetMenu(fileName = "newGunWeaponData", menuName = "Data/Weapon Data/GunWeapon")]
public class SO_GunData : SO_WeaponData
{
    [SerializeField] private GunAttackDetails m_AttackDetail;

    public GunAttackDetails AttackDetail { get => m_AttackDetail; private set => m_AttackDetail = value; }      //���������ű����ô˽ű��е�˽�б���
}



[System.Serializable]       //������Unity����ʾ�ҵ������²���
public struct GunAttackDetails
{
    public float DamageAmount;

    public float KnockbackStrength;
}
