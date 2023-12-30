using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class FireBatAttackState : AttackState
{
    FireBat m_FireBat;
    Transform m_Target;


    
    public FireBatAttackState(FireBat firebat) : base(firebat)      //构造函数（参数为FireBat脚本）
    {
        m_FireBat = firebat;
    }
    


    public override void OnEnter()
    {
        //Debug.Log("FireBatAttackState");

        m_Target = m_Parameter.target;      //储存玩家坐标信息，防止发射火球时丢失坐标
        base.OnEnter();
    }

    public override void OnUpdate()
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
                m_FireBat.FireBallLaunch(m_Target);
                m_Manager.TransitionState(StateType.Chase);
            }
        }
    }
}
