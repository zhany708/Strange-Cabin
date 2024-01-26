using UnityEngine;


public class EnemyState
{
    protected Core core;
    protected Movement enemyMovement
    {
        get
        {
            if (m_Movement) { return m_Movement; }      //�������Ƿ�Ϊ��
            m_Movement = core.GetCoreComponent<Movement>();
            return m_Movement;
        }
    }
    private Movement m_Movement;


    protected Combat enemyCombat
    {
        get
        {
            if (m_Combat) { return m_Combat; }
            m_Combat = core.GetCoreComponent<Combat>();
            return m_Combat;
        }
    }
    private Combat m_Combat;

    protected Stats enemyStats
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

    protected bool isHit = false;


    string m_AnimationBoolName;     //���߶�����Ӧ�ò����ĸ�����


    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.enemyData = enemyData;
        m_AnimationBoolName = animBoolName;
        core = enemy.Core;

        if (!core)
        {
            Debug.LogError("Core is missing in the EnemyState!");
        }
    }


    public virtual void Enter()
    {
        if (!enemyMovement || !enemyCombat || !enemyStats)
        {
            Debug.Log("Something is wrong in the EnemyState!");
        }


        core.Animator.SetBool(m_AnimationBoolName, true);     //����״̬�Ķ���
        
        //Debug.Log(m_AnimationBoolName);
    }

    public virtual void LogicUpdate()
    {
        if (enemyCombat.IsHit && !isHit)
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

            enemyMovement.ReduceVelocity(amount);    //���������ƶ��ٶ�
        }

        else
        {
            enemyMovement.SetVelocityZero();     //�ܻ�������ʹ����ֹͣ�ƶ���Ҳ��ֹ�����ײ���˺���˳����ƶ�
        }
    }

    public virtual void Exit()
    {
        enemy.Core.Animator.SetBool(m_AnimationBoolName, false);        //���õ�ǰ״̬����Ϊfalse�Խ����¸�״̬
    }
}
