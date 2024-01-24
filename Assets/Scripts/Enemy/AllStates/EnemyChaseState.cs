using UnityEngine;
using ZhangYu.Utilities;


public class EnemyChaseState : EnemyState
{
    float m_DistanceToPlayer;

    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        if (enemy.Parameter.Target != null)
        {
            enemy.EnemyFlip.FlipX( enemy.Movement.GetFlipNum(enemy.Parameter.Target.position, enemy.transform.position) );    //ʹ���ﳯ�����

            m_DistanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.Parameter.Target.position);       //�����������ҵľ���
        }

        base.LogicUpdate();

        if (enemy.Parameter.Target == null || enemy.CheckOutside())
        {
            stateMachine.ChangeState(enemy.IdleState);      //��ʧĿ����߳���׷����Χʱ�л�������״̬
        }

        //��⹥����Χ����һ������ΪԲ��λ�ã��ڶ���Ϊ�뾶��������ΪĿ��ͼ��.��Ҵ��ڹ�����Χ�ҹ��������������빥��״̬
        else if (Physics2D.OverlapCircle(enemy.Parameter.AttackPoint.position, enemyData.AttackArea, enemyData.TargetLayer) && enemy.CanAttack)
        {
            stateMachine.ChangeState(enemy.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemy.Parameter.Target && m_DistanceToPlayer > enemyData.StoppingDistance)     //���������������Ҿ��������С����ʱ����׷�����
        {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.Parameter.Target.position, enemyData.ChaseSpeed * Time.deltaTime);
        }
    }
}
