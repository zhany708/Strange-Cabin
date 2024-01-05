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
        enemy.SetLastHitTime(Time.time);     //设置当前时间为上次受击时间
    }

    public override void LogicUpdate()
    {
        animatorInfo = core.Animator.GetCurrentAnimatorStateInfo(0);       //获取当前动画

        if (Stats.GetCurrentHealth() <= 0)
        {
            stateMachine.ChangeState(enemy.DeathState);
        }

        else if (animatorInfo.IsName("Hit"))
        {
            if (animatorInfo.normalizedTime >= 0.95f)
            {
                enemy.Parameter.Target = GameObject.FindWithTag("Player").transform;        //寻找有Player标签的物件坐标
                stateMachine.ChangeState(enemy.ChaseState);
            }

            else if (animatorInfo.normalizedTime >= 0.5f)
            {
                Movement.SetVelocityZero();     //动画播到60%时停止移动
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
