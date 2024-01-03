using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }


    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();       //将玩家速度归零
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (input.x != 0f || input.y != 0f)
        {
            stateMachine.ChangeState(player.MoveState);
        }
    }
}
