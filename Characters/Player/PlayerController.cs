using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //�ƶ����
    public InputAction moveAction;
    public float moveSpeed = 2.5f;

    protected Vector2 m_Position;
    Vector2 m_Move;
    protected Rigidbody2D m_Rigidbody2d;


    //�������
    protected Animator m_Animator;
    protected Vector2 m_MoveDirection = new Vector2(1, 0);


    //�������
    public InputAction heavySwingAction;
    public InputAction quickSwingAction;
    public float quickSwingSpeed;    //�����ٶ�
    public float heavySwingSpeed;
    public int damage;

    protected bool m_IsAttack = false;
    protected CameraShake m_CameraShake;


    //�ܻ����
    public float HitSpeed;      //�����ܻ��ƶ��ٶ�

    protected AnimatorStateInfo m_AnimationInfo;
    protected Vector2 m_HitDirection;
    protected bool m_IsHit = false;
    

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

        if (!m_IsAttack && !m_IsHit)
        {
            //ͨ�������ƶ�����ֹ���ֶ���.��Normalized��ֹб���߸���
            m_Position = m_Rigidbody2d.position + m_Move.normalized * moveSpeed * Time.deltaTime;     //��ɫ��ǰ�����λ�ü����ƶ���������ٶȺ�ʱ��
        }      
        
        else if (m_IsHit)       //�ܻ�ʱ�ᱻ����һ�ξ���
        {
            m_Position = (Vector2)m_Rigidbody2d.position + m_HitDirection * HitSpeed * Time.deltaTime;       //�������Vector2��ʾ������ת���ɶ�ά���������������Ƕ����
        }

        else if(m_IsAttack && !m_IsHit)
        {
            float currentSpeed = m_Animator.GetFloat("SwingType") == 1 ? quickSwingSpeed : heavySwingSpeed;     //���ݶ�������ȷ����������
            m_Position = m_Rigidbody2d.position + m_MoveDirection * currentSpeed * Time.deltaTime;       //���ݹ�����ʽ�����ƶ���������ֹʱҲ����ͨ�������ƶ���
        }

        m_Rigidbody2d.MovePosition(m_Position);
    }



    //�������
    private void SetAnimator()      
    {
        if (!Mathf.Approximately(m_Move.x, 0f) || !Mathf.Approximately(m_Move.y, 0f))     //����ɫ�Ƿ��ƶ�����ɫ�ƶ������ȼ�����ת���ܻ�����
        {
            m_Animator.SetBool("IsWalking", true);

            m_MoveDirection.Set(m_Move.x, m_Move.y);      //��ɫ�ƶ�ʱ������ɫ�ƶ�����������m_MoveDirection
            m_MoveDirection.Normalize();      //��������ֵ�ĳ�1����Ϊֻ��Ҫ����         
        }

        else if (m_IsHit)
        {
            m_MoveDirection = -m_HitDirection;
        }

        else
        {
            m_Animator.SetBool("IsWalking", false);
        }


        m_Animator.SetFloat("MoveX", m_MoveDirection.x);
        m_Animator.SetFloat("MoveY", m_MoveDirection.y);
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

    public void PlayerAttackOver()    //���ڶ����¼����ڲ�ͬ��֡����˺�����
    {
        m_IsAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D other)     //����Ƿ���е���
    {
        if (other.CompareTag("Enemy"))
        {
            //�����
            float shakeIntensity;
            float shakeDuration;

            if (Time.time - other.GetComponent<EnemyFSM>().GetLastHitTime() > other.GetComponent<EnemyFSM>().parameter.hitInterval)     //�������޵�ʱ�����ʱ
            {
                //��������
                other.GetComponent<EnemyFSM>().EnemyTakeDamage(damage);
                other.GetComponent<EnemyFSM>().EnemyGetHit(m_MoveDirection);

                shakeIntensity = m_Animator.GetFloat("SwingType") == 1 ? 1.5f : 2f;
                shakeDuration = m_Animator.GetFloat("SwingType") == 1 ? 0.3f : 0.5f;

                if (m_CameraShake != null)
                {
                    m_CameraShake.ShakeCamera(shakeIntensity, shakeDuration);
                }
            }
        }
    }
    


    //�ܻ����
    public void PlayerGetHit(int damage, Vector2 direction)     //�ܻ�ʱ���ô˺���������Ϊ����ǰ���ķ���
    {
        HealthBar playerHealthBar = GameObject.FindWithTag("HealthBar").GetComponent<HealthBar>();      //��ȡѪ�������Ѫ������ű�

        m_HitDirection = direction;
        health -= damage;

        playerHealthBar.SetCurrentHealth(health);       //ΪѪ������ű������ܻ���ĵ�ǰ����

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
            m_Position = m_Rigidbody2d.position + m_HitDirection * HitSpeed * Time.deltaTime;      //ʹ����򹥻������ƶ�
            m_Rigidbody2d.MovePosition(m_Position);

            if (m_AnimationInfo.normalizedTime >= 0.6f)     //0.6���ȡ���ܻ��ƶ�
            {
                m_IsHit = false;
            }
        }
    }   
}