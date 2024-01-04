using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactState : IEnemyState
{

    EnemyFSM m_manager;
    Parameter m_parameter;

    public ReactState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.Parameter;

    }

    public void OnEnter()
    {

    }


    public void OnLogicUpdate()
    {

    }

    public void OnPhysicsUpdate()
    {

    }

    public void OnExit()
    {

    }
}