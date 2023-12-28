using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public abstract class Enemy : MonoBehaviour
{
    //�������
    public int health;
    public int damage;


    //�������
    Animator m_Animator;
    AnimatorStateInfo m_AnimationInfo;


    //�ܻ����
    public float HitSpeed;      //�ܻ��ƶ��ٶ�

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



    //�ܻ����
    public void TakeDamage(int damage)      //�ܻ�ʱ���ô˺���
    {
        health -= damage;
    }

    public void GetHit(Vector2 direction)      //�ܻ�ʱ���ô˺���������Ϊ��ҹ���ʱ��Եķ���
    {
        //ʹ���ﲥ�ų�����ҵķ�����Ķ���
        m_Animator.SetFloat("Move X", -direction.x);    
        m_Animator.SetFloat("Move Y", -direction.y);

        m_IsHit = true;
        this.m_HitDirection = direction;    

        m_Animator.SetTrigger("Hit");
    }

    protected void DetectHit()
    {
        m_AnimationInfo = m_Animator.GetCurrentAnimatorStateInfo(0);    //��ȡ��������

        if (m_IsHit)
        {
            m_Position = (Vector2)m_Rigidbody2d.position + m_HitDirection * HitSpeed * Time.deltaTime;      //ʹ�����򹥻������ƶ�
            m_Rigidbody2d.MovePosition(m_Position);

            if (m_AnimationInfo.normalizedTime >= 0.6f)     //0.6���ȡ���ܻ��ƶ�
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

    protected void DestroyAfterDeath()      //�����¼����ݻ�����
    {
        Destroy(gameObject);
    }
}