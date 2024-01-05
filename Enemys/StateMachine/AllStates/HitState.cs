using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HitState : IEnemyState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;
    Core m_Core;
    protected AnimatorStateInfo animatorInfo;       //查询动画状态


    public HitState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.Parameter;
        m_Core = m_Manager.Core;
    }




    public void OnEnter()
    {
        //Debug.Log("HitState");
        m_Manager.SetCanHitFalse();     //使敌人无法被重复攻击

        m_Core.Animator.SetBool("Flying", false);
        m_Core.Animator.SetBool("Hit", true);
        m_Manager.SetLastHitTime(Time.time);     //设置当前时间为上次受击时间
    }


    public void OnLogicUpdate()
    {
        animatorInfo = m_Core.Animator.GetCurrentAnimatorStateInfo(0);       //获取当前动画

        if (m_Manager.Stats.GetCurrentHealth() <= 0 )
        {
            m_Manager.TransitionState(StateType.Death);
        }

        
        else if (animatorInfo.IsName("Hit") && animatorInfo.normalizedTime >= 0.95f)
        {
            m_Parameter.Target = GameObject.FindWithTag("Player").transform;        //寻找有Player标签的物件坐标
            m_Manager.TransitionState(StateType.Chase);          
        }
    }

    public void OnPhysicsUpdate() { }


    public void OnExit()
    {
        m_Core.Animator.SetBool("Hit", false);

        m_Manager.Combat.SetIsHitFalse();

        m_Manager.SetCanHitTrue();      //使敌人可以再次被击中
    }
}