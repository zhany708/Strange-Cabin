using System;
using UnityEngine;
using ZhangYu.Utilities;




public class Weapon : MonoBehaviour
{
    public event Action OnExit;      //�����¼���ΪPlayerAttackState�ű�


    public SO_WeaponData WeaponData;



    protected Animator animator;

    protected Core core;
    protected Player player;
    protected WeaponHitboxToWeapon weaponAnimationEventHandler { get; private set; }
    protected Flip weaponInventoryFlip;

    protected Movement Movement => m_Movement ? m_Movement : core.GetCoreComponent(ref m_Movement);   //���m_Movement�Ƿ�Ϊ�գ����ǵĻ��򷵻������ǵĻ������GetCoreComponent�����Ի�ȡ���
    private Movement m_Movement;









    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        player = GetComponentInParent<Player>();

        core = player.GetComponentInChildren<Core>();   //�ȵ���Player�����壬Ȼ���ٴӸ�������Ѱ��Core������


        weaponAnimationEventHandler = GetComponent<WeaponHitboxToWeapon>();
    }

    private void Start()
    {
        weaponInventoryFlip = new Flip(transform.parent.transform);
    }

    protected virtual void Update()
    {
        PointToMouse();
    }


    protected virtual void OnEnable()
    {
        weaponAnimationEventHandler.OnFinish += ExitWeapon;
    }

    protected virtual void OnDisable()
    {
        weaponAnimationEventHandler.OnFinish -= ExitWeapon;
    }



    public virtual void EnterWeapon()
    {
        animator.SetBool("Attack", true);
    }


    public virtual void ExitWeapon()
    {
        animator.SetBool("Attack", false);

        OnExit?.Invoke();
    }




    protected void PointToMouse()
    {
        Vector2 direction = (player.InputHandler.ProjectedMousePos - new Vector2(transform.parent.position.x, transform.parent.position.y) ).normalized;    //������Ҫ�������ķ���

        transform.parent.right = direction;   //����������ĳ��򣬶�����������
        weaponInventoryFlip.DoFlip(player.FacingNum);       //ʵʱ��ת��������ֹ��ҷ�תʱ����Ҳ����ת
    }
}