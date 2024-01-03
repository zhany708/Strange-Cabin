using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationToWeapon : MonoBehaviour    //�˽ű���Ϊ��תվ��ר�ŵ��������ű������ڶ���֡�¼��ĺ���
{
    Weapon m_Weapon;


    private void Start()
    {
        m_Weapon = GetComponentInParent<Weapon>();      //���ø������к���Weapon�ű������
    }

    private void AnimationFinishTrigger()
    {
        m_Weapon.AnimationFinishTrigger();      //����Weapon�ű��еĺ������ڶ���֡�¼�
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



