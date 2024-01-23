using System;
using UnityEngine;


public class WeaponHitboxToWeapon : MonoBehaviour       //用于将AggressiveWeapon脚本中的物理检测函数转交给Weapon脚本，同时负责不同武器的攻击动画效果
{
    public event Action OnStart;          //接收方为MeleeWeapon脚本
    public event Action OnFinish;         //接受事件方为Weapon脚本

    MeleeWeapon m_Weapon;

    private void Awake()
    {
        m_Weapon = GetComponent<MeleeWeapon>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Weapon.AddToDetected(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Weapon.RemoveFromDetected(collision);
    }



    private void AnimationActionTrigger() => OnStart?.Invoke();
    private void AnimationFinishTrigger() => OnFinish?.Invoke();
}
