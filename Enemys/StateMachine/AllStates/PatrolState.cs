using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;
    Core m_Core;

    Vector2 m_RandomPosition;       //随机坐标
    float m_PatrolTimer;


    public PatrolState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.Parameter;
        m_Core = m_Manager.Core;
    }




    public void OnEnter()
    {
        //Debug.Log("PatrolState");
        m_Core.Animator.SetBool("Flying", true);

        //生成随机坐标
        m_RandomPosition = new Vector2(Random.Range(m_Manager.GetLeftDownPos().x, m_Manager.GetRightTopPos().x), Random.Range(m_Manager.GetLeftDownPos().y, m_Manager.GetRightTopPos().y));
    }


    public void OnLogicUpdate()
    {
        m_PatrolTimer += Time.deltaTime;
        m_Manager.Movement.SetAnimationDirection(m_RandomPosition, m_Manager.transform.position);      //朝向巡逻点的方向

        if (m_Manager.Combat.IsHit && Time.time - m_Manager.GetLastHitTime() >= m_Parameter.HitInterval)     //检测是否受击
        {
            m_Manager.TransitionState(StateType.Hit);
        }

        else if (m_Parameter.Target != null && !m_Manager.CheckOutside())
        {
            m_Manager.TransitionState(StateType.Chase);     //巡逻时如果检测到玩家则切换为追击状态
        }


        //移动到目标点
        //m_Manager.transform.position = Vector2.MoveTowards(m_Manager.transform.position, m_RandomPosition, m_Parameter.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(m_Manager.transform.position, m_RandomPosition) < 0.1f)     //当距离目标巡逻点足够近时
        {
            m_Manager.TransitionState(StateType.Idle);
        }

        else if (m_PatrolTimer >= 5f)
        {
            m_Manager.TransitionState(StateType.Idle);      //如果5秒后敌人仍没有到达巡逻点（卡住了），则强制转换成闲置状态
        }
    }

    public void OnPhysicsUpdate()
    {
        if (m_RandomPosition != null)
        {
            //移动到目标点
            m_Manager.transform.position = Vector2.MoveTowards(m_Manager.transform.position, m_RandomPosition, m_Parameter.MoveSpeed * Time.deltaTime);
        }
    }


    public void OnExit()
    {
        m_PatrolTimer = 0f;
    }
}