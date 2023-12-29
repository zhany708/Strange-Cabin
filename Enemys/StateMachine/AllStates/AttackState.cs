using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;
    AnimatorStateInfo m_AnimatorInfo;       //查询动画状态
    Transform m_Target;     

    public AttackState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.parameter;

    }




    public void OnEnter()
    {
        m_Target = m_Parameter.target;      //储存玩家坐标信息，防止发射火球时丢失坐标
        m_Parameter.animator.SetTrigger("Attack");
        m_Manager.SetLastAttackTime(Time.time);     //设置当前时间为上次攻击时间
    }


    public void OnUpdate()
    {
        m_AnimatorInfo = m_Parameter.animator.GetCurrentAnimatorStateInfo(0);       //获取当前动画

        if (m_Parameter.isHit)     //检测是否受击
        {
            m_Manager.TransitionState(StateType.Hit);
        }

        if (m_AnimatorInfo.IsName("Attack"))
        {
            if (m_AnimatorInfo.normalizedTime >= 0.95f)     //播放完攻击动画则发射火球且切换成追击状态
            {
                m_Manager.FireBallLaunch(m_Target);     
                m_Manager.TransitionState(StateType.Chase);
            }
        }
    }


    public void OnExit()
    {

    }
}