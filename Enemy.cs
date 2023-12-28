using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public abstract class Enemy : MonoBehaviour
{
    //生命相关
    public int health;
    public int damage;


    //动画相关
    Animator m_Animator;
    AnimatorStateInfo m_AnimationInfo;


    //受击相关
    public float HitSpeed;      //受击移动速度

    Vector2 m_HitDirection;
    Vector2 m_Position;
    Rigidbody2D m_Rigidbody2d;
    bool m_IsHit;

    

    protected void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody2d = GetComponent<Rigidbody2D>();
    }

    
    protected void Update()
    {
        DetectDeath(); 
    }

    protected void FixedUpdate()
    {
        DetectHit();
    }



    //受击相关
    public void TakeDamage(int damage)      //受击时调用此函数
    {
        health -= damage;
    }

    public void GetHit(Vector2 direction)      //受击时调用此函数，参数为玩家攻击时面对的方向
    {
        //使怪物播放朝向玩家的反方向的动画
        m_Animator.SetFloat("Move X", -direction.x);    
        m_Animator.SetFloat("Move Y", -direction.y);

        m_IsHit = true;
        this.m_HitDirection = direction;    

        m_Animator.SetTrigger("Hit");
    }

    protected void DetectHit()
    {
        m_AnimationInfo = m_Animator.GetCurrentAnimatorStateInfo(0);    //获取动画进度

        if (m_IsHit)
        {
            m_Position = (Vector2)m_Rigidbody2d.position + m_HitDirection * HitSpeed * Time.deltaTime;      //使怪物向攻击方向移动
            m_Rigidbody2d.MovePosition(m_Position);

            if (m_AnimationInfo.normalizedTime >= 0.6f)     //0.6秒后取消受击移动
            {
                m_IsHit = false;
            }
        }
    }

    protected void DetectDeath()
    {
        if (health <= 0)
        {
            health = 0;
            m_Animator.SetBool("Death", true);
        }
    }

    protected void DestroyAfterDeath()      //动画事件，摧毁物体
    {
        Destroy(gameObject);
    }
}