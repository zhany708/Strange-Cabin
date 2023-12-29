using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;

    Vector2 m_RandomPosition;       //�������


    public PatrolState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }




    public void OnEnter()
    {
        //�����������
        m_RandomPosition = new Vector2(Random.Range(m_manager.getLeftDownPos().x, m_manager.getRightTopPos().x), Random.Range(m_manager.getLeftDownPos().y, m_manager.getRightTopPos().y));
    }


    public void OnUpdate()
    {
        m_manager.FaceTo(m_RandomPosition, m_manager.transform.position);      //����Ѳ�ߵ�ķ���

        if (m_parameter.isHit)     //����Ƿ��ܻ�
        {
            m_manager.TransitionState(StateType.Hit);
        }

        else if (m_parameter.target != null && !m_manager.CheckOutside())
        {
            m_manager.TransitionState(StateType.Chase);     //Ѳ��ʱ�����⵽������л�Ϊ��Ӧ״̬
        }


        //�ƶ���Ŀ���
        m_manager.transform.position = Vector2.MoveTowards(m_manager.transform.position, m_RandomPosition, m_parameter.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(m_manager.transform.position, m_RandomPosition) < 0.1f)     //������Ŀ��Ѳ�ߵ��㹻��ʱ
        {
            m_manager.TransitionState(StateType.Idle);
        }
    }


    public void OnExit()
    {

    }
}