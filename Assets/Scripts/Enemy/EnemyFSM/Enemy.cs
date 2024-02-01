using System;
using UnityEngine;
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
    #region FSM States
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; protected set; }
    public EnemyHitState HitState { get; private set; }
    public EnemyDeathState DeathState { get; protected set;}
    #endregion

    #region Components
    public EnemyParameter Parameter;
    public Core Core { get; private set; }

    public Movement Movement => m_Movement ? m_Movement : Core.GetCoreComponent(ref m_Movement);   //���m_Movement�Ƿ�Ϊ�գ����ǵĻ��򷵻������ǵĻ������GetCoreComponent�����Ի�ȡ���
    private Movement m_Movement;

    public Combat Combat => m_Combat ? m_Combat : Core.GetCoreComponent(ref m_Combat);   //���m_Movement�Ƿ�Ϊ�գ����ǵĻ��򷵻������ǵĻ������GetCoreComponent�����Ի�ȡ���
    private Combat m_Combat;

    /*  �������ú�������ķ���
    public EnemyDeath Death
    {
        get
        {
            if (m_Death) { return m_Death; }      //�������Ƿ�Ϊ��
            m_Death = Core.GetCoreComponent<EnemyDeath>();
            return m_Death;
        }
    }
    private EnemyDeath m_Death;
    */

    public Timer AttackTimer;
    public RandomPosition PatrolRandomPos;
    public Flip EnemyFlip;



    [SerializeField]
    protected SO_EnemyData enemyData;



    EnemyDeath m_Death;
    #endregion

    #region Variables
    public bool CanAttack { get ; private set; }        //���ڹ������

    Vector2 SpawnPos;
    float m_LastHitTime;        //�ϴ��ܻ�ʱ��
    bool m_IsReactivate = false;    //�жϵ����Ƿ�Ϊ���¼���
    #endregion

    #region Unity Callback Functions
    private void Awake()    //����ʵʩ�ĺ�����ֻʵʩһ�Σ�
    {  
        Core = GetComponentInChildren<Core>();      //���������ǵ���Core�ű�
        Core.SetParameters(enemyData.MaxHealth, enemyData.Defense, enemyData.HitResistance);    //���ò���

        StateMachine = new EnemyStateMachine();

        //��ʼ����״̬
        IdleState = new EnemyIdleState(this, StateMachine, enemyData, "Idle");
        PatrolState = new EnemyPatrolState(this, StateMachine, enemyData, "Idle");
        ChaseState = new EnemyChaseState(this, StateMachine, enemyData, "Idle");
        AttackState = new EnemyAttackState(this, StateMachine, enemyData, "Attack");
        HitState = new EnemyHitState(this, StateMachine, enemyData, "Hit");
        DeathState = new EnemyDeathState(this, StateMachine, enemyData, "Death");       
    }

    protected virtual void Start()      //ֻ�ڵ�һ֡����ǰ�����������
    {
        m_Death = GetComponentInChildren<EnemyDeath>();

        StateMachine.Initialize(IdleState);     //��ʼ��״̬Ϊ����
    }

    private void Update()
    {
        Core.LogicUpdate();     //��ȡ��ǰ�ٶ�

        StateMachine.CurrentState.LogicUpdate();

        AttackTimer.Tick();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    protected virtual void OnEnable()       //ÿ�����¼���ʱ���������������
    {
        AttackTimer = new Timer(enemyData.AttackInterval);      //�ù��������ʼ����ʱ��

        //�������������ʼ�������������ű���transform.localPosition���ص���Զ������ڸ���������꣩
        PatrolRandomPos = new RandomPosition(Parameter.PatrolPoints[0].transform.localPosition + (Vector3)SpawnPos, Parameter.PatrolPoints[1].transform.localPosition + (Vector3)SpawnPos);
        //Debug.Log("The LeftDown point is " + (Parameter.PatrolPoints[0].transform.localPosition + (Vector3)SpawnPos) );

        EnemyFlip = new Flip(transform);



        foreach (Transform child in transform.parent)    //�ڳ�����ȡ����������Ѳ�ߵ�
        {
            foreach (Transform child2 in child)     //�ڵ��˵ĸ������м���ÿһ���������������
            {
                if (child2.CompareTag("PatrolPoint"))
                {
                    child2.gameObject.SetActive(false);  
                }
            }
        }


        CanAttack = true;   //��Ϸ��ʼʱ���ɹ�������Ϊtrue
        AttackTimer.OnTimerDone += SetCanAttackTrue;        //�����¼���ʹ���˿������¹���


        if (m_IsReactivate)     //�������¼����Ż��������ʼ������״̬�������һ������ʱ��������ʼ������Ϊ�ű���ʵʩ˳�����null����
        {
            StateMachine.Initialize(IdleState);
        }
    }

    private void OnDisable()
    {
        Movement.Rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;   //���¼�����˺�ֻ����Z�����ת����Ϊ��������ʱ����ֹ�����ƶ�
        Movement.SetVelocityZero();     //ȡ������󽫸����ٶ����ã���ֹ����
        Combat.SetIsHit(false);     //��������������Combat�е��ܻ�����Ϊfalse����ֹ���¼����ֱ�ӽ����ܻ�״̬

        m_IsReactivate = true;

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

    private void SetCanAttackTrue()     //����Action�����ڲ��ܴ���������˲���������Setters��ĺ���
    {
        CanAttack = true;
    }
    #endregion

    #region Animation Event Functions
    private void DestroyEnemyAfterDeath()      //���ڶ����¼����ݻ�����
    {
        if (transform.parent != null)
        {
            if (m_Death.DoorController != null)
            {
                m_Death.DoorController.CheckIfOpenDoors();     //�ж��Ƿ����㿪������
            }
            
            EnemyPool.Instance.PushObject(transform.parent.gameObject);      //�����˵ĸ�����Żس��У�Ҳ���Żظ����������������
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
    #endregion

    #region Setters
    //���ó�Ա����
    public void SetCanAttack(bool isTrue)
    {
        CanAttack = isTrue;
    }

    public void SetLastHitTime(float currentTime)
    {
        m_LastHitTime = currentTime;
    }

    public void SetSpawnPos(Vector2 pos)
    {
        SpawnPos = pos;
    }
    #endregion
}
