using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;
    Core m_Core;
    float m_IdleTimer;      //巡逻（原地停留）时间

    
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

        if (m_Manager.Combat.IsHit && Time.time - m_Manager.GetLastHitTime() >= m_Parameter.HitInterval)     //检测是否受击
        {
            m_Manager.TransitionState(StateType.Hit);
        }
        
        else if (m_Parameter.Target != null && !m_Manager.CheckOutside())
        {
            m_Manager.TransitionState(StateType.Chase);     //巡逻时如果检测到玩家则切换为追击状态（如果有反应动画可以先切换为反应状态。然后再反应动画播完后切换成追击状态）
        }
        

        else if (m_IdleTimer >= m_Parameter.IdleDuration)    //检测是否该进入巡逻状态
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