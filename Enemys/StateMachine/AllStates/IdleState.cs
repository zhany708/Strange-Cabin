using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;
    float m_Timer;      //Ѳ�ߣ�ԭ��ͣ����ʱ��

    public IdleState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }




    public void OnEnter()
    {

    }


    public void OnUpdate()
    {
        m_Timer += Time.deltaTime;

        if (m_parameter.isHit)     //����Ƿ��ܻ�
        {
            m_manager.TransitionState(StateType.Hit);
        }
        
        else if (m_parameter.target != null && !m_manager.CheckOutside())
        {
            m_manager.TransitionState(StateType.Chase);     //Ѳ��ʱ�����⵽������л�Ϊ׷��״̬������з�Ӧ�����������л�Ϊ��Ӧ״̬��Ȼ���ٷ�Ӧ����������л���׷��״̬��
        }
        

        else if (m_Timer >= m_parameter.idleDuration)    //����Ƿ�ý���Ѳ��״̬
        {
            m_manager.TransitionState(StateType.Patrol);
        }
    }


    public void OnExit()
    {
        m_Timer = 0;
    }
}