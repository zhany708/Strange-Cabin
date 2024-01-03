using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Core core;

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    //protected float startTime;    //���ڼ��ÿ��״̬�ĳ���ʱ��
    //protected bool isAnimationFinished;     //���ڼ�鶯���Ƿ񲥷����
    protected bool isAttack;

    string m_AnimationBoolName;     //���߶�����Ӧ�ò����ĸ�����


    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        m_AnimationBoolName = animBoolName;
        core = player.Core;
    }


    public virtual void Enter()
    {
        DoChecks();
        player.Core.Animator.SetBool(m_AnimationBoolName, true);     //����״̬�Ķ���

        //Debug.Log(m_AnimationBoolName);
    }

    public virtual void Exit()
    {
        player.Core.Animator.SetBool(m_AnimationBoolName, false);        //���õ�ǰ״̬����Ϊfalse�Խ����¸�״̬`` 
    }

    public virtual void LogicUpdate() { }


    public virtual void PhysicsUpdate()
    {
        //DoChecks();
    }




    public virtual void DoChecks() { }

    public virtual void AnimationFinishTrigger()
    {

    }
}
