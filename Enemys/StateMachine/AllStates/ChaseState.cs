using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;

    float m_DistanceToPlayer;


    public ChaseState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }




    public void OnEnter()
    {
        //Debug.Log("ChaseState");
    }


    public void OnUpdate()
    {
        if (m_parameter.target != null)
        {
            m_manager.FaceTo(m_parameter.target.position, m_manager.transform.position);       //ʹ���ﳯ�����

            m_DistanceToPlayer = Vector2.Distance(m_manager.transform.position, m_parameter.target.position);       //�����������ҵľ���
        }

        if (m_parameter.isHit)     //����Ƿ��ܻ�
        {
            m_manager.TransitionState(StateType.Hit);
        }

        else if (m_parameter.target == null || m_manager.CheckOutside())
        {
            m_manager.TransitionState(StateType.Idle);      //��ʧĿ����߳���׷����Χʱ�л�������״̬
        }

        //��⹥����Χ����һ������ΪԲ��λ�ã��ڶ���Ϊ�뾶��������ΪĿ��ͼ��.��Ҵ��ڹ�����Χ�ҹ��������������빥��״̬
        else if (Physics2D.OverlapCircle(m_parameter.attackPoint.position, m_parameter.attackArea, m_parameter.targetLayer) && Time.time - m_manager.getLastAttackTime() >= m_parameter.attackInterval)
        {
            m_manager.TransitionState(StateType.Attack);
        }

        else if (m_parameter.target && m_DistanceToPlayer > m_parameter.stoppingDistance)     //���������������Ҿ��������С����ʱ����׷�����
        {
            m_manager.transform.position = Vector2.MoveTowards(m_manager.transform.position, m_parameter.target.position, m_parameter.chaseSpeed * Time.deltaTime);
        }

        else
        {
            m_manager.TransitionState(StateType.Idle);      //��else��ֹBug
        }
    }


    public void OnExit()
    {

    }
}