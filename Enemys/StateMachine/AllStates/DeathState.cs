using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IEnemyState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;
    Core m_Core;

    public DeathState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.Parameter;
        m_Core = m_Manager.Core;
    }



    public void OnEnter()
    {
        //m_Parameter.animator.SetBool("Death", true);
        m_Core.Animator.SetBool("Death", true) ;
    }


    public void OnLogicUpdate() { }
 

    public void OnPhysicsUpdate() { }


    public void OnExit() { }

}