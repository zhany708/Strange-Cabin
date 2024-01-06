using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackState : PlayerAbilityState
{

    Weapon m_Weapon;

    float m_VelocityToSet;      //用于攻击期间持续移动玩家（不能只在一帧移动玩家）
    bool m_SetVelocity;         //用于检测何时结束移动玩家

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, SO_PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        m_SetVelocity = false;

        isAttack = true;

        m_Weapon.EnterWeapon();
    }

    public override void Exit()
    {
        base.Exit();

        isAttack = false;

        m_Weapon.ExitWeapon();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (m_SetVelocity)
        {
            Movement.SetVelocity(m_VelocityToSet, Movement.FacingDirection);      //持续调用此函数以持续移动玩家
        }
    }



    public void SetWeapon(Weapon weapon)
    {
        m_Weapon = weapon;      //设置新武器
        m_Weapon.InitializeWeapon(this, core);        //给武器脚本攻击状态的索引
    }

    public void SetPlayerVelocity(float velocity)       //用于动画帧事件
    {
        m_VelocityToSet = velocity;
        m_SetVelocity = true;
    }

    #region Animation Trigger
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;       //用于结束攻击动画进入闲置状态
    }

    #endregion
}