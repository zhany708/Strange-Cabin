using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;
    AnimatorStateInfo m_AnimatorInfo;       //��ѯ����״̬
    Transform m_Target;     

    public AttackState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.parameter;

    }




    public void OnEnter()
    {
        m_Target = m_Parameter.target;      //�������������Ϣ����ֹ�������ʱ��ʧ����
        m_Parameter.animator.SetTrigger("Attack");
        m_Manager.SetLastAttackTime(Time.time);     //���õ�ǰʱ��Ϊ�ϴι���ʱ��
    }


    public void OnUpdate()
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
                m_Manager.FireBallLaunch(m_Target);     
                m_Manager.TransitionState(StateType.Chase);
            }
        }
    }


    public void OnExit()
    {

    }
}