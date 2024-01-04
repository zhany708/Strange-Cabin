using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerHitState : PlayerGroundedState
{
    AnimatorStateInfo m_AnimatorStateInfo;

    public PlayerHitState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isHit = true;
        playerData.CurrentHealth -= core.Combat.DamageAmount;
    }

    public override void Exit()
    {
        base.Exit();

        core.Combat.SetIsHitFalse();
        isHit = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        m_AnimatorStateInfo = core.Animator.GetCurrentAnimatorStateInfo(0);       //获取当前动画


        if (playerData.CurrentHealth <= 0)
        {
            playerData.CurrentHealth = 0;
        }
     
        else if (m_AnimatorStateInfo.IsName("Hit") && m_AnimatorStateInfo.normalizedTime >= 0.95f)
        {
            stateMachine.ChangeState(player.IdleState);     //受击动画结束后切换成闲置状态
        }
    }
}
