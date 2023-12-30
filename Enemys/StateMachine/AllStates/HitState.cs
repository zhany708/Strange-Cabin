using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class HitState : IState
{

    EnemyFSM m_Manager;
    Parameter m_Parameter;
    protected AnimatorStateInfo m_AnimatorInfo;       //��ѯ����״̬


    public HitState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.parameter;

    }




    public void OnEnter()
    {
        //Debug.Log("HitState");

        m_Parameter.isHit = true;

        m_Parameter.animator.SetTrigger("Hit");
        m_Manager.SetLastHitTime(Time.time);     //���õ�ǰʱ��Ϊ�ϴ��ܻ�ʱ��
    }


    public void OnUpdate()
    {
        m_AnimatorInfo = m_Parameter.animator.GetCurrentAnimatorStateInfo(0);       //��ȡ��ǰ����

        if (m_Parameter.health <= 0 )
        {
            m_Parameter.health = 0;
            m_Manager.TransitionState(StateType.Death);
        }

        
        else if (m_AnimatorInfo.IsName("Hit") && m_AnimatorInfo.normalizedTime >= 0.95f)
        {
            m_Parameter.target = GameObject.FindWithTag("Player").transform;        //Ѱ����Player��ǩ���������
            m_Manager.TransitionState(StateType.Chase);          
        }
    }


    public void OnExit()
    {
        m_Parameter.isHit = false;
    }
}