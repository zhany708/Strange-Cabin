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

        enemy.AttackTimer.StopTimer();     //攻击期间停止计时
    }

    public override void LogicUpdate()
    {
        animatorInfo = core.Animator.GetCurrentAnimatorStateInfo(0);       //获取当前动画

        base.LogicUpdate();

        if (animatorInfo.IsName("Attack") && animatorInfo.normalizedTime >= 0.95f)
        {
            stateMachine.ChangeState(enemy.ChaseState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.SetCanAttackFalse();
        enemy.AttackTimer.StartTimer();     //攻击结束后开始计时
    }
}
