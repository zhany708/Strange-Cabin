using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhangYu.Weapons;


public class PlayerAttackState : PlayerAbilityState
{
    Weapon m_Weapon;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, SO_PlayerData playerData, string animBoolName, Weapon weapon) : base(player, stateMachine, playerData, animBoolName)
    {
        m_Weapon = weapon;   //初始化武器
        m_Weapon.OnExit += ExitHandler;     //将此脚本的ExitHandler函数加进武器脚本的OnExit事件
    }

    public override void Enter()
    {
        base.Enter();

        m_Weapon.Enter();
    }

    private void ExitHandler()
    {
        AnimationFinishTrigger();

        isAbilityDone = true;
    }
}
