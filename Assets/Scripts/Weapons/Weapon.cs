using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Weapon : MonoBehaviour
{
    [SerializeField] protected SO_WeaponData weaponData;      //����������ֵ

    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected PlayerAttackState state;
    protected Core core;

    protected Movement Movement
    {
        get
        {
            if (m_Movement) { return m_Movement; }      //�������Ƿ�Ϊ��
            m_Movement = core.GetCoreComponent<Movement>();
            return m_Movement;
        }
    }
    private Movement m_Movement;


    protected int attackCounter = 0;        //��ʾ��������������


    protected virtual void Awake()
    {
        baseAnimator = transform.Find("Base").GetComponent<Animator>();       //ͨ��Find�����������ϵĶ��������
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

        gameObject.SetActive(false);        //������ʱ�������
    }



    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);     //����ʱ��ʾ���

        if (attackCounter > weaponData.AmountOfAttack - 1)      //������������������������ù�������
        {
            attackCounter = 0;
        }


        //ͨ��Core�е�Facing Direction����ȷ����������,Ȼ�����ù���ΪTrue
        baseAnimator.SetFloat("MoveX", Movement.FacingDirection.x);
        baseAnimator.SetFloat("MoveY", Movement.FacingDirection.y);
        weaponAnimator.SetFloat("MoveX", Movement.FacingDirection.x);
        weaponAnimator.SetFloat("MoveY", Movement.FacingDirection.y);

        baseAnimator.SetBool("Attack", true);
        weaponAnimator.SetBool("Attack", true);

        baseAnimator.SetInteger("AttackCounter", attackCounter);
        weaponAnimator.SetInteger("AttackCounter", attackCounter);
    }


    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool("Attack", false);
        weaponAnimator.SetBool("Attack", false);

        attackCounter++;    //���ӹ�������������ҽ��ж�����

        gameObject.SetActive(false);        //�����������ٴ��������
    }



    public void InitializeWeapon(PlayerAttackState state, Core core)       //���ù���״̬�ű�
    {
        this.state = state;
        this.core = core;
    }

    #region Animation Trigger

    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }

    public virtual void AnimationStartMovementTrigger()
    {
        state.SetPlayerVelocity(weaponData.movementSpeed[attackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        state.SetPlayerVelocity(0f);        //���������ƶ�
    }

    public virtual void AnimationActionTrigger() { }

    #endregion
}