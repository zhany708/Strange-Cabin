using System;
using UnityEngine;


public class WeaponHitboxToWeapon : MonoBehaviour       //���ڽ�AggressiveWeapon�ű��е������⺯��ת����Weapon�ű���ͬʱ����ͬ�����Ĺ�������Ч��
{
    public event Action OnStart;          //���շ�ΪMeleeWeapon�ű�
    public event Action OnFinish;         //�����¼���ΪWeapon�ű�

    MeleeWeapon m_Weapon;

    private void Awake()
    {
        m_Weapon = GetComponent<MeleeWeapon>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Weapon.AddToDetected(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Weapon.RemoveFromDetected(collision);
    }



    private void AnimationActionTrigger() => OnStart?.Invoke();
    private void AnimationFinishTrigger() => OnFinish?.Invoke();
}
