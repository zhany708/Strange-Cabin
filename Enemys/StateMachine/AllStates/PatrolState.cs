using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;
    int m_PatrolPosition;   //���ڲ��Һ��л�Ѳ�ߵ�

    public PatrolState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }




    public void OnEnter()
    {

    }


    public void OnUpdate()
    {
        m_manager.FaceTo(m_parameter.patrolPoints[m_PatrolPosition].position, m_manager.transform.position);      //����Ѳ�ߵ�ķ���

        if (m_parameter.isHit)     //����Ƿ��ܻ�
        {
            m_manager.TransitionState(StateType.Hit);
        }

        else if (m_parameter.target != null && !m_manager.CheckOutside())
        {
            m_manager.TransitionState(StateType.Chase);     //Ѳ��ʱ�����⵽������л�Ϊ��Ӧ״̬
        }
        

        //�ƶ���Ŀ���
        m_manager.transform.position = Vector2.MoveTowards(m_manager.transform.position, m_parameter.patrolPoints[m_PatrolPosition].position, m_parameter.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(m_manager.transform.position, m_parameter.patrolPoints[m_PatrolPosition].position) < 0.1f)     //������Ŀ��Ѳ�ߵ��㹻��ʱ
        {
            m_manager.TransitionState(StateType.Idle);
        }
    }


    public void OnExit()
    {
        m_PatrolPosition++;

        if (m_PatrolPosition >= m_parameter.patrolPoints.Length)      //���Ѳ�ߵ㳬����Χ���ͷѲ��
        {
            m_PatrolPosition = 0;
        }
    }
}