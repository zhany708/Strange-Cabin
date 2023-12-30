using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;
    float m_IdleTimer;      //Ѳ�ߣ�ԭ��ͣ����ʱ��

    
    public IdleState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.parameter;

    }
  



    public void OnEnter()
    {
        //Debug.Log("IdleState");
    }


    public void OnUpdate()
    {
        m_IdleTimer += Time.deltaTime;

        if (m_Parameter.isHit && Time.time - m_Manager.GetLastHitTime() >= m_Parameter.hitInterval)     //����Ƿ��ܻ�
        {
            m_Manager.TransitionState(StateType.Hit);
        }
        
        else if (m_Parameter.target != null && !m_Manager.CheckOutside())
        {
            m_Manager.TransitionState(StateType.Chase);     //Ѳ��ʱ�����⵽������л�Ϊ׷��״̬������з�Ӧ�����������л�Ϊ��Ӧ״̬��Ȼ���ٷ�Ӧ����������л���׷��״̬��
        }
        

        else if (m_IdleTimer >= m_Parameter.idleDuration)    //����Ƿ�ý���Ѳ��״̬
        {
            m_Manager.TransitionState(StateType.Patrol);
        }
    }


    public void OnExit()
    {
        m_IdleTimer = 0;
    }
}