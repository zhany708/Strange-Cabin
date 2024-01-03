using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackState : PlayerAbilityState
{

    Weapon m_Weapon;

    float m_VelocityToSet;      //���ڹ����ڼ�����ƶ���ң�����ֻ��һ֡�ƶ���ң�
    bool m_SetVelocity;         //���ڼ���ʱ�����ƶ����

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        m_SetVelocity = false;

        isAttack = true;

        m_Weapon.EnterWeapon();
    }

    public override void Exit() 
    { 
         base.Exit();

        isAttack = false;

        m_Weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (m_SetVelocity)
        {
            core.Movement.SetVelocity(m_VelocityToSet * core.Movement.FacingDirection);      //�������ô˺����Գ����ƶ����
        }
    }





    public void SetWeapon(Weapon weapon)
    {
        m_Weapon = weapon;      //����������
        m_Weapon.InitializeWeapon(this);        //�������ű�����״̬������
    }

    public void SetPlayerVelocity(float velocity)       //���ڶ���֡�¼�
    {
        core.Movement.SetVelocity(velocity * core.Movement.FacingDirection);     //����ʱ��������ƶ��ٶ�

        m_VelocityToSet = velocity;
        m_SetVelocity = true;
    }

    #region Animation Trigger
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;       //���ڽ�������������������״̬
    }

    #endregion
}
