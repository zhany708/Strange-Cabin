using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Windows;


public enum StateType
{
    Idle, Patrol, Chase, React, Attack, Hit, Death
}



[Serializable]      //�ñ༭�����л������
public class Parameter
{
    //������Ϣ
    public float health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleDuration;      //����ʱ��
    public Transform[] patrolPoints;    //Ѳ�߷�Χ
    public float stoppingDistance;      //��������ҵ���С����

    //�������
    public Transform target;        //��ҵ�����
    public LayerMask targetLayer;
    public Transform[] chasePoints;     //׷����Χ
    public Transform attackPoint;   //������Χ��Բ��λ��
    public float attackArea;        //Բ�İ뾶����
    public float attackInterval;    //�������

    //�ܻ����
    public bool isHit;
    public float HitSpeed;      //�ܻ��ƶ��ٶ�

    public Animator animator;
}





public abstract class EnemyFSM : MonoBehaviour
{
    public Parameter parameter;
    public GameObject FireBallPrefab;       //��ȡ����Ԥ�Ƽ�


    protected Rigidbody2D m_Rigidbody2d;
    AnimatorStateInfo m_Info;
    Vector2 m_HitDirection;     //�ܻ�����
    Vector2 m_Position;     //�����ܻ��ƶ�

    Vector2 m_LeftDownPosition;     //�����������Ѳ������
    Vector2 m_RightTopPosition;

    IState m_CurrentState;
    Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();     //ʹ���ֵ�ע������״̬

    float m_LastAttackTime;     //�ϴι�����ʱ��



    protected void Awake()
    {
        parameter.animator = GetComponent<Animator>();
        m_Rigidbody2d = GetComponent<Rigidbody2D>();

        m_LeftDownPosition = parameter.patrolPoints[0].transform.position;      //�ڽű��д���Ѳ�ߵ�
        m_RightTopPosition = parameter.patrolPoints[1].transform.position;

        foreach(Transform child in transform.parent)    //�ڳ�����ɾ������Ѳ�ߵ�
        {
            foreach(Transform child2 in child)     //�ڵ��˵ĸ������м���ÿһ���������������
            {
                if (child2.CompareTag("PatrolPoint"))
                {
                    Destroy(child2.gameObject);
                }
            }
        }
    }

