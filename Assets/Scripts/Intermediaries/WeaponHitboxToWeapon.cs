using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WeaponHitboxToWeapon : MonoBehaviour       //用于将AggressiveWeapon脚本中的物理检测函数转交给Weapon脚本，同时负责不同武器的攻击动画效果
{
    public event Action OnStartMovement;  //接收方为AggressiveWeapon脚本
    public event Action OnStopMovement;   //接收方为AggressiveWeapon脚本


    AggressiveWeapon m_Weapon;

    private void Awake()
    {
        m_Weapon = GetComponentInParent<AggressiveWeapon>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Weapon.AddToDetected(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Weapon.RemoveFromDetected(collision);
    }



    private void AnimationStartMovementTrigger() => OnStartMovement?.Invoke();
    private void AnimationStopMovementTrigger() => OnStopMovement?.Invoke();
}
