using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhangYu.Utilities;



public class Weapon : MonoBehaviour
{
    public event Action OnExit;      //�����¼���ΪPlayerAttackState

    //[SerializeField] private SO_WeaponData weaponData;      //����������ֵ
    [SerializeField] private float m_AttackCounterResetCooldown;


    public SO_WeaponData WeaponData;

    public int CurrentAttackCounter
    {
        get => m_CurrentAttackCounter;
        private set => m_CurrentAttackCounter = value >= WeaponData.AmountOfAttack ? 0 : value;
    }
    private int m_CurrentAttackCounter;        //��ʾ��������������


    protected GameObject baseGameObject;
    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    //protected PlayerAttackState state;
    protected Core core;

    protected Movement Movement => m_Movement ? m_Movement : core.GetCoreComponent(ref m_Movement);
    private Movement m_Movement;


    AnimationEventHandler m_EventHandler;

    Timer m_AttackCounterResetTimer;


    protected virtual void Awake()
    {
        core = GetComponentInParent<Player>().GetComponentInChildren<Core>();   //�ȵ���Player�����壬Ȼ���ٴӸ�������Ѱ��Core������

        baseGameObject = transform.Find("Base").gameObject;
        baseAnimator = baseGameObject.GetComponent<Animator>();      //ͨ��Find�����������ϵĶ��������

        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();
        m_EventHandler = baseGameObject.GetComponent<AnimationEventHandler>();

        m_AttackCounterResetTimer = new Timer(m_AttackCounterResetCooldown);

        //gameObject.SetActive(false);        //����ʱ��ʾ���
    }

    private void Update()
    {
        m_AttackCounterResetTimer.Tick();   //�����Լ�ʱ�����м�ʱ
    }

    private void OnEnable()
    {
        m_EventHandler.OnFinish += ExitWeapon;

        m_AttackCounterResetTimer.OnTimerDone += ResetAttackCounter;    //�����¼�����ʱ������Ŀ��ʱ�䣩ʱ��������
    }

    private void OnDisable()
    {
        m_EventHandler.OnFinish -= ExitWeapon;

        m_AttackCounterResetTimer.OnTimerDone -= ResetAttackCounter;  
    }



    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);     //����ʱ��ʾ���

        m_AttackCounterResetTimer.StopTimer();      //����״̬����ͣ��ʱ������ֹ����������û��������������

        //ͨ��Core�е�Facing Direction����ȷ����������,Ȼ�����ù���ΪTrue
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

        CurrentAttackCounter++;    //���ӹ�������������ҽ��ж�����
        m_AttackCounterResetTimer.StartTimer();      //����������ʼ��ʱ��

        //gameObject.SetActive(false);        //�����������ٴ��������

        OnExit?.Invoke();
    }



    private void ResetAttackCounter() => CurrentAttackCounter = 0;     //����������

    /*
    public void InitializeWeapon(PlayerAttackState state, Core core)       //���ù���״̬�ű�
    {
        this.state = state;
        this.core = core;
    
    */

    /*
    #region Animation Trigger

    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }

    public virtual void AnimationStartMovementTrigger()
    {
        state.SetPlayerVelocity(weaponData.movementSpeed[CurrentAttackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        state.SetPlayerVelocity(0f);        //���������ƶ�
    }

    public virtual void AnimationActionTrigger() { }

    #endregion
    */
}