using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, SO_PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (input.x == 0f && input.y == 0f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
        if (!isAttack)   //禁止玩家攻击时自由移动
        {
            Movement.SetVelocity(playerData.MovementVelocity, input);       //在变量后面加问号，表示只有变量不为空时才会调用变量的函数（不能用在Unity内部的变量上）
            Movement.SetAnimationDirection(Movement.FacingDirection, Vector2.zero);       //不需要减去玩家的当前坐标，因此第二个参数为0
        }

        else
        {
            Movement.SetVelocityZero();     //在移动状态中，玩家攻击时禁止移动
        }
    }
}
