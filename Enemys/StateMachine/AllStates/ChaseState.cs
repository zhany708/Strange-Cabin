using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;

    float m_DistanceToPlayer;


    public ChaseState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.parameter;

    }




    public void OnEnter()
    {
        //Debug.Log("ChaseState");
    }


    public void OnUpdate()
    {
        if (m_Parameter.target != null)
        {
            m_Manager.Core.Movement.SetAnimationDirection(m_Parameter.target.position, m_Manager.transform.position);       //ʹ���ﳯ�����

            m_DistanceToPlayer = Vector2.Distance(m_Manager.transform.position, m_Parameter.target.position);       //�����������ҵľ���
        }

        if (m_Parameter.isHit && Time.time - m_Manager.GetLastHitTime() >= m_Parameter.hitInterval)     //����Ƿ��ܻ�
        {
            m_Manager.TransitionState(StateType.Hit);
        }

        else if (m_Parameter.target == null || m_Manager.CheckOutside())
        {
            m_Manager.TransitionState(StateType.Idle);      //��ʧĿ����߳���׷����Χʱ�л�������״̬
        }

        //��⹥����Χ����һ������ΪԲ��λ�ã��ڶ���Ϊ�뾶��������ΪĿ��ͼ��.��Ҵ��ڹ�����Χ�ҹ��������������빥��״̬
        else if (Physics2D.OverlapCircle(m_Parameter.attackPoint.position, m_Parameter.attackArea, m_Parameter.targetLayer) && Time.time - m_Manager.GetLastAttackTime() >= m_Parameter.attackInterval)
        {
            m_Manager.TransitionState(StateType.Attack);
        }

        else if (m_Parameter.target && m_DistanceToPlayer > m_Parameter.stoppingDistance)     //���������������Ҿ��������С����ʱ����׷�����
        {
            m_Manager.transform.position = Vector2.MoveTowards(m_Manager.transform.position, m_Parameter.target.position, m_Parameter.chaseSpeed * Time.deltaTime);
        }
    }


    public void OnExit()
    {

    }
}