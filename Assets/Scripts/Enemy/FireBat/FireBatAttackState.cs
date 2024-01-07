using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class FireBatAttackState : EnemyAttackState
{
    FireBat m_FireBat;
    Transform m_Target;

    public FireBatAttackState(FireBat fireBat, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(fireBat, stateMachine, enemyData, animBoolName)
    {
        m_FireBat = fireBat;
    }


    public override void Enter()
    {
        //Debug.Log("FireBatAttackState");

        m_Target = enemy.Parameter.Target;      //储存玩家坐标信息，防止发射火球时丢失坐标
        base.Enter();
    }

    public override void LogicUpdate()
    {
        animatorInfo = core.Animator.GetCurrentAnimatorStateInfo(0);       //获取当前动画

        if (Combat.IsHit)     //检测是否受击
        {
            stateMachine.ChangeState(enemy.HitState);
        }

        if (animatorInfo.IsName("Attack"))
        {
            if (animatorInfo.normalizedTime >= 0.95f)     //播放完攻击动画则发射火球且切换成追击状态
            {
                m_FireBat.FireBallLaunch(m_Target);
                stateMachine.ChangeState(enemy.ChaseState);
            }
        }
    }
}
