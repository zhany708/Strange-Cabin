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
        m_Weapon = weapon;      //����������
        m_Weapon.InitializeWeapon(this);        //�������ű�����״̬������
    }


    #region Animation Trigger
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;       //���ڽ�������������������״̬
    }

    #endregion
}
