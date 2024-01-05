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
    protected AnimatorStateInfo animatorInfo;       //��ѯ����״̬


    public HitState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.Parameter;
        m_Core = m_Manager.Core;
    }




    public void OnEnter()
    {
        //Debug.Log("HitState");
        m_Manager.SetCanHitFalse();     //ʹ�����޷����ظ�����

        m_Core.Animator.SetBool("Flying", false);
        m_Core.Animator.SetBool("Hit", true);
        m_Manager.SetLastHitTime(Time.time);     //���õ�ǰʱ��Ϊ�ϴ��ܻ�ʱ��
    }


    public void OnLogicUpdate()
    {
        animatorInfo = m_Core.Animator.GetCurrentAnimatorStateInfo(0);       //��ȡ��ǰ����

        if (m_Manager.Stats.GetCurrentHealth() <= 0 )
        {
            m_Manager.TransitionState(StateType.Death);
        }

        
        else if (animatorInfo.IsName("Hit") && animatorInfo.normalizedTime >= 0.95f)
        {
            m_Parameter.Target = GameObject.FindWithTag("Player").transform;        //Ѱ����Player��ǩ���������
            m_Manager.TransitionState(StateType.Chase);          
        }
    }

    public void OnPhysicsUpdate() { }


    public void OnExit()
    {
        m_Core.Animator.SetBool("Hit", false);

        m_Manager.Combat.SetIsHitFalse();

        m_Manager.SetCanHitTrue();      //ʹ���˿����ٴα�����
    }
}