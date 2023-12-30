using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class AttackState : IState
{

    protected EnemyFSM m_Manager;
    protected Parameter m_Parameter;
    protected AnimatorStateInfo m_AnimatorInfo;       //��ѯ����״̬


    public AttackState(EnemyFSM manager)
    {
        m_Manager = manager;
        m_Parameter = manager.parameter;
    }



    public virtual void OnEnter()
    {
        //Debug.Log("AttackState");

        m_Parameter.animator.SetTrigger("Attack");
        m_Manager.SetLastAttackTime(Time.time);     //���õ�ǰʱ��Ϊ�ϴι���ʱ��
    }


    public virtual void OnUpdate()
    {
        m_AnimatorInfo = m_Parameter.animator.GetCurrentAnimatorStateInfo(0);       //��ȡ��ǰ����

        if (m_Parameter.isHit && Time.time - m_Manager.GetLastHitTime() >= m_Parameter.hitInterval)     //����Ƿ��ܻ�
        {
            m_Manager.TransitionState(StateType.Hit);
        }

        if (m_AnimatorInfo.IsName("Attack") && m_AnimatorInfo.normalizedTime >= 0.95f)
        {                  
            m_Manager.TransitionState(StateType.Chase);           
        }
    }


    public void OnExit()
    {

    }
}