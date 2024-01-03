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

        if (!isAttack)
        {
            core.Movement.SetVelocity(playerData.MovementVelocity * input);        //��ֹ��ҹ���ʱ�����ƶ�
            core.Movement.SetAnimationDirection(core.Movement.FacingDirection, Vector2.zero);       //����Ҫ��ȥ��ҵĵ�ǰ���꣬��˵ڶ�������Ϊ0
        }
    }
}
