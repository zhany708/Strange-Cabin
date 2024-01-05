using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Windows;


public enum StateType
{
    Idle, Patrol, Chase, React, Attack, Hit, Death
}


#region Parameters
[Serializable]      //�ñ༭�����л������
public class Parameter
{
    //������Ϣ
    public float MoveSpeed;
    public float ChaseSpeed;
    public float IdleDuration;      //����ʱ��
    public Transform[] PatrolPoints;    //Ѳ�߷�Χ
    public float StoppingDistance;      //��������ҵ���С����

    //�������
    public Transform Target;     //��ҵ�����
    public LayerMask TargetLayer;
    public Transform[] ChasePoints;     //׷����Χ
    public Transform AttackPoint;   //������Χ��Բ��λ��
    public float AttackArea;        //Բ�İ뾶����
    public float AttackInterval;    //�������

    //�ܻ����
    public float HitInterval;   //�޵�ʱ��
}
#endregion





public abstract class EnemyFSM : MonoBehaviour
{
    #region Components
    public Parameter Parameter;
    public Core Core { get; private set; }

    public Movement Movement
    {
        get
        {
            if (m_Movement) { return m_Movement; }      //�������Ƿ�Ϊ��
            m_Movement = Core.GetCoreComponent<Movement>();
            return m_Movement;
        }
    }
    private Movement m_Movement;

    public Combat Combat
    {
        get
        {
            if (m_Combat) { return m_Combat; }
            m_Combat = Core.GetCoreComponent<Combat>();
            return m_Combat;
        }
    }
    private Combat m_Combat;

    public Stats Stats
    {
        get
        {
            if (m_Stats) { return m_Stats; }
            m_Stats = Core.GetCoreComponent<Stats>();
            return m_Stats;
        }
    }
    private Stats m_Stats;



    protected Dictionary<StateType, IEnemyState> states = new Dictionary<StateType, IEnemyState>();     //ʹ���ֵ�ע������״̬

    IEnemyState m_CurrentState;
    #endregion

    #region Variables
    public bool CanHit { get; private set; }

    Vector2 m_LeftDownPosition;     //�����������Ѳ������
    Vector2 m_RightTopPosition;

    float m_LastAttackTime;     //�ϴι�����ʱ��
    float m_LastHitTime;        //�ϴ��ܻ�ʱ��
    #endregion

    #region Unity CallBack Functions
    protected void Awake()      
    {
        Core = GetComponentInChildren<Core>();

        m_LeftDownPosition = Parameter.PatrolPoints[0].transform.position;      //�ڽű��д���Ѳ�ߵ�
        m_RightTopPosition = Parameter.PatrolPoints[1].transform.position;

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

    protected virtual void Start()
    {
        m_LastAttackTime = -Parameter.AttackInterval;       //��Ϸ��ʼʱ�����ϴι���ʱ��
        CanHit = true;
        
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
        m_CurrentState.OnLogicUpdate();      //����ִ�е�ǰ״̬���߼���غ���                      
    }

    protected void FixedUpdate()
    {
        DetectHit();

        m_CurrentState.OnPhysicsUpdate();            //����ִ�е�ǰ״̬��������غ���        
    }
    #endregion

    #region Main Functions
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

    //�������Ƿ񳬳�׷����Χ
    public bool CheckOutside() 
    {
        float minX = Mathf.Min(Parameter.ChasePoints[0].position.x, Parameter.ChasePoints[1].position.x);
        float minY = Mathf.Min(Parameter.ChasePoints[0].position.y, Parameter.ChasePoints[1].position.y);
        float maxX = Mathf.Max(Parameter.ChasePoints[0].position.x, Parameter.ChasePoints[1].position.x);
        float maxY = Mathf.Max(Parameter.ChasePoints[0].position.y, Parameter.ChasePoints[1].position.y);

        return Parameter.Target.position.x < minX || Parameter.Target.position.x > maxX || Parameter.Target.position.y < minY || Parameter.Target.position.y > maxY;
    }
    #endregion

    #region Hit Functions
    //�ܻ����
    protected void DetectHit()
    {
        if (Combat.IsHit && CanHit)        //�ܵ�����ʱ
        {
            TransitionState(StateType.Hit);
        }

        else if (!CanHit)   //�ܻ�״̬��
        {
            float amount = 0.3f;
            amount += Time.deltaTime;

            Movement.ReduceVelocity(amount);    //���������ƶ��ٶ�
        }

        else
        {
            Movement.SetVelocityZero();     //�ܻ�������ʹ����ֹͣ�ƶ���Ҳ��ֹ�����ײ���˺���˳����ƶ�
        }
    }

    public void DestroyEnemyAfterDeath()      //���ڶ����¼����ݻ�����
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);       //�ݻٵ��˵ĸ����壬Ҳ���ݻٸ����������������
        }
    }
    #endregion

    #region Trigger Detections
    //����������
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Parameter.Target = other.transform;     //������ҵ�λ����Ϣ
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Parameter.Target = null;     //����˳���Χʱ���
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
        Gizmos.DrawWireSphere(Parameter.AttackPoint.position, Parameter.AttackArea);    //���ù�����Χ��Բ�ĺͰ뾶
    }
    #endregion

    #region Getters
    //��ȡ��Ա����
    public float GetLastAttackTime()
    {
        return m_LastAttackTime;
    }

    public float GetLastHitTime()
    {
        return m_LastHitTime;
    }

    public Vector2 GetLeftDownPos()
    {
        return m_LeftDownPosition;
    }

    public Vector2 GetRightTopPos()
    {
        return m_RightTopPosition;
    }
    #endregion

    #region Setters
    //���ó�Ա����
    public void SetLastAttackTime(float currentTime)
    {
        m_LastAttackTime = currentTime;
    }

    public void SetLastHitTime(float currentTime)
    {
        m_LastHitTime = currentTime;
    }

    public void SetCanHitTrue()
    {
        CanHit = true;
    }

    public void SetCanHitFalse()
    {
        CanHit = false;
    }
    #endregion
}
