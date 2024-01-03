using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;

    Vector2 m_RandomPosition;       //�������
    float m_PatrolTimer;


    public PatrolState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.parameter;

    }




    public void OnEnter()
    {
        //Debug.Log("PatrolState");

        //�����������
        m_RandomPosition = new Vector2(Random.Range(m_Manager.GetLeftDownPos().x, m_Manager.GetRightTopPos().x), Random.Range(m_Manager.GetLeftDownPos().y, m_Manager.GetRightTopPos().y));
    }


    public void OnUpdate()
    {
        m_PatrolTimer += Time.deltaTime;
        m_Manager.Core.Movement.SetAnimationDirection(m_RandomPosition, m_Manager.transform.position);      //����Ѳ�ߵ�ķ���

        if (m_Parameter.isHit && Time.time - m_Manager.GetLastHitTime() >= m_Parameter.hitInterval)     //����Ƿ��ܻ�
        {
            m_Manager.TransitionState(StateType.Hit);
        }

        else if (m_Parameter.target != null && !m_Manager.CheckOutside())
        {
            m_Manager.TransitionState(StateType.Chase);     //Ѳ��ʱ�����⵽������л�Ϊ��Ӧ״̬
        }


        //�ƶ���Ŀ���
        m_Manager.transform.position = Vector2.MoveTowards(m_Manager.transform.position, m_RandomPosition, m_Parameter.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(m_Manager.transform.position, m_RandomPosition) < 0.1f)     //������Ŀ��Ѳ�ߵ��㹻��ʱ
        {
            m_Manager.TransitionState(StateType.Idle);
        }

        else if (m_PatrolTimer >= 5f)
        {
            m_Manager.TransitionState(StateType.Idle);      //���5��������û�е���Ѳ�ߵ㣨��ס�ˣ�����ǿ��ת��������״̬
        }
    }


    public void OnExit()
    {
        m_PatrolTimer = 0f;
    }
}