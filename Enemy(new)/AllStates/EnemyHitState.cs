using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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
        animatorInfo = core.Animator.GetCurrentAnimatorStateInfo(0);       //��ȡ��ǰ����

        if (Stats.GetCurrentHealth() <= 0)
        {
            stateMachine.ChangeState(enemy.DeathState);
        }

        else if (animatorInfo.IsName("Hit"))
        {
            if (animatorInfo.normalizedTime >= 0.95f)
            {
                enemy.Parameter.Target = GameObject.FindWithTag("Player").transform;        //Ѱ����Player��ǩ���������
                stateMachine.ChangeState(enemy.ChaseState);
            }

            else if (animatorInfo.normalizedTime >= 0.5f)
            {
                Movement.SetVelocityZero();     //��������60%ʱֹͣ�ƶ�
            }        
        }
    }

    public override void Exit()
    {
        base.Exit();

        Combat.SetIsHitFalse();
        isHit = false;
    }
}
