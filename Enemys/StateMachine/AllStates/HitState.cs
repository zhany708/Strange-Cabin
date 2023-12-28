using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;


    public HitState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }




    public void OnEnter()
    {
        m_parameter.animator.SetTrigger("Hit");
    }


    public void OnUpdate()
    {
        if (m_parameter.health <= 0 )
        {
            m_manager.TransitionState(StateType.Death);
        }

        else
        {
            m_parameter.target = GameObject.FindWithTag("Player").transform;        //当未死亡时，受击后立刻进入追击状态
            m_manager.TransitionState(StateType.Chase);
        }
    }


    public void OnExit()
    {
        m_parameter.isHit = false;
    }
}