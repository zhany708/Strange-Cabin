using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Core core;

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    //protected float startTime;    //用于检查每个状态的持续时间
    //protected bool isAnimationFinished;     //用于检查动画是否播放完毕
    protected bool isAttack;

    string m_AnimationBoolName;     //告诉动画器应该播放哪个动画


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
        player.Core.Animator.SetBool(m_AnimationBoolName, true);     //播放状态的动画

        //Debug.Log(m_AnimationBoolName);
    }

    public virtual void Exit()
    {
        player.Core.Animator.SetBool(m_AnimationBoolName, false);        //设置当前状态布尔为false以进入下个状态`` 
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
