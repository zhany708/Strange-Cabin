using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;

    float m_DistanceToPlayer;


    public ChaseState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.parameter;

    }




    public void OnEnter()
    {
        //Debug.Log("ChaseState");
    }


    public void OnUpdate()
    {
        if (m_Parameter.target != null)
        {
            m_Manager.Core.Movement.SetAnimationDirection(m_Parameter.target.position, m_Manager.transform.position);       //使怪物朝向玩家

            m_DistanceToPlayer = Vector2.Distance(m_Manager.transform.position, m_Parameter.target.position);       //计算敌人与玩家的距离
        }

        if (m_Parameter.isHit && Time.time - m_Manager.GetLastHitTime() >= m_Parameter.hitInterval)     //检测是否受击
        {
            m_Manager.TransitionState(StateType.Hit);
        }

        else if (m_Parameter.target == null || m_Manager.CheckOutside())
        {
            m_Manager.TransitionState(StateType.Idle);      //丢失目标或者超出追击范围时切换到待机状态
        }

        //检测攻击范围：第一个参数为圆心位置，第二个为半径，第三个为目标图层.玩家处于攻击范围且攻击间隔结束则进入攻击状态
        else if (Physics2D.OverlapCircle(m_Parameter.attackPoint.position, m_Parameter.attackArea, m_Parameter.targetLayer) && Time.time - m_Manager.GetLastAttackTime() >= m_Parameter.attackInterval)
        {
            m_Manager.TransitionState(StateType.Attack);
        }

        else if (m_Parameter.target && m_DistanceToPlayer > m_Parameter.stoppingDistance)     //有玩家坐标且与玩家距离大于最小距离时持续追击玩家
        {
            m_Manager.transform.position = Vector2.MoveTowards(m_Manager.transform.position, m_Parameter.target.position, m_Parameter.chaseSpeed * Time.deltaTime);
        }
    }


    public void OnExit()
    {

    }
}