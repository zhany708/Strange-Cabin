using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;
    float m_Timer;      //巡逻（原地停留）时间

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

        if (m_parameter.isHit)     //检测是否受击
        {
            m_manager.TransitionState(StateType.Hit);
        }
        
        else if (m_parameter.target != null && !m_manager.CheckOutside())
        {
            m_manager.TransitionState(StateType.Chase);     //巡逻时如果检测到玩家则切换为追击状态（如果有反应动画可以先切换为反应状态。然后再反应动画播完后切换成追击状态）
        }
        

        else if (m_Timer >= m_parameter.idleDuration)    //检测是否该进入巡逻状态
        {
            m_manager.TransitionState(StateType.Patrol);
        }
    }


    public void OnExit()
    {
        m_Timer = 0;
    }
}