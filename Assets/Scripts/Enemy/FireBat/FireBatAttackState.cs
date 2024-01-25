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

        m_Target = enemy.Parameter.Target;      //�������������Ϣ����ֹ�������ʱ��ʧ����
        base.Enter();
    }

    public override void LogicUpdate()
    {
        /*
        if (Combat.IsHit)     //����Ƿ��ܻ�
        {
            stateMachine.ChangeState(enemy.HitState);
        }
        */

        if (core.AnimatorInfo.IsName("Attack") && core.AnimatorInfo.normalizedTime >= 0.95f)     //�����깥����������������л���׷��״̬
        {
            m_FireBat.FireBallLaunch(m_Target);
            stateMachine.ChangeState(enemy.ChaseState);
        }
    }
}
