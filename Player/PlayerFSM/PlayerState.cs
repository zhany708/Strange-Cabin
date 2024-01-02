using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;    //���ڼ��ÿ��״̬�ĳ���ʱ��
    protected bool isAnimationFinished;     //���ڼ�鶯���Ƿ񲥷����

    string m_AnimationBoolName;     //���߶�����Ӧ�ò����ĸ�����


    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        m_AnimationBoolName = animBoolName;
    }


    public virtual void Enter()
    {
        DoChecks();
        player.Animator.SetBool(m_AnimationBoolName, true);     //����״̬�Ķ���
        startTime = Time.time;

        Debug.Log(m_AnimationBoolName);

        isAnimationFinished = false;
    }

    public virtual void Exit()
    {
        player.Animator.SetBool(m_AnimationBoolName, false);        //���õ�ǰ״̬����Ϊfalse�Խ����¸�״̬
    }

    public virtual void LogicUpdate() { }


    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }




    public virtual void DoChecks() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

}
