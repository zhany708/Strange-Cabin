using UnityEngine;


public class EnemyHitState : EnemyState
{
    public EnemyHitState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isHit = true;
        enemy.SetLastHitTime(Time.time);     //���õ�ǰʱ��Ϊ�ϴ��ܻ�ʱ��
    }

    public override void LogicUpdate()
    {
        if (enemyStats.GetCurrentHealth() == 0)
        {
            stateMachine.ChangeState(enemy.DeathState);
        }

        else if (core.AnimatorInfo.IsName("Hit"))
        {
            if (core.AnimatorInfo.normalizedTime >= 0.95f)
            {
                enemy.Parameter.Target = GameObject.FindWithTag("Player").transform;        //Ѱ����Player��ǩ���������
                stateMachine.ChangeState(enemy.ChaseState);
            }

            else if (core.AnimatorInfo.normalizedTime >= 0.5f)
            {
                enemyMovement.SetVelocityZero();     //��������50%ʱֹͣ�ƶ�
            }        
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemyCombat.SetIsHit(false);
        isHit = false;
    }
}
