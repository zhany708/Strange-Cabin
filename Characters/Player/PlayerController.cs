using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //移动相关
    public InputAction moveAction;
    public float moveSpeed = 2.5f;

    protected Vector2 m_Position;
    Vector2 m_Move;
    protected Rigidbody2D m_Rigidbody2d;


    //动画相关
    protected Animator m_Animator;
    protected Vector2 m_MoveDirection = new Vector2(1, 0);


    //攻击相关
    public InputAction heavySwingAction;
    public InputAction quickSwingAction;
    public float quickSwingSpeed;    //补偿速度
    public float heavySwingSpeed;
    public int damage;

    protected bool m_IsAttack = false;
    protected CameraShake m_CameraShake;


    //受击相关
    public float HitSpeed;      //人物受击移动速度

    protected AnimatorStateInfo m_AnimationInfo;
    protected Vector2 m_HitDirection;
    protected bool m_IsHit = false;
    

    //生命相关
    public float health;




    void Start()
    {
        moveAction.Enable();

        
        heavySwingAction.Enable();
        quickSwingAction.Enable();
        heavySwingAction.performed += HeavySwing;       //回调攻击函数
        quickSwingAction.performed += QuickSwing;
        

        m_Rigidbody2d = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        m_CameraShake = FindObjectOfType<CameraShake>();    //找拥有CameraShake脚本的组件
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



    //移动相关
    private void Move()
    {
        //计算Input system的物理值
        m_Move = moveAction.ReadValue<Vector2>();

        if (!m_IsAttack && !m_IsHit)
        {
            //通过刚体移动，防止出现抖动.加Normalized防止斜着走更快
            m_Position = m_Rigidbody2d.position + m_Move.normalized * moveSpeed * Time.deltaTime;     //角色当前刚体的位置加上移动方向乘以速度和时间
        }      
        
        else if (m_IsHit)       //受击时会被击退一段距离
        {
            m_Position = (Vector2)m_Rigidbody2d.position + m_HitDirection * HitSpeed * Time.deltaTime;       //括号里的Vector2表示将向量转换成二维向量，但在这里是多余的
        }

        else if(m_IsAttack && !m_IsHit)
        {
            float currentSpeed = m_Animator.GetFloat("SwingType") == 1 ? quickSwingSpeed : heavySwingSpeed;     //根据动画参数确定攻击类型
            m_Position = m_Rigidbody2d.position + m_MoveDirection * currentSpeed * Time.deltaTime;       //根据攻击方式给予移动补偿（静止时也可以通过攻击移动）
        }

        m_Rigidbody2d.MovePosition(m_Position);
    }



    //动画相关
    private void SetAnimator()      
    {
        if (!Mathf.Approximately(m_Move.x, 0f) || !Mathf.Approximately(m_Move.y, 0f))     //检查角色是否移动。角色移动的优先级大于转向受击方向
        {
            m_Animator.SetBool("IsWalking", true);

            m_MoveDirection.Set(m_Move.x, m_Move.y);      //角色移动时，将角色移动的向量传给m_MoveDirection
            m_MoveDirection.Normalize();      //将向量的值改成1（因为只需要方向）         
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


    
    //攻击相关
    private void HeavySwing(InputAction.CallbackContext context)        //重攻击
    {
        if (!m_IsAttack)
        {
            m_IsAttack = true;
            m_Animator.SetTrigger("Swing");
            m_Animator.SetFloat("SwingType", 0f);
        }
    }

    private void QuickSwing(InputAction.CallbackContext context)        //轻攻击
    {
        if (!m_IsAttack)
        {
            m_IsAttack = true;
            m_Animator.SetTrigger("Swing");
            m_Animator.SetFloat("SwingType", 1f);
        }
    }

    public void PlayerAttackOver()    //用于动画事件（在不同的帧插入此函数）
    {
        m_IsAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D other)     //检测是否击中敌人
    {
        if (other.CompareTag("Enemy"))
        {
            //相机震动
            float shakeIntensity;
            float shakeDuration;

            if (Time.time - other.GetComponent<EnemyFSM>().GetLastHitTime() > other.GetComponent<EnemyFSM>().parameter.hitInterval)     //当敌人无敌时间结束时
            {
                //敌人受伤
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
    


    //受击相关
    public void PlayerGetHit(int damage, Vector2 direction)     //受击时调用此函数，参数为火球前进的方向
    {
        HealthBar playerHealthBar = GameObject.FindWithTag("HealthBar").GetComponent<HealthBar>();      //获取血条组件的血条缓冲脚本

        m_HitDirection = direction;
        health -= damage;

        playerHealthBar.SetCurrentHealth(health);       //为血条缓冲脚本设置受击后的当前生命

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
            m_Position = m_Rigidbody2d.position + m_HitDirection * HitSpeed * Time.deltaTime;      //使玩家向攻击方向移动
            m_Rigidbody2d.MovePosition(m_Position);

            if (m_AnimationInfo.normalizedTime >= 0.6f)     //0.6秒后取消受击移动
            {
                m_IsHit = false;
            }
        }
    }   
}