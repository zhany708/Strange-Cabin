using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class FireBatAttackStates : EnemyAttackState
{
    FireBats m_FireBats;
    Transform m_Target;

    public FireBatAttackStates(FireBats fireBat, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(fireBat, stateMachine, enemyData, animBoolName)
    {
        m_FireBats = fireBat;
    }


    public override void Enter()
    {
        //Debug.Log("FireBatAttackState");

        m_Target = enemy.Parameter.Target;      //�������������Ϣ����ֹ�������ʱ��ʧ����
        base.Enter();
    }

    public override void LogicUpdate()
    {
        animatorInfo = core.Animator.GetCurrentAnimatorStateInfo(0);       //��ȡ��ǰ����

        if (Combat.IsHit)     //����Ƿ��ܻ�
        {
            stateMachine.ChangeState(enemy.HitState);
        }

        if (animatorInfo.IsName("Attack"))
        {
            if (animatorInfo.normalizedTime >= 0.95f)     //�����깥����������������л���׷��״̬
            {
                m_FireBats.FireBallLaunch(m_Target);
                stateMachine.ChangeState(enemy.ChaseState);
            }
        }
    }
}
