using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Core core;
    
    protected Movement Movement
    {
        get
        {
            if (m_Movement) { return m_Movement; }      //�������Ƿ�Ϊ��
            m_Movement = core.GetCoreComponent<Movement>();
            return m_Movement;
        }
    }
    private Movement m_Movement; 
    
    
    protected Combat Combat
    {
        get
        {
            if (m_Combat) { return m_Combat; }
            m_Combat = core.GetCoreComponent<Combat>();
            return m_Combat;
        }
    }
    private Combat m_Combat;
    


    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected SO_PlayerData playerData;

    //protected float startTime;    //���ڼ��ÿ��״̬�ĳ���ʱ��
    //protected bool isAnimationFinished = false;     //���ڼ�鶯���Ƿ񲥷����
    protected bool isAttack = false;
    protected bool isHit = false;


    string m_AnimationBoolName;     //���߶�����Ӧ�ò����ĸ�����


    public PlayerState(Player player, PlayerStateMachine stateMachine, SO_PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        m_AnimationBoolName = animBoolName;
        core = player.Core;
    }


    public virtual void Enter()
    {
        player.Core.Animator.SetBool(m_AnimationBoolName, true);     //����״̬�Ķ���

        //Debug.Log(m_AnimationBoolName);
    }

    public virtual void Exit()
    {
        player.Core.Animator.SetBool(m_AnimationBoolName, false);        //���õ�ǰ״̬����Ϊfalse�Խ����¸�״̬
    }

    public virtual void LogicUpdate() 
    { 
        if (Combat.IsHit && !isHit)
        {
            stateMachine.ChangeState(player.HitState);
        }
    }


    public virtual void PhysicsUpdate() { }
    
    public virtual void AnimationFinishTrigger() { }
}
