using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;



    public ChaseState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }




    public void OnEnter()
    {

    }


    public void OnUpdate()
    {
        if (m_parameter.target != null)
        {
            m_manager.FaceTo(m_parameter.target.position, m_manager.transform.position);       //ʹ���ﳯ�����
        }

        if (m_parameter.isHit)     //����Ƿ��ܻ�
        {
            m_manager.TransitionState(StateType.Hit);
        }

        else if (Physics2D.OverlapCircle(m_parameter.attackPoint.position, m_parameter.attackArea, m_parameter.targetLayer))     //��⹥����Χ����һ������ΪԲ��λ�ã��ڶ���Ϊ�뾶��������ΪĿ��ͼ��
        {
            m_manager.TransitionState(StateType.Attack);
        }

        else if (m_parameter.target)     //���������ʱ����׷�����
        {
            m_manager.transform.position = Vector2.MoveTowards(m_manager.transform.position, m_parameter.target.position, m_parameter.chaseSpeed * Time.deltaTime);
        }

        else if (m_parameter.target == null || m_manager.CheckOutside())
        {
            m_manager.TransitionState(StateType.Idle);      //��ʧĿ����߳���׷����Χʱ�л�������״̬
        }
    }


    public void OnExit()
    {

    }
}