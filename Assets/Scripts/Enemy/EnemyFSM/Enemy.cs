using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using ZhangYu.Utilities;



#region Parameters
[Serializable]      //让编辑器序列化这个类
public class EnemyParameter
{
    //基础信息
    public Transform[] PatrolPoints;    //巡逻范围

    //攻击相关
    public Transform Target;     //玩家的坐标
    public Transform[] ChasePoints;     //追击范围
    public Transform AttackPoint;   //攻击范围的圆心位置
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

    public Movement Movement => m_Movement ? m_Movement : Core.GetCoreComponent(ref m_Movement);   //检查m_Movement是否为空，不是的话则返回它，是的话则调用GetCoreComponent函数以获取组件
    private Movement m_Movement;


    /*  基础调用核心组件的方法
    public Death Death
    {
        get
        {
            if (m_Death) { return m_Death; }      //检查组件是否为空
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

    Vector2 m_LeftDownPosition;     //用于随机生成巡逻坐标
    Vector2 m_RightTopPosition;

    float m_LastHitTime;        //上次受击时间
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();      //从子物体那调用Core脚本

        StateMachine = new EnemyStateMachine();

        //初始化各状态
        IdleState = new EnemyIdleState(this, StateMachine, enemyData, "Idle");
        PatrolState = new EnemyPatrolState(this, StateMachine, enemyData, "Move");
        ChaseState = new EnemyChaseState(this, StateMachine, enemyData, "Move");
        AttackState = new EnemyAttackState(this, StateMachine, enemyData, "Attack");
        HitState = new EnemyHitState(this, StateMachine, enemyData, "Hit");

        AttackTimer = new Timer(enemyData.AttackInterval);      //用攻击间隔初始化计时器

        m_LeftDownPosition = Parameter.PatrolPoints[0].transform.position;      //在脚本中储存巡逻点
        m_RightTopPosition = Parameter.PatrolPoints[1].transform.position;

        foreach (Transform child in transform.parent)    //在场景中删除所有巡逻点
        {
            foreach (Transform child2 in child)     //在敌人的父物体中检索每一个子物体的子物体
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
        StateMachine.Initialize(IdleState);     //初始化状态为闲置

        CanAttack = true;   //游戏开始时将可攻击设置为true
    }

    private void Update()
    {
        //Core.LogicUpdate();     //获取当前速度

        StateMachine.CurrentState.LogicUpdate();

        AttackTimer.Tick();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnEnable()
    {
        AttackTimer.OnTimerDone += SetCanAttackTrue;        //触发事件（计时器到达目标时间）时将可攻击布尔设置为true
    }

    private void OnDisable()
    {
        AttackTimer.OnTimerDone -= SetCanAttackTrue;
    }
    #endregion

    #region Main Functions
    //检测玩家是否超出追击范围
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
    private void DestroyEnemyAfterDeath()      //用于动画事件，摧毁物体
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);       //摧毁敌人的父物体，也将摧毁父物体的所有子物体
        }
    }
    #endregion

    #region Trigger Detections
    //各种物理检测
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Parameter.Target = other.transform;     //储存玩家的位置信息
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Parameter.Target = null;     //玩家退出范围时清空
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Furniture") || other.gameObject.CompareTag("Wall"))
        {
            StateMachine.ChangeState(IdleState);        //与家具或墙触发碰撞时切换成闲置状态    
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Parameter.AttackPoint.position, enemyData.AttackArea);    //设置攻击范围的圆心和半径
    }
    #endregion

    #region Getters
    //获取成员变量
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
    //设置成员变量
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
