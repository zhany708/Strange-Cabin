using System;
using UnityEngine;




public class Weapon : MonoBehaviour
{
    public event Action OnExit;      //接受事件方为PlayerAttackState脚本


    public SO_WeaponData WeaponData;


    protected GameObject baseGameObject;
    protected GameObject weaponGameObject;
    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected Core core;
    protected AnimationEventHandler baseAnimationEventHandler { get; private set; }
    protected WeaponHitboxToWeapon weaponAnimationEventHandler { get; private set; }

    protected Movement Movement => m_Movement ? m_Movement : core.GetCoreComponent(ref m_Movement);   //检查m_Movement是否为空，不是的话则返回它，是的话则调用GetCoreComponent函数以获取组件
    private Movement m_Movement;




    protected virtual void Awake()
    {
        core = GetComponentInParent<Player>().GetComponentInChildren<Core>();   //先调用Player父物体，然后再从父物体中寻找Core子物体

        baseGameObject = transform.Find("Base").gameObject;
        baseAnimator = baseGameObject.GetComponent<Animator>();      //通过Find调用子物体上的动画器组件
        baseAnimationEventHandler = baseGameObject.GetComponent<AnimationEventHandler>();

        weaponGameObject = transform.Find("Weapon").gameObject;
        weaponAnimator = weaponGameObject.GetComponent<Animator>();
        weaponAnimationEventHandler = weaponGameObject.GetComponent<WeaponHitboxToWeapon>();
    }

    protected virtual void OnEnable()
    {
        baseAnimationEventHandler.OnFinish += ExitWeapon;
    }

    protected virtual void OnDisable()
    {
        baseAnimationEventHandler.OnFinish -= ExitWeapon;
    }



    public virtual void EnterWeapon()
    {
        //通过Core中的Facing Direction向量确定动画方向,然后设置攻击为True
        baseAnimator.SetFloat("MoveX", Movement.FacingDirection.x);
        baseAnimator.SetFloat("MoveY", Movement.FacingDirection.y);
        weaponAnimator.SetFloat("MoveX", Movement.FacingDirection.x);
        weaponAnimator.SetFloat("MoveY", Movement.FacingDirection.y);

        baseAnimator.SetBool("Attack", true);
        weaponAnimator.SetBool("Attack", true);
    }


    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool("Attack", false);
        weaponAnimator.SetBool("Attack", false);

        OnExit?.Invoke();
    }
}