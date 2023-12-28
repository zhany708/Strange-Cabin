using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;
    AnimatorStateInfo m_info;

    public AttackState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }




    public void OnEnter()
    {
        m_parameter.animator.SetTrigger("Attack");
    }


    public void OnUpdate()
    {
        m_info = m_parameter.animator.GetCurrentAnimatorStateInfo(0);       //��ȡ��ǰ����

        if (m_parameter.isHit)     //����Ƿ��ܻ�
        {
            m_manager.TransitionState(StateType.Hit);
        }

        else if (m_info.normalizedTime >= 0.95f)     //�����깥���������л���׷������
        {
            m_manager.TransitionState(StateType.Chase);
        }
    }


    public void OnExit()
    {

    }
}