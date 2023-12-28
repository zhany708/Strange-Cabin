using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;

    public DeathState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }



    public void OnEnter()
    {
        m_parameter.animator.SetBool("Death", true);
    }


    public void OnUpdate()
    {

    }


    public void OnExit()
    {

    }
}