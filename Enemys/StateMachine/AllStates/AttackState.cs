using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class AttackState : IEnemyState
{

    protected EnemyFSM manager;
    protected Parameter parameter;
    protected Core core;
    protected AnimatorStateInfo animatorInfo;       //��ѯ����״̬


    public AttackState(EnemyFSM manager)
    {
        this.manager = manager;
        parameter = manager.Parameter;
        core = this.manager.Core;
    }



    public virtual void OnEnter()
    {
        //Debug.Log("AttackState");

        core.Animator.SetTrigger("Attack");
        manager.SetLastAttackTime(Time.time);     //���õ�ǰʱ��Ϊ�ϴι���ʱ��
    }


    public virtual void OnLogicUpdate()
    {
        animatorInfo = core.Animator.GetCurrentAnimatorStateInfo(0);       //��ȡ��ǰ����

        if (core.Combat.IsHit && Time.time - manager.GetLastHitTime() >= parameter.HitInterval)     //����Ƿ��ܻ�
        {
            manager.TransitionState(StateType.Hit);
        }

        if (animatorInfo.IsName("Attack") && animatorInfo.normalizedTime >= 0.95f)
        {                  
            manager.TransitionState(StateType.Chase);           
        }
    }

    public void OnPhysicsUpdate() { }
 

    public void OnExit() { }
}