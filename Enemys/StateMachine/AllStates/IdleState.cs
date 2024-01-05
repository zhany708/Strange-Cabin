using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;
    Core m_Core;
    float m_IdleTimer;      //Ѳ�ߣ�ԭ��ͣ����ʱ��

    
    public IdleState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.Parameter;
        m_Core = m_Manager.Core;
    }
  



    public void OnEnter()
    {
        //Debug.Log("IdleState");
        m_Core.Animator.SetBool("Flying", true);
    }


    public void OnLogicUpdate()
    {
        m_IdleTimer += Time.deltaTime;

        if (m_Manager.Combat.IsHit && Time.time - m_Manager.GetLastHitTime() >= m_Parameter.HitInterval)     //����Ƿ��ܻ�
        {
            m_Manager.TransitionState(StateType.Hit);
        }
        
        else if (m_Parameter.Target != null && !m_Manager.CheckOutside())
        {
            m_Manager.TransitionState(StateType.Chase);     //Ѳ��ʱ�����⵽������л�Ϊ׷��״̬������з�Ӧ�����������л�Ϊ��Ӧ״̬��Ȼ���ٷ�Ӧ����������л���׷��״̬��
        }
        

        else if (m_IdleTimer >= m_Parameter.IdleDuration)    //����Ƿ�ý���Ѳ��״̬
        {
            m_Manager.TransitionState(StateType.Patrol);
        }
    }

    public void OnPhysicsUpdate() { }


    public void OnExit()
    {
        m_IdleTimer = 0;
    }
}