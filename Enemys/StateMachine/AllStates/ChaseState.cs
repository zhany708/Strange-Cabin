using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;
    Core m_Core;

    float m_DistanceToPlayer;


    public ChaseState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.Parameter;
        m_Core = m_Manager.Core;
    }




    public void OnEnter()
    {
        //Debug.Log("ChaseState");
    }


    public void OnLogicUpdate()
    {
        if (m_Parameter.Target != null)
        {
            m_Core.Movement.SetAnimationDirection(m_Parameter.Target.position, m_Manager.transform.position);       //ʹ���ﳯ�����

            m_DistanceToPlayer = Vector2.Distance(m_Manager.transform.position, m_Parameter.Target.position);       //�����������ҵľ���
        }

        if (m_Core.Combat.IsHit && Time.time - m_Manager.GetLastHitTime() >= m_Parameter.HitInterval)     //����Ƿ��ܻ�
        {
            m_Manager.TransitionState(StateType.Hit);
        }

        else if (m_Parameter.Target == null || m_Manager.CheckOutside())
        {
            m_Manager.TransitionState(StateType.Idle);      //��ʧĿ����߳���׷����Χʱ�л�������״̬
        }

        //��⹥����Χ����һ������ΪԲ��λ�ã��ڶ���Ϊ�뾶��������ΪĿ��ͼ��.��Ҵ��ڹ�����Χ�ҹ��������������빥��״̬
        else if (Physics2D.OverlapCircle(m_Parameter.AttackPoint.position, m_Parameter.AttackArea, m_Parameter.TargetLayer) && Time.time - m_Manager.GetLastAttackTime() >= m_Parameter.AttackInterval)
        {
            m_Manager.TransitionState(StateType.Attack);
        }
    }


    public void OnPhysicsUpdate()
    {
        if (m_Parameter.Target && m_DistanceToPlayer > m_Parameter.StoppingDistance)     //���������������Ҿ��������С����ʱ����׷�����
        {
            m_Manager.transform.position = Vector2.MoveTowards(m_Manager.transform.position, m_Parameter.Target.position, m_Parameter.ChaseSpeed * Time.deltaTime);
        }
    }


    public void OnExit()
    {

    }
}