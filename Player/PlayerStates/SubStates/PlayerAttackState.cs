using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackState : PlayerAbilityState
{

    Weapon m_Weapon;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        m_Weapon.EnterWeapon();
    }

    public override void Exit() 
    { 
         base.Exit();

        m_Weapon.ExitWeapon();
    }

    public void SetWeapon(Weapon weapon)
    {
        m_Weapon = weapon;      //设置新武器
        m_Weapon.InitializeWeapon(this);        //给武器脚本攻击状态的索引
    }


    #region Animation Trigger
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;       //用于结束攻击动画进入闲置状态
    }

    #endregion
}
