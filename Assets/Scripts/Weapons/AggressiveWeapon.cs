using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;
    protected CameraShake cameraShake;

    List<Idamageable> m_DetectedDamageables = new List<Idamageable>();     //用于储存所有在攻击范围的碰撞体
    List<IKnockbackable> m_DetectedKnockbackables = new List<IKnockbackable>();  //用于储存所有攻击范围内可击退的碰撞体


    protected override void Awake()
    {
        base.Awake();

        if (WeaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)WeaponData;     //如果WeaponData与当前AggressiveWeaponData相同，则将当前攻击性武器数据的Reference传给Weapon脚本
        }
        else
        {
            Debug.LogError("Wrong data for the weapon");
        }

        cameraShake = FindObjectOfType<CameraShake>();    //找拥有CameraShake脚本的组件
    }

    /*
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }
    */

    public void CheckMeleeAttack()     //攻击到敌人时调用此函数
    {
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[CurrentAttackCounter];        //调用攻击性武器中不同连击次数的信息

        foreach (Idamageable item in m_DetectedDamageables.ToList())      //对每一个有可造成伤害接口的碰撞体生效，加ToList防止敌人死亡后出现Bug（ToList可以复制原始List）
        {
            if (cameraShake != null)
            {
                cameraShake.ShakeCamera(details.CameraShakeIntensity, details.CameraShakeDuration);     //调用相机震动脚本
            }

            item.Damage(details.DamageAmount);      //对被检测到碰撞体造成伤害
            item.GetHit(Movement.FacingDirection);      //使被检测碰撞体面向玩家
        }

        foreach (IKnockbackable item in m_DetectedKnockbackables.ToList())
        {
            item.KnockBack(details.KnockbackStrength, Movement.FacingDirection);       //击退目标
            
        }
    }


    public void AddToDetected(Collider2D collision)
    {
        
        if (collision.TryGetComponent<Idamageable>(out var damageable))
        {
            m_DetectedDamageables.Add(damageable);     //如果检测到可造成伤害的碰撞体，则加进List
        }

        
        if (collision.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            m_DetectedKnockbackables.Add(knockbackable);    //如果检测到可被击退的碰撞体，则加进List
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        if (collision.TryGetComponent<Idamageable>(out var damageable))
        {
            m_DetectedDamageables.Remove(damageable);     //如果检测到可造成伤害的碰撞体，则加进List
        }


        if (collision.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            m_DetectedKnockbackables.Remove(knockbackable);    //如果检测到可被击退的碰撞体，则加进List
        }
    }
}
