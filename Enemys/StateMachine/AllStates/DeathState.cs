using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;

    public DeathState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.parameter;

    }



    public void OnEnter()
    {
        m_Parameter.animator.SetBool("Death", true);
    }


    public void OnUpdate()
    {

    }


    public void OnExit()
    {

    }
}