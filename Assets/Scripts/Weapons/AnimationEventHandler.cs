using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationEventHandler : MonoBehaviour
{
    public event Action OnFinish;       //接受事件方为Weapon


    private AggressiveWeapon m_AggressiveWeapon;
    private Movement m_Movement;



    private void Awake()
    {
        m_Movement = GetComponentInParent<Player>().GetComponentInChildren<Movement>();   //先调用Player父物体，然后再从Player中寻找Core子物体
        m_AggressiveWeapon = GetComponentInParent<AggressiveWeapon>();
    }



    private void AnimationStartMovementTrigger()
    {
        m_Movement.SetVelocity(m_AggressiveWeapon.WeaponData.MovementSpeed[m_AggressiveWeapon.CurrentAttackCounter], m_Movement.FacingDirection);       //使玩家攻击时获得移动补偿
    }

    private void AnimationStopMovementTrigger()
    {
        m_Movement.SetVelocityZero();
    }

    private void AnimationActionTrigger() => m_AggressiveWeapon.CheckMeleeAttack();

    private void AnimationFinishTrigger() => OnFinish?.Invoke();

}

