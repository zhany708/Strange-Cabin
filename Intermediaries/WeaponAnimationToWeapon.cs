using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationToWeapon : MonoBehaviour    //此脚本作为中转站，专门调用其他脚本中用于动画帧事件的函数
{
    Weapon m_Weapon;


    private void Start()
    {
        m_Weapon = GetComponentInParent<Weapon>();      //调用父物体中含有Weapon脚本的组件
    }

    private void AnimationFinishTrigger()
    {
        m_Weapon.AnimationFinishTrigger();      //调用Weapon脚本中的函数用于动画帧事件
    }

    private void AnimationStartMovementTrigger()
    {
        m_Weapon.AnimationStartMovementTrigger();
    }

    private void AnimationStopMovementTrigger()
    {
        m_Weapon.AnimationStopMovementTrigger();
    }

    private void AnimationActionTrigger()
    {
        m_Weapon.AnimationActionTrigger();
    }
}



