using System;
using UnityEngine;




public class Weapon : MonoBehaviour
{
    public event Action OnExit;      //�����¼���ΪPlayerAttackState�ű�


    public SO_WeaponData WeaponData;


    protected GameObject baseGameObject;
    protected GameObject weaponGameObject;
    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected Core core;
    protected AnimationEventHandler baseAnimationEventHandler { get; private set; }
    protected WeaponHitboxToWeapon weaponAnimationEventHandler { get; private set; }

    protected Movement Movement => m_Movement ? m_Movement : core.GetCoreComponent(ref m_Movement);   //���m_Movement�Ƿ�Ϊ�գ����ǵĻ��򷵻������ǵĻ������GetCoreComponent�����Ի�ȡ���
    private Movement m_Movement;




    protected virtual void Awake()
    {
        core = GetComponentInParent<Player>().GetComponentInChildren<Core>();   //�ȵ���Player�����壬Ȼ���ٴӸ�������Ѱ��Core������

        baseGameObject = transform.Find("Base").gameObject;
        baseAnimator = baseGameObject.GetComponent<Animator>();      //ͨ��Find�����������ϵĶ��������
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
        //ͨ��Core�е�Facing Direction����ȷ����������,Ȼ�����ù���ΪTrue
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