using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        if (!isAttack)   //��ֹ��ҹ���ʱ�����ƶ�
        {
            core.Movement.SetVelocity(playerData.MovementVelocity, input);       
            core.Movement.SetAnimationDirection(core.Movement.FacingDirection, Vector2.zero);       //����Ҫ��ȥ��ҵĵ�ǰ���꣬��˵ڶ�������Ϊ0
        }
    }
}
