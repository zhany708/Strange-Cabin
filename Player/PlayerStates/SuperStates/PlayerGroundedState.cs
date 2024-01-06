using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{

    protected Vector2 input;


    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, SO_PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        input = player.InputHandler.RawMovementInput;   //ͨ��Player�ű���������״̬���ƶ�״̬��Ҫ��������ֵ


        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])        //����������ʱ�����ɵ�����������״̬
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }

        /*
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])     //��������Ҽ�ʱ�����ɵ�����������״̬
        {
            
            stateMachine.ChangeState(player.SecondaryAttackState);
        }  
        */    }
}
