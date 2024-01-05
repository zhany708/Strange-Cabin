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
[Serializable]      //让编辑器序列化这个类
public class Parameter
{
    //基础信息
    public float MoveSpeed;
    public float ChaseSpeed;
    public float IdleDuration;      //待机时长
    public Transform[] PatrolPoints;    //巡逻范围
    public float StoppingDistance;      //敌人与玩家的最小距离

    //攻击相关
    public Transform Target;     //玩家的坐标
    public LayerMask TargetLayer;
    public Transform[] ChasePoints;     //追击范围
    public Transform AttackPoint;   //攻击范围的圆心位置
    public float AttackArea;        //圆的半径参数
    public float AttackInterval;    //攻击间隔

    //受击相关
    public float HitInterval;   //无敌时间
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
            if (m_Movement) { return m_Movement; }      //检查组件是否为空
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



    protected Dictionary<StateType, IEnemyState> states = new Dictionary<StateType, IEnemyState>();     //使用字典注册所有状态

    IEnemyState m_CurrentState;
    #endregion

    #region Variables
    public bool CanHit { get; private set; }

    Vector2 m_LeftDownPosition;     //用于随机生成巡逻坐标
    Vector2 m_RightTopPosition;

    float m_LastAttackTime;     //上次攻击的时间
    float m_LastHitTime;        //上次受击时间
    #endregion

    #region Unity CallBack Functions
    protected void Awake()      
    {
        Core = GetComponentInChildren<Core>();

        m_LeftDownPosition = Parameter.PatrolPoints[0].transform.position;      //在脚本中储存巡逻点
        m_RightTopPosition = Parameter.PatrolPoints[1].transform.position;

        foreach(Transform child in transform.parent)    //在场景中删除所有巡逻点
        {
            foreach(Transform child2 in child)     //在敌人的父物体中检索每一个子物体的子物体
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
        m_LastAttackTime = -Parameter.AttackInterval;       //游戏开始时重置上次攻击时间
        CanHit = true;
        
        states.Add(StateType.Idle, new IdleState(this));        //给字典添加所有状态
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        //states.Add(StateType.React, new ReactState(this));      //暂时用不到这个状态，等有反应动画时再用
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));


        TransitionState(StateType.Idle);        //设置初始状态
    }

    protected void Update()
    {
        m_CurrentState.OnLogicUpdate();      //持续执行当前状态的逻辑相关函数                      
    }

    protected void FixedUpdate()
    {
        DetectHit();

        m_CurrentState.OnPhysicsUpdate();            //持续执行当前状态的物理相关函数        
    }
    #endregion

    #region Main Functions
    //更改状态
    public void TransitionState(StateType type)   
    {
        if (m_CurrentState != null)     //转换状态前先执行当前状态的退出函数
        {
            m_CurrentState.OnExit();
        }

        m_CurrentState = states[type];      //使用字典找到相应的状态类
        m_CurrentState.OnEnter();
    }

    //检测玩家是否超出追击范围
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
    //受击相关
    protected void DetectHit()
    {
        if (Combat.IsHit && CanHit)        //受到攻击时
        {
            TransitionState(StateType.Hit);
        }

        else if (!CanHit)   //受击状态中
        {
            float amount = 0.3f;
            amount += Time.deltaTime;

            Movement.ReduceVelocity(amount);    //持续减少移动速度
        }

        else
        {
            Movement.SetVelocityZero();     //受击结束后使敌人停止移动，也防止玩家碰撞敌人后敌人持续移动
        }
    }

    public void DestroyEnemyAfterDeath()      //用于动画事件，摧毁物体
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
            TransitionState(StateType.Idle);        //与家具或墙触发碰撞时切换成闲置状态    
        } 
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Parameter.AttackPoint.position, Parameter.AttackArea);    //设置攻击范围的圆心和半径
    }
    #endregion

    #region Getters
    //获取成员变量
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
    //设置成员变量
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
