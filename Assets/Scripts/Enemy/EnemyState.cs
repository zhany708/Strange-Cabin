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

    protected AnimatorStateInfo animatorInfo;       //��ѯ����״̬

    protected bool isHit = false;


    string m_AnimationBoolName;     //���߶�����Ӧ�ò����ĸ�����


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
        enemy.Core.Animator.SetBool(m_AnimationBoolName, true);     //����״̬�Ķ���

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
        if (isHit)   //�ܻ�״̬��
        {
            float amount = 0.3f;
            amount += Time.deltaTime;

            Movement.ReduceVelocity(amount);    //���������ƶ��ٶ�
        }

        else
        {
            Movement.SetVelocityZero();     //�ܻ�������ʹ����ֹͣ�ƶ���Ҳ��ֹ�����ײ���˺���˳����ƶ�
        }
    }

    public virtual void Exit()
    {
        enemy.Core.Animator.SetBool(m_AnimationBoolName, false);        //���õ�ǰ״̬����Ϊfalse�Խ����¸�״̬
    }
}
