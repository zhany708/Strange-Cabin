using UnityEngine;


public class EnemyIdleState : EnemyState
{
    float m_IdleTimer;      //Ѳ�ߣ�ԭ��ͣ����ʱ��

    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();       //���ٶȹ���
    }

    public override void LogicUpdate()
    {
        m_IdleTimer += Time.deltaTime;

        base.LogicUpdate();

        if (enemy.Parameter.Target != null && !enemy.CheckOutside())
        {
            stateMachine.ChangeState(enemy.ChaseState);     //�����⵽������л�Ϊ׷��״̬������з�Ӧ�����������л�Ϊ��Ӧ״̬��Ȼ���ٷ�Ӧ����������л���׷��״̬��
        }


        else if (m_IdleTimer >= enemyData.IdleDuration)    //����Ƿ�ý���Ѳ��״̬
        {
            stateMachine.ChangeState(enemy.PatrolState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        m_IdleTimer = 0;
    }
}