    protected void Start()
    {
        m_Info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        m_LastAttackTime = -parameter.attackInterval;

        states.Add(StateType.Idle, new IdleState(this));        //���ֵ��������״̬
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        //states.Add(StateType.React, new ReactState(this));      //��ʱ�ò������״̬�����з�Ӧ����ʱ����
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));

        TransitionState(StateType.Idle);        //���ó�ʼ״̬
    }

    protected void Update()
    {
        DetectHit();
                           
    }

    protected void FixedUpdate()
    {
        m_CurrentState.OnUpdate();      //����ִ�е�ǰ״̬��OnUpdate����                           
    }



    //����״̬
    public void TransitionState(StateType type)   
    {
        if (m_CurrentState != null)     //ת��״̬ǰ��ִ�е�ǰ״̬���˳�����
        {
            m_CurrentState.OnExit();
        }

        m_CurrentState = states[type];      //ʹ���ֵ��ҵ���Ӧ��״̬��
        m_CurrentState.OnEnter();
    }

    //���ĵ�ǰ����
    public void FaceTo(Vector2 faceDirection, Vector2 currentDirection)     
    {
        if (faceDirection != null)
        {
            Vector2 direction = (faceDirection - currentDirection).normalized;      //ֻ��Ҫ����

            //ʹ���ﲥ�ų�������Ķ���
            parameter.animator.SetFloat("MoveX", direction.x);
            parameter.animator.SetFloat("MoveY", direction.y);
        }
    }


    //�������Ƿ񳬳�׷����Χ
    public bool CheckOutside() 
    {
        float minX = Mathf.Min(parameter.chasePoints[0].position.x, parameter.chasePoints[1].position.x);
        float minY = Mathf.Min(parameter.chasePoints[0].position.y, parameter.chasePoints[1].position.y);
        float maxX = Mathf.Max(parameter.chasePoints[0].position.x, parameter.chasePoints[1].position.x);
        float maxY = Mathf.Max(parameter.chasePoints[0].position.y, parameter.chasePoints[1].position.y);

        return parameter.target.position.x < minX || parameter.target.position.x > maxX || parameter.target.position.y < minY || parameter.target.position.y > maxY;
    }



    //�ܻ����
    public void EnemyTakeDamage(int damage)      //�ܻ�ʱ���ô˺���
    {
        TransitionState(StateType.Hit);
        parameter.health -= damage;
    }

    public void EnemyGetHit(Vector2 direction)      //�ܻ�ʱ���ô˺���������Ϊ��ҹ���ʱ��Եķ���
    {
        parameter.isHit = true;

        //ʹ���ﲥ�ų�����ҵķ�����Ķ���
        parameter.animator.SetFloat("MoveX", -direction.x);
        parameter.animator.SetFloat("MoveY", -direction.y);

        m_HitDirection = direction;
    }

    protected void DetectHit()
    {
        if (parameter.isHit)
        {
            m_Position = (Vector2)m_Rigidbody2d.position + m_HitDirection * parameter.HitSpeed * Time.deltaTime;      //ʹ�����򹥻������ƶ�
            m_Rigidbody2d.MovePosition(m_Position);

            if (m_Info.normalizedTime >= 0.6f)     //0.6���ȡ���ܻ��ƶ�
            {
                parameter.isHit = false;
            }
        }
    }

    public void DestroyEnemyAfterDeath()      //�����¼����ݻ�����
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);       //�ݻٵ��˵ĸ����壬Ҳ���ݻٸ����������������
        }
    }



    //�������
    public void FireBallLaunch(Transform target)
    {
        Vector2 attackX = parameter.animator.GetFloat("MoveX") > Mathf.Epsilon? Vector2.right : Vector2.left;      //���ݶ�������MoveX�жϵ��˳���
        float deviation = Mathf.Abs(parameter.animator.GetFloat("MoveY")) >= Mathf.Abs(parameter.animator.GetFloat("MoveX")) ? 0f : 0.2f;     //ƫ����������ݵ����泯�������ƫ���첿���٣�
        Vector2 attackPosition = m_Rigidbody2d.position + Vector2.up * 0.8f + attackX * deviation;       //��������λ����y����Ӧλ��ͷ����x����Ӧƫ����˵�λ�ã��첿���䣩
        

        float angle = Mathf.Atan2((target.position.y + 0.5f - attackPosition.y), (target.position.x - attackPosition.x)) * Mathf.Rad2Deg;      //����������������֮��ļн�

        GameObject FireBallObject = Instantiate(FireBallPrefab, attackPosition, Quaternion.Euler(0, 0, angle));      //���ɻ���

        FireBall fireBall = FireBallObject.GetComponent<FireBall>();        //���û���ű�
        fireBall.Launch(target.position + Vector3.up * 0.5f - FireBallObject.transform.position, 150);        //����ɫ���ķ��������
    }




    //����������
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = other.transform;     //������ҵ�λ����Ϣ
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = null;     //����˳���Χʱ���
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Furniture") || other.gameObject.CompareTag("Wall"))     
        {
            TransitionState(StateType.Idle);        //��Ҿ߻�ǽ������ײʱ�л�������״̬    
        } 
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);    //���ù�����Χ��Բ�ĺͰ뾶
    }



    //��ȡ��Ա����
    public float getLastAttackTime()
    {
        return m_LastAttackTime;
    }

    public Vector2 getLeftDownPos()
    {
        return m_LeftDownPosition;
    }

    public Vector2 getRightTopPos()
    {
        return m_RightTopPosition;
    }


    //���ó�Ա����
    public void SetLastAttackTime(float currentTime)
    {
        m_LastAttackTime = currentTime;
    }
}
