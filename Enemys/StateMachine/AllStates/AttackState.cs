using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class AttackState : IEnemyState
{

    protected EnemyFSM manager;
    protected Parameter parameter;
    protected Core core;
    protected AnimatorStateInfo animatorInfo;       //查询动画状态


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
        manager.SetLastAttackTime(Time.time);     //设置当前时间为上次攻击时间
    }


    public virtual void OnLogicUpdate()
    {
        animatorInfo = core.Animator.GetCurrentAnimatorStateInfo(0);       //获取当前动画

        if (core.Combat.IsHit && Time.time - manager.GetLastHitTime() >= parameter.HitInterval)     //检测是否受击
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