using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;

    List<Idamageable> m_DetectedDamageable = new List<Idamageable>();     //用于储存所有在攻击范围的碰撞体


    protected override void Awake()
    {
        base.Awake();

        if (weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;     //如果WeaponData等于于当前AggressiveWeaponData相同，则将当前攻击性武器数据的Reference传给Weapon脚本
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
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];        //调用攻击性武器中不同连击次数的信息

        foreach (Idamageable item in m_DetectedDamageable)      //对每一个有Idamageable Interface的碰撞体生效
        {
            item.Damage(details.DamageAmount);      //对被检测到碰撞体造成伤害
            item.GetHit(player.Core.Movement.FacingDirection);      //使被检测碰撞体面向玩家（如果是敌人的话）
        }
    }


    public void AddToDetected(Collider2D collision)
    {
        Idamageable damageable = collision.GetComponent<Idamageable>();

        if (damageable != null)
        {
            m_DetectedDamageable.Add(damageable);     //如果检测到可造成伤害的碰撞体，则加进List
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        Idamageable damageable = collision.GetComponent<Idamageable>();

        if (damageable != null)
        {
            m_DetectedDamageable.Remove(damageable);     //如果检测到可造成伤害的碰撞体，则移除出List
        }
    }
}
