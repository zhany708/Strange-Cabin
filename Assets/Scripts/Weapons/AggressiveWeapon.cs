using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;

    List<Idamageable> m_DetectedDamageables = new List<Idamageable>();     //���ڴ��������ڹ�����Χ����ײ��
    List<IKnockbackable> m_DetectedKnockbackables = new List<IKnockbackable>();  //���ڴ������й�����Χ�ڿɻ��˵���ײ��


    protected override void Awake()
    {
        base.Awake();

        if (weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;     //���WeaponData�뵱ǰAggressiveWeaponData��ͬ���򽫵�ǰ�������������ݵ�Reference����Weapon�ű�
        }
        else
        {
            Debug.LogError("Wrong data for the weapon");
        }
    }


    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }


    private void CheckMeleeAttack()     //����������ʱ���ô˺���
    {
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];        //���ù����������в�ͬ������������Ϣ

        foreach (Idamageable item in m_DetectedDamageables.ToList())      //��ÿһ���п�����˺��ӿڵ���ײ����Ч����ToList��ֹ�������������Bug��ToList���Ը���ԭʼList��
        {
            item.Damage(details.DamageAmount);      //�Ա���⵽��ײ������˺�
            item.GetHit(Movement.FacingDirection);      //ʹ�������ײ���������
        }

        foreach (IKnockbackable item in m_DetectedKnockbackables.ToList())
        {
            item.KnockBack(details.KnockbackStrength, Movement.FacingDirection);       //����Ŀ��
            
        }
    }


    public void AddToDetected(Collider2D collision)
    {
        
        if (collision.TryGetComponent<Idamageable>(out var damageable))
        {
            m_DetectedDamageables.Add(damageable);     //�����⵽������˺�����ײ�壬��ӽ�List
        }

        
        if (collision.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            m_DetectedKnockbackables.Add(knockbackable);    //�����⵽�ɱ����˵���ײ�壬��ӽ�List
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<Idamageable>(out var damageable))
        {
            m_DetectedDamageables.Remove(damageable);     //�����⵽������˺�����ײ�壬��ӽ�List
        }


        if (collision.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            m_DetectedKnockbackables.Remove(knockbackable);    //�����⵽�ɱ����˵���ײ�壬��ӽ�List
        }
    }
}
