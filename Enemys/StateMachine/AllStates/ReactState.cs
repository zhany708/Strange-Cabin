using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;

    public ReactState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }

    public void OnEnter()
    {

    }


    public void OnUpdate()
    {

    }



    public void OnExit()
    {

    }
}