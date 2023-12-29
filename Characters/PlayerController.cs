using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //�ƶ����
    public InputAction moveAction;
    public float moveSpeed = 2.5f;

    Vector2 m_Position;
    Vector2 m_Move;
    Rigidbody2D m_Rigidbody2d;


    //�������
    Animator m_Animator;
    Vector2 m_MoveDirection = new Vector2(1, 0);


    //�������
    public InputAction heavySwingAction;
    public InputAction quickSwingAction;
    public float quickSwingSpeed;    //�����ٶ�
    public float heavySwingSpeed;
    public int damage;

    bool m_IsAttack = false;
    CameraShake m_CameraShake;


    //�ܻ����
    public float HitSpeed;      //�����ܻ��ƶ��ٶ�

    AnimatorStateInfo m_AnimationInfo;
    Vector2 m_HitDirection;
    bool m_IsHit;


    //�������
    public float health;
    



    void Start()
    {
        moveAction.Enable();

        heavySwingAction.Enable();
        quickSwingAction.Enable();
        heavySwingAction.performed += HeavySwing;       //�ص���������
        quickSwingAction.performed += QuickSwing;

        m_Rigidbody2d = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        m_CameraShake = FindObjectOfType<CameraShake>();    //��ӵ��CameraShake�ű������

    }

    void Update()
    {
        SetAnimator();
        DetectDeath();
    }

    void FixedUpdate()
    {
        DetectHit();
        Move();
    }



    //�ƶ����
    private void Move()
    {
        //����Input system������ֵ
        m_Move = moveAction.ReadValue<Vector2>();

        if (!m_IsAttack)
        {
            //ͨ�������ƶ�����ֹ���ֶ���
            m_Position = (Vector2)m_Rigidbody2d.position + m_Move * moveSpeed * Time.deltaTime;     //��ɫ��ǰ�����λ�ü����ƶ���������ٶȺ�ʱ��
        }      
        
        else if (m_IsHit)
        {
            m_Position = (Vector2)m_Rigidbody2d.position + m_HitDirection * HitSpeed * Time.deltaTime;
        }

        else
        {
            float currentSpeed = m_Animator.GetFloat("SwingType") == 1 ? quickSwingSpeed : heavySwingSpeed;     //���ݶ�������ȷ����������
            m_Position = (Vector2)m_Rigidbody2d.position + m_MoveDirection * currentSpeed * Time.deltaTime;       //���ݹ�����ʽ�����ƶ���������ֹʱҲ����ͨ�������ƶ���
        }

        m_Rigidbody2d.MovePosition(m_Position);
    }



    //�������
    private void SetAnimator()      
    {
        if (!Mathf.Approximately(m_Move.x, 0f) || !Mathf.Approximately(m_Move.y, 0f))     //����ɫ�Ƿ��ƶ�
        {
            m_MoveDirection.Set(m_Move.x, m_Move.y);      //��ɫ�ƶ�ʱ������ɫ�ƶ�����������m_MoveDirection
            m_MoveDirection.Normalize();      //��������ֵ�ĳ�1����Ϊֻ��Ҫ����
        }

        m_Animator.SetFloat("Move X", m_MoveDirection.x);
        m_Animator.SetFloat("Move Y", m_MoveDirection.y);
        m_Animator.SetFloat("Speed", m_Move.magnitude);
    }



    //�������

    private void HeavySwing(InputAction.CallbackContext context)        //�ع���
    {
        if (!m_IsAttack)
        {
            m_IsAttack = true;
            m_Animator.SetTrigger("Swing");
            m_Animator.SetFloat("SwingType", 0f);
        }
    }

    private void QuickSwing(InputAction.CallbackContext context)        //�ṥ��
    {
        if (!m_IsAttack)
        {
            m_IsAttack = true;
            m_Animator.SetTrigger("Swing");
            m_Animator.SetFloat("SwingType", 1f);
        }
    }

    public void AttackOver()    //���ڶ����¼����ڲ�ͬ��֡����˺�����
    {
        m_IsAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D other)     //����Ƿ���е���
    {
        if (other.CompareTag("Enemy"))
        {
            //��������
            other.GetComponent<EnemyFSM>().TakeDamage(damage);
            other.GetComponent<EnemyFSM>().GetHit(m_MoveDirection);

            //�����
            float shakeIntensity = m_Animator.GetFloat("SwingType") == 1 ? 1.5f : 2f;
            float shakeDuration = m_Animator.GetFloat("SwingType") == 1 ? 0.3f : 0.5f;

            if (m_CameraShake != null)
            {
                m_CameraShake.ShakeCamera(shakeIntensity, shakeDuration);
            }
        }
    }



    //�ܻ����
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void GetHit(Vector2 direction)      //�ܻ�ʱ���ô˺���������Ϊ����ǰ���ķ���
    {
        //ʹ��Ҳ����泯�ܻ�����Ķ���
        m_Animator.SetFloat("Move X", -direction.x);
        m_Animator.SetFloat("Move Y", -direction.y);

        m_IsHit = true;
        m_HitDirection = direction;

        m_Animator.SetTrigger("Hit");
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

    private void DetectDeath()
    {
        if (health <= 0)
        {
            health = 0;
            m_Animator.SetBool("Death", true);
        }
    }
}