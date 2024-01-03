using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;

    List<Idamageable> m_DetectedDamageable = new List<Idamageable>();     //���ڴ��������ڹ�����Χ����ײ��


    protected override void Awake()
    {
        base.Awake();

        if (weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;     //���WeaponData�����ڵ�ǰAggressiveWeaponData��ͬ���򽫵�ǰ�������������ݵ�Reference����Weapon�ű�
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


    private void CheckMeleeAttack()
    {
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];        //���ù����������в�ͬ������������Ϣ

        foreach (Idamageable item in m_DetectedDamageable)      //��ÿһ����Idamageable Interface����ײ����Ч
        {
            item.Damage(details.DamageAmount);      //�Ա���⵽��ײ������˺�
            item.GetHit(player.Core.Movement.FacingDirection);      //ʹ�������ײ��������ң�����ǵ��˵Ļ���
        }
    }


    public void AddToDetected(Collider2D collision)
    {
        Idamageable damageable = collision.GetComponent<Idamageable>();

        if (damageable != null)
        {
            m_DetectedDamageable.Add(damageable);     //�����⵽������˺�����ײ�壬��ӽ�List
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        Idamageable damageable = collision.GetComponent<Idamageable>();

        if (damageable != null)
        {
            m_DetectedDamageable.Remove(damageable);     //�����⵽������˺�����ײ�壬���Ƴ���List
        }
    }
}
