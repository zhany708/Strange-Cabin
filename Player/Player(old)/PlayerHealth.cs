using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{

    //�������
    //public float health;

    Animator m_Animator;
    PlayerController m_PlayerController;


    /*
    public float HitSpeed;      //�����ܻ��ƶ��ٶ�

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


    
    public void PlayerGetHit(int damage, Vector2 direction)     //�ܻ�ʱ���ô˺���������Ϊ����ǰ���ķ���
    {
        m_HitDirection = direction;
        health -= damage;

        m_Animator.SetTrigger("Hit");
    }

    private void SetHitState()      //���ڶ���֡�¼�
    {
        m_IsHit = true;
    }




    private void DetectHit()
    {
        m_AnimationInfo = m_Animator.GetCurrentAnimatorStateInfo(0);    //��ȡ��������

        if (m_IsHit)
        {
            m_Position = (Vector2)m_Rigidbody2d.position + m_HitDirection * HitSpeed * Time.deltaTime;      //ʹ����򹥻������ƶ�
            m_Rigidbody2d.MovePosition(m_Position);

            if (m_AnimationInfo.normalizedTime >= 0.6f)     //0.6���ȡ���ܻ��ƶ�
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

    private void DestroyPlayer()    //���ڶ����¼�
    {
        Destroy(gameObject);
    }
}
