using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhangYu.Utilities;



public class Weapon : MonoBehaviour
{
    public event Action OnExit;      //接受事件方为PlayerAttackState脚本

    [SerializeField] private float m_AttackCounterResetCooldown;


    public SO_WeaponData WeaponData;


    public int CurrentAttackCounter
    {
        get => m_CurrentAttackCounter;
        private set => m_CurrentAttackCounter = value >= WeaponData.AmountOfAttack ? 0 : value;
    }
    private int m_CurrentAttackCounter;        //表示武器的连击次数




    protected GameObject baseGameObject;
    protected GameObject weaponGameObject;
    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected Core core;
    protected AnimationEventHandler baseAnimationEventHandler { get; private set; }
    protected WeaponHitboxToWeapon weaponAnimationEventHandler { get; private set; }

    protected Movement Movement => m_Movement ? m_Movement : core.GetCoreComponent(ref m_Movement);   //检查m_Movement是否为空，不是的话则返回它，是的话则调用GetCoreComponent函数以获取组件
    private Movement m_Movement;


    Timer m_AttackCounterResetTimer;


    protected virtual void Awake()
    {
        core = GetComponentInParent<Player>().GetComponentInChildren<Core>();   //先调用Player父物体，然后再从父物体中寻找Core子物体

        baseGameObject = transform.Find("Base").gameObject;
        baseAnimator = baseGameObject.GetComponent<Animator>();      //通过Find调用子物体上的动画器组件
        baseAnimationEventHandler = baseGameObject.GetComponent<AnimationEventHandler>();

        weaponGameObject = transform.Find("Weapon").gameObject;
        weaponAnimator = weaponGameObject.GetComponent<Animator>();
        weaponAnimationEventHandler = weaponGameObject.GetComponent<WeaponHitboxToWeapon>();

        m_AttackCounterResetTimer = new Timer(m_AttackCounterResetCooldown);
    }

    private void Update()
    {
        m_AttackCounterResetTimer.Tick();   //持续对计时器进行计时
    }

    protected virtual void OnEnable()
    {
        baseAnimationEventHandler.OnFinish += ExitWeapon;

        m_AttackCounterResetTimer.OnTimerDone += ResetAttackCounter;    //触发事件（计时器到达目标时间）时重置连击
    }

    protected virtual void OnDisable()
    {
        baseAnimationEventHandler.OnFinish -= ExitWeapon;

        m_AttackCounterResetTimer.OnTimerDone -= ResetAttackCounter;  
    }



    public virtual void EnterWeapon()
    {
        m_AttackCounterResetTimer.StopTimer();      //攻击状态中暂停计时器，防止攻击动画还没结束就重置连击

        //通过Core中的Facing Direction向量确定动画方向,然后设置攻击为True
        baseAnimator.SetFloat("MoveX", Movement.FacingDirection.x);
        baseAnimator.SetFloat("MoveY", Movement.FacingDirection.y);
        weaponAnimator.SetFloat("MoveX", Movement.FacingDirection.x);
        weaponAnimator.SetFloat("MoveY", Movement.FacingDirection.y);

        baseAnimator.SetBool("Attack", true);
        weaponAnimator.SetBool("Attack", true);

        baseAnimator.SetInteger("AttackCounter", CurrentAttackCounter);
        weaponAnimator.SetInteger("AttackCounter", CurrentAttackCounter);
    }


    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool("Attack", false);
        weaponAnimator.SetBool("Attack", false);

        CurrentAttackCounter++;    //增加攻击计数以让玩家进行二连击
        m_AttackCounterResetTimer.StartTimer();      //攻击结束后开始计时器

        OnExit?.Invoke();
    }



    private void ResetAttackCounter() => CurrentAttackCounter = 0;     //重置连击数
}