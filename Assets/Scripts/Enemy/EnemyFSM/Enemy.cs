using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using ZhangYu.Utilities;



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

    [SerializeField]
    protected SO_EnemyData enemyData;
    #endregion

    #region Components
    public EnemyParameter Parameter;
    public Core Core { get; private set; }

    public Movement Movement => m_Movement ? m_Movement : Core.GetCoreComponent(ref m_Movement);   //���m_Movement�Ƿ�Ϊ�գ����ǵĻ��򷵻������ǵĻ������GetCoreComponent�����Ի�ȡ���
    private Movement m_Movement;


    /*  �������ú�������ķ���
    public Death Death
    {
        get
        {
            if (m_Death) { return m_Death; }      //�������Ƿ�Ϊ��
            m_Death = Core.GetCoreComponent<Death>();
            return m_Death;
        }
    }
    private Death m_Death;
    */


    public Timer AttackTimer;
    #endregion

    #region Variables
    public bool CanAttack { get ; private set; }

    Vector2 m_LeftDownPosition;     //�����������Ѳ������
    Vector2 m_RightTopPosition;

    float m_LastHitTime;        //�ϴ��ܻ�ʱ��
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();      //���������ǵ���Core�ű�

        StateMachine = new EnemyStateMachine();

        //��ʼ����״̬
        IdleState = new EnemyIdleState(this, StateMachine, enemyData, "Idle");
        PatrolState = new EnemyPatrolState(this, StateMachine, enemyData, "Move");
        ChaseState = new EnemyChaseState(this, StateMachine, enemyData, "Move");
        AttackState = new EnemyAttackState(this, StateMachine, enemyData, "Attack");
        HitState = new EnemyHitState(this, StateMachine, enemyData, "Hit");

        AttackTimer = new Timer(enemyData.AttackInterval);      //�ù��������ʼ����ʱ��

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
        StateMachine.Initialize(IdleState);     //��ʼ��״̬Ϊ����

        CanAttack = true;   //��Ϸ��ʼʱ���ɹ�������Ϊtrue
    }

    private void Update()
    {
        //Core.LogicUpdate();     //��ȡ��ǰ�ٶ�

        StateMachine.CurrentState.LogicUpdate();

        AttackTimer.Tick();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnEnable()
    {
        AttackTimer.OnTimerDone += SetCanAttackTrue;        //�����¼�����ʱ������Ŀ��ʱ�䣩ʱ���ɹ�����������Ϊtrue
    }

    private void OnDisable()
    {
        AttackTimer.OnTimerDone -= SetCanAttackTrue;
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

    private void SetCanAttackTrue()
    {
        CanAttack = true;
    }
    #endregion

    #region Animation Event Functions
    private void DestroyEnemyAfterDeath()      //���ڶ����¼����ݻ�����
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
    public void SetCanAttackFalse()
    {
        CanAttack = false;
    }

    public void SetLastHitTime(float currentTime)
    {
        m_LastHitTime = currentTime;
    }
    #endregion
}
