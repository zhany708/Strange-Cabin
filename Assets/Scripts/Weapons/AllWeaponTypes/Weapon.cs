using System;
using UnityEngine;
using ZhangYu.Utilities;




public class Weapon : MonoBehaviour
{
    public event Action OnExit;      //接受事件方为PlayerAttackState脚本


    public SO_WeaponData WeaponData;



    protected Animator animator;

    protected Core core;
    protected Player player;
    protected WeaponHitboxToWeapon weaponAnimationEventHandler { get; private set; }
    protected Flip weaponInventoryFlip;

    protected Movement Movement => m_Movement ? m_Movement : core.GetCoreComponent(ref m_Movement);   //检查m_Movement是否为空，不是的话则返回它，是的话则调用GetCoreComponent函数以获取组件
    private Movement m_Movement;









    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        player = GetComponentInParent<Player>();

        core = player.GetComponentInChildren<Core>();   //先调用Player父物体，然后再从父物体中寻找Core子物体


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
        Vector2 direction = (player.InputHandler.ProjectedMousePos - new Vector2(transform.parent.position.x, transform.parent.position.y) ).normalized;    //计算需要朝向鼠标的方向

        transform.parent.right = direction;   //更改武器库的朝向，而不是武器的
        weaponInventoryFlip.DoFlip(player.FacingNum);       //实时翻转武器，防止玩家翻转时武器也被翻转
    }
}