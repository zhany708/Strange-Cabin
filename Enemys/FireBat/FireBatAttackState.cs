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

        m_Target = m_Parameter.target;      //�������������Ϣ����ֹ�������ʱ��ʧ����
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        m_AnimatorInfo = m_Parameter.animator.GetCurrentAnimatorStateInfo(0);       //��ȡ��ǰ����

        if (m_Parameter.isHit)     //����Ƿ��ܻ�
        {
            m_Manager.TransitionState(StateType.Hit);
        }

        if (m_AnimatorInfo.IsName("Attack"))
        {
            if (m_AnimatorInfo.normalizedTime >= 0.95f)     //�����깥����������������л���׷��״̬
            {
                m_FireBat.FireBallLaunch(m_Target);
                m_Manager.TransitionState(StateType.Chase);
            }
        }
    }
}