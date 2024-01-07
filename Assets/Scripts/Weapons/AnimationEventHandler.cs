using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationEventHandler : MonoBehaviour
{
    public event Action OnFinish;       //�����¼���ΪWeapon


    private AggressiveWeapon m_AggressiveWeapon;
    private Movement m_Movement;



    private void Awake()
    {
        m_Movement = GetComponentInParent<Player>().GetComponentInChildren<Movement>();   //�ȵ���Player�����壬Ȼ���ٴ�Player��Ѱ��Core������
        m_AggressiveWeapon = GetComponentInParent<AggressiveWeapon>();
    }



    private void AnimationStartMovementTrigger()
    {
        m_Movement.SetVelocity(m_AggressiveWeapon.WeaponData.MovementSpeed[m_AggressiveWeapon.CurrentAttackCounter], m_Movement.FacingDirection);       //ʹ��ҹ���ʱ����ƶ�����
    }

    private void AnimationStopMovementTrigger()
    {
        m_Movement.SetVelocityZero();
    }

    private void AnimationActionTrigger() => m_AggressiveWeapon.CheckMeleeAttack();

    private void AnimationFinishTrigger() => OnFinish?.Invoke();

}

