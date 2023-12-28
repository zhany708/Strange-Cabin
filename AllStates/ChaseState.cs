using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;



    public ChaseState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }




    public void OnEnter()
    {

    }


    public void OnUpdate()
    {
        if (m_parameter.target != null)
        {
            m_manager.FaceTo(m_parameter.target.position, m_manager.transform.position);       //使怪物朝向玩家
        }

        if (m_parameter.isHit)     //检测是否受击
        {
            m_manager.TransitionState(StateType.Hit);
        }

        else if (Physics2D.OverlapCircle(m_parameter.attackPoint.position, m_parameter.attackArea, m_parameter.targetLayer))     //检测攻击范围：第一个参数为圆心位置，第二个为半径，第三个为目标图层
        {
            m_manager.TransitionState(StateType.Attack);
        }

        else if (m_parameter.target)     //有玩家坐标时持续追击玩家
        {
            m_manager.transform.position = Vector2.MoveTowards(m_manager.transform.position, m_parameter.target.position, m_parameter.chaseSpeed * Time.deltaTime);
        }

        else if (m_parameter.target == null || m_manager.CheckOutside())
        {
            m_manager.TransitionState(StateType.Idle);      //丢失目标或者超出追击范围时切换到待机状态
        }
    }


    public void OnExit()
    {

    }
}