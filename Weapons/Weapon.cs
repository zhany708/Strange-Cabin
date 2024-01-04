using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Weapon : MonoBehaviour
{
    [SerializeField] protected SO_WeaponData weaponData;      //引用武器数值

    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected PlayerAttackState state;
    protected Player player;
    protected Core core;

    protected int attackCounter = 0;        //表示武器的连击次数


    protected virtual void Awake()
    {
        baseAnimator = transform.Find("Base").GetComponent<Animator>();       //通过Find调用子物体上的动画器组件
        weaponAnimator = transform.Find("Weapon").GetComponent <Animator>();    
        
        player = GetComponentInParent<Player>();        //调用父物件中含有Player脚本的物件
        
        gameObject.SetActive(false);        //不攻击时隐藏物件
    }



    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);     //攻击时显示物件

        if (attackCounter > weaponData.AmountOfAttack - 1)      //根据武器的最大连击次数重置攻击计数
        {
            attackCounter = 0;
        }


        //通过Core中的Facing Direction向量确定动画方向,然后设置攻击为True
        baseAnimator.SetFloat("MoveX", player.Core.Movement.FacingDirection.x);
        baseAnimator.SetFloat("MoveY", player.Core.Movement.FacingDirection.y);
        weaponAnimator.SetFloat("MoveX", player.Core.Movement.FacingDirection.x);
        weaponAnimator.SetFloat("MoveY", player.Core.Movement.FacingDirection.y);

        baseAnimator.SetBool("Attack", true);
        weaponAnimator.SetBool("Attack", true);

        baseAnimator.SetInteger("AttackCounter", attackCounter);
        weaponAnimator.SetInteger("AttackCounter", attackCounter);
    }


    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool("Attack", false);
        weaponAnimator.SetBool("Attack", false);

        attackCounter++;    //增加攻击计数以让玩家进行二连击

        gameObject.SetActive(false);        //攻击结束后再次隐藏物件
    }



    public void InitializeWeapon(PlayerAttackState state, Core core)       //引用攻击状态脚本
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
        state.SetPlayerVelocity(0f);        //结束攻击移动
    }

    public virtual void AnimationActionTrigger() { }

    #endregion
}

