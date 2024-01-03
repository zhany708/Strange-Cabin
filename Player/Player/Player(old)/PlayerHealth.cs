using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{

    //生命相关
    //public float health;

    Animator m_Animator;
    PlayerController m_PlayerController;


    /*
    public float HitSpeed;      //人物受击移动速度

    protected AnimatorStateInfo m_AnimationInfo;
    public Vector2 m_HitDirection;
    public bool m_IsHit = false;
    */


    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_PlayerController = GetComponent<PlayerController>();
    }


    void Update()
    {
        DetectDeath();
    }

    /*
    void FixedUpdate()
    {
        DetectHit();
    }


    
    public void PlayerGetHit(int damage, Vector2 direction)     //受击时调用此函数，参数为火球前进的方向
    {
        m_HitDirection = direction;
        health -= damage;

        m_Animator.SetTrigger("Hit");
    }

    private void SetHitState()      //用于动画帧事件
    {
        m_IsHit = true;
    }




    private void DetectHit()
    {
        m_AnimationInfo = m_Animator.GetCurrentAnimatorStateInfo(0);    //获取动画进度

        if (m_IsHit)
        {
            m_Position = (Vector2)m_Rigidbody2d.position + m_HitDirection * HitSpeed * Time.deltaTime;      //使玩家向攻击方向移动
            m_Rigidbody2d.MovePosition(m_Position);

            if (m_AnimationInfo.normalizedTime >= 0.6f)     //0.6秒后取消受击移动
            {
                m_IsHit = false;
            }
        }
    }
    */
    private void DetectDeath()
    {
        if (m_PlayerController.health <= 0)
        {
            m_PlayerController.health = 0;
            m_Animator.SetBool("Death", true);
        }
    }

    private void DestroyPlayer()    //用于动画事件
    {
        Destroy(gameObject);
    }
}
