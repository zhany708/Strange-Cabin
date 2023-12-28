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
    public float health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleDuration;

    public Transform[] patrolPoints;    //Ѳ�߷�Χ
    public Transform[] chasePoints;     //׷����Χ

    public Transform target;        //��ҵ�����
    public LayerMask targetLayer;
    public Transform attackPoint;   //Բ�ļ��λ��
    public float attackArea;        //Բ�İ뾶����

    public bool isHit;
    public float HitSpeed;      //�ܻ��ƶ��ٶ�

    public Animator animator;
}





public class EnemyFSM : MonoBehaviour
{
    public Parameter parameter;

    AnimatorStateInfo m_Info;
    Rigidbody2D m_Rigidbody2d;
    Vector2 m_HitDirection;
    Vector2 m_Position;

    IState m_CurrentState;
    Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();     //ʹ���ֵ�ע������״̬



    void Awake()
    {
        parameter.animator = GetComponent<Animator>();
        m_Rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        m_Info = parameter.animator.GetCurrentAnimatorStateInfo(0);

        states.Add(StateType.Idle, new IdleState(this));        //���ֵ��������״̬
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        //states.Add(StateType.React, new ReactState(this));      //��ʱ�ò������״̬�����з�Ӧ����ʱ����
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));

        TransitionState(StateType.Idle);        //���ó�ʼ״̬
    }

    void Update()
    {
        m_CurrentState.OnUpdate();      //����ִ�е�ǰ״̬��OnUpdate����                                              
    }




    public void TransitionState(StateType type)     //����״̬
    {
        if (m_CurrentState != null)     //ת��״̬ǰ��ִ�е�ǰ״̬���˳�����
        {
            m_CurrentState.OnExit();
        }

        m_CurrentState = states[type];      //ʹ���ֵ��ҵ���Ӧ��״̬��
        m_CurrentState.OnEnter();
    }


    public void FaceTo(Vector2 faceDirection, Vector2 currentDirection)       //���ĵ�ǰ����
    {
        if (faceDirection != null)
        {
            Vector2 direction = (faceDirection - currentDirection).normalized;      //ֻ��Ҫ����

            //ʹ���ﲥ�ų�������Ķ���
            parameter.animator.SetFloat("Move X", direction.x);
            parameter.animator.SetFloat("Move Y", direction.y);
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
    public void TakeDamage(int damage)      //�ܻ�ʱ���ô˺���
    {
        TransitionState(StateType.Hit);
        parameter.health -= damage;
    }

    public void GetHit(Vector2 direction)      //�ܻ�ʱ���ô˺���������Ϊ��ҹ���ʱ��Եķ���
    {
        parameter.isHit = true;

        //ʹ���ﲥ�ų�����ҵķ�����Ķ���
        parameter.animator.SetFloat("Move X", -direction.x);
        parameter.animator.SetFloat("Move Y", -direction.y);

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

    public void DestroyAfterDeath()      //�����¼����ݻ�����
    {
        Destroy(gameObject);
    }



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


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);    //���ù�����Χ��Բ�ĺͰ뾶
    }
}
