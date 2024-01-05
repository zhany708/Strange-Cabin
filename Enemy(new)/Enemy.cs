using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



#region Parameters
[Serializable]      //�ñ༭�����л������
public class EnemyParameter
{
    //������Ϣ
    public Transform[] PatrolPoints;    //Ѳ�߷�Χ

    //�������
    public Transform Target;     //��ҵ�����
    public Transform[] ChasePoints;     //׷����Χ
    public Transform AttackPoint;   //������Χ��Բ��λ��
}
#endregion





public class Enemy : MonoBehaviour
{
    #region State Variables
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; protected set; }
    public EnemyHitState HitState { get; private set; }
    public EnemyDeathState DeathState { get; private set; }

    [SerializeField]
    protected SO_EnemyData enemyData;
    #endregion

    #region Components
    public EnemyParameter Parameter;
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
    #endregion

    #region Variables
    Vector2 m_LeftDownPosition;     //�����������Ѳ������
    Vector2 m_RightTopPosition;

    float m_LastAttackTime;     //�ϴι�����ʱ��
    float m_LastHitTime;        //�ϴ��ܻ�ʱ��
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();      //���������ǵ���Core�ű�

        StateMachine = new EnemyStateMachine();

        //��ʼ����״̬
        IdleState = new EnemyIdleState(this, StateMachine, enemyData, "Idle");
        //MoveState = new EnemyMoveState(this, StateMachine, m_EnemyData, "Move");      //��ʱ����Ҫ�ƶ�״̬����Ϊ��������״̬�й̶���Ҫ�ƶ�
        PatrolState = new EnemyPatrolState(this, StateMachine, enemyData, "Move");
        ChaseState = new EnemyChaseState(this, StateMachine, enemyData, "Move");
        AttackState = new EnemyAttackState(this, StateMachine, enemyData, "Attack");
        HitState = new EnemyHitState(this, StateMachine, enemyData, "Hit");
        DeathState = new EnemyDeathState(this, StateMachine, enemyData, "Death");

        m_LeftDownPosition = Parameter.PatrolPoints[0].transform.position;      //�ڽű��д���Ѳ�ߵ�
        m_RightTopPosition = Parameter.PatrolPoints[1].transform.position;

        foreach (Transform child in transform.parent)    //�ڳ�����ɾ������Ѳ�ߵ�
        {
            foreach (Transform child2 in child)     //�ڵ��˵ĸ������м���ÿһ���������������
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
        m_LastAttackTime = -enemyData.AttackInterval;       //��Ϸ��ʼʱ�����ϴι���ʱ��

        StateMachine.Initialize(IdleState);     //��ʼ��״̬Ϊ����
    }

    private void Update()
    {
        //Core.LogicUpdate();     //��ȡ��ǰ�ٶ�

        StateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }
    #endregion

    #region Main Functions
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
            StateMachine.ChangeState(IdleState);        //��Ҿ߻�ǽ������ײʱ�л�������״̬    
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Parameter.AttackPoint.position, enemyData.AttackArea);    //���ù�����Χ��Բ�ĺͰ뾶
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
    #endregion
}
