using System;
using UnityEngine;


public class WeaponHitboxToWeapon : MonoBehaviour       //���ڽ�AggressiveWeapon�ű��е������⺯��ת����Weapon�ű���ͬʱ����ͬ�����Ĺ�������Ч��
{
    public event Action OnStartMovement;  //���շ�ΪAggressiveWeapon�ű�
    public event Action OnStopMovement;   //���շ�ΪAggressiveWeapon�ű�


    MeleeWeapon m_Weapon;

    private void Awake()
    {
        m_Weapon = GetComponentInParent<MeleeWeapon>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Weapon.AddToDetected(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Weapon.RemoveFromDetected(collision);
    }



    private void AnimationStartMovementTrigger() => OnStartMovement?.Invoke();
    private void AnimationStopMovementTrigger() => OnStopMovement?.Invoke();
}
