using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetLastAttackTime(Time.time);     //���õ�ǰʱ��Ϊ�ϴι���ʱ��
    }

    public override void LogicUpdate()
    {
        animatorInfo = core.Animator.GetCurrentAnimatorStateInfo(0);       //��ȡ��ǰ����

        base.LogicUpdate();

        if (animatorInfo.IsName("Attack") && animatorInfo.normalizedTime >= 0.95f)
        {
            stateMachine.ChangeState(enemy.ChaseState);
        }
    }
}
