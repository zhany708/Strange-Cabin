using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{

    EnemyFSM m_manager;
    Parameter m_parameter;

    Vector2 m_RandomPosition;       //随机坐标


    public PatrolState(EnemyFSM manager)
    {
        m_manager = manager;
        m_parameter = manager.parameter;

    }




    public void OnEnter()
    {
        //生成随机坐标
        m_RandomPosition = new Vector2(Random.Range(m_manager.getLeftDownPos().x, m_manager.getRightTopPos().x), Random.Range(m_manager.getLeftDownPos().y, m_manager.getRightTopPos().y));
    }


    public void OnUpdate()
    {
        m_manager.FaceTo(m_RandomPosition, m_manager.transform.position);      //朝向巡逻点的方向

        if (m_parameter.isHit)     //检测是否受击
        {
            m_manager.TransitionState(StateType.Hit);
        }

        else if (m_parameter.target != null && !m_manager.CheckOutside())
        {
            m_manager.TransitionState(StateType.Chase);     //巡逻时如果检测到玩家则切换为反应状态
        }


        //移动到目标点
        m_manager.transform.position = Vector2.MoveTowards(m_manager.transform.position, m_RandomPosition, m_parameter.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(m_manager.transform.position, m_RandomPosition) < 0.1f)     //当距离目标巡逻点足够近时
        {
            m_manager.TransitionState(StateType.Idle);
        }
    }


    public void OnExit()
    {

    }
}