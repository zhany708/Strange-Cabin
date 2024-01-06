using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Core core;
    protected Movement Movement
    {
        get
        {
            if (m_Movement) { return m_Movement; }      //检查组件是否为空
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

    protected Stats Stats
    {
        get
        {
            if (m_Stats) { return m_Stats; }
            m_Stats = core.GetCoreComponent<Stats>();
            return m_Stats;
        }
    }
    private Stats m_Stats;



    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected SO_EnemyData enemyData;

    protected AnimatorStateInfo animatorInfo;       //查询动画状态

    protected bool isHit = false;


    string m_AnimationBoolName;     //告诉动画器应该播放哪个动画


    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.enemyData = enemyData;
        m_AnimationBoolName = animBoolName;
        core = enemy.Core;
    }


    public virtual void Enter()
    {
        enemy.Core.Animator.SetBool(m_AnimationBoolName, true);     //播放状态的动画

        //Debug.Log(m_AnimationBoolName);
    }

    public virtual void LogicUpdate()
    {
        if (Combat.IsHit && !isHit)
        {
            stateMachine.ChangeState(enemy.HitState);
        }
    }

    public virtual void PhysicsUpdate() 
    {
        if (isHit)   //受击状态中
        {
            float amount = 0.3f;
            amount += Time.deltaTime;

            Movement.ReduceVelocity(amount);    //持续减少移动速度
        }

        else
        {
            Movement.SetVelocityZero();     //受击结束后使敌人停止移动，也防止玩家碰撞敌人后敌人持续移动
        }
    }

    public virtual void Exit()
    {
        enemy.Core.Animator.SetBool(m_AnimationBoolName, false);        //设置当前状态布尔为false以进入下个状态
    }
}
