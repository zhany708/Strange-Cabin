using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackState : PlayerAbilityState
{

    Weapon m_Weapon;




    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, SO_PlayerData playerData, string animBoolName, Weapon weapon) : base(player, stateMachine, playerData, animBoolName)
    {
        m_Weapon = weapon;

        m_Weapon.OnExit += ExitHandler;
    }

    public override void Enter()
    {
        base.Enter();

        isAttack = true;

        m_Weapon.EnterWeapon();
    }
    



    private void ExitHandler()
    {
        AnimationFinishTrigger();

        isAttack = false;
        isAbilityDone = true;
    }
}