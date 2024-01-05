using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class FireBatAttackState : AttackState
{
    FireBat m_FireBat;
    Transform m_Target;


    
    public FireBatAttackState(FireBat firebat) : base(firebat)      //���캯��������ΪFireBat�ű���
    {
        m_FireBat = firebat;
    }
    


    public override void OnEnter()
    {
        //Debug.Log("FireBatAttackState");

        m_Target = parameter.Target;      //�������������Ϣ����ֹ�������ʱ��ʧ����
        base.OnEnter();
    }

    public override void OnLogicUpdate()
    {
        animatorInfo = core.Animator.GetCurrentAnimatorStateInfo(0);       //��ȡ��ǰ����

        if (manager.Combat.IsHit)     //����Ƿ��ܻ�
        {
            manager.TransitionState(StateType.Hit);
        }

        if (animatorInfo.IsName("Attack"))
        {
            if (animatorInfo.normalizedTime >= 0.95f)     //�����깥����������������л���׷��״̬
            {
                m_FireBat.FireBallLaunch(m_Target);
                manager.TransitionState(StateType.Chase);
            }
        }
    }
}
