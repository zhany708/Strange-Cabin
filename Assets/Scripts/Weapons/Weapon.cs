using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhangYu.Utilities;



public class Weapon : MonoBehaviour
{
    public event Action OnExit;      //�����¼���ΪPlayerAttackState�ű�

    [SerializeField] private float m_AttackCounterResetCooldown;


    public SO_WeaponData WeaponData;


    public int CurrentAttackCounter
    {
        get => m_CurrentAttackCounter;
        private set => m_CurrentAttackCounter = value >= WeaponData.AmountOfAttack ? 0 : value;
    }
    private int m_CurrentAttackCounter;        //��ʾ��������������




    protected GameObject baseGameObject;
    protected GameObject weaponGameObject;
    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected Core core;
    protected AnimationEventHandler baseAnimationEventHandler { get; private set; }
    protected WeaponHitboxToWeapon weaponAnimationEventHandler { get; private set; }

    protected Movement Movement => m_Movement ? m_Movement : core.GetCoreComponent(ref m_Movement);   //���m_Movement�Ƿ�Ϊ�գ����ǵĻ��򷵻������ǵĻ������GetCoreComponent�����Ի�ȡ���
    private Movement m_Movement;


    Timer m_AttackCounterResetTimer;


    protected virtual void Awake()
    {
        core = GetComponentInParent<Player>().GetComponentInChildren<Core>();   //�ȵ���Player�����壬Ȼ���ٴӸ�������Ѱ��Core������

        baseGameObject = transform.Find("Base").gameObject;
        baseAnimator = baseGameObject.GetComponent<Animator>();      //ͨ��Find�����������ϵĶ��������
        baseAnimationEventHandler = baseGameObject.GetComponent<AnimationEventHandler>();

        weaponGameObject = transform.Find("Weapon").gameObject;
        weaponAnimator = weaponGameObject.GetComponent<Animator>();
        weaponAnimationEventHandler = weaponGameObject.GetComponent<WeaponHitboxToWeapon>();

        m_AttackCounterResetTimer = new Timer(m_AttackCounterResetCooldown);
    }

    private void Update()
    {
        m_AttackCounterResetTimer.Tick();   //�����Լ�ʱ�����м�ʱ
    }

    protected virtual void OnEnable()
    {
        baseAnimationEventHandler.OnFinish += ExitWeapon;

        m_AttackCounterResetTimer.OnTimerDone += ResetAttackCounter;    //�����¼�����ʱ������Ŀ��ʱ�䣩ʱ��������
    }

    protected virtual void OnDisable()
    {
        baseAnimationEventHandler.OnFinish -= ExitWeapon;

        m_AttackCounterResetTimer.OnTimerDone -= ResetAttackCounter;  
    }



    public virtual void EnterWeapon()
    {
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

        OnExit?.Invoke();
    }



    private void ResetAttackCounter() => CurrentAttackCounter = 0;     //����������
}