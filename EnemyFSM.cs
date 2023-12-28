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



[Serializable]      //让编辑器序列化这个类
public class Parameter
{
    public float health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleDuration;

    public Transform[] patrolPoints;    //巡逻范围
    public Transform[] chasePoints;     //追击范围

    public Transform target;        //玩家的坐标
    public LayerMask targetLayer;
    public Transform attackPoint;   //圆心检测位置
    public float attackArea;        //圆的半径参数

    public bool isHit;
    public float HitSpeed;      //受击移动速度

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
    Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();     //使用字典注册所有状态



    void Awake()
    {
        parameter.animator = GetComponent<Animator>();
        m_Rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        m_Info = parameter.animator.GetCurrentAnimatorStateInfo(0);

        states.Add(StateType.Idle, new IdleState(this));        //给字典添加所有状态
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        //states.Add(StateType.React, new ReactState(this));      //暂时用不到这个状态，等有反应动画时再用
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));

        TransitionState(StateType.Idle);        //设置初始状态
    }

    void Update()
    {
        m_CurrentState.OnUpdate();      //持续执行当前状态的OnUpdate函数                                              
    }




    public void TransitionState(StateType type)     //更改状态
    {
        if (m_CurrentState != null)     //转换状态前先执行当前状态的退出函数
        {
            m_CurrentState.OnExit();
        }

        m_CurrentState = states[type];      //使用字典找到相应的状态类
        m_CurrentState.OnEnter();
    }


    public void FaceTo(Vector2 faceDirection, Vector2 currentDirection)       //更改当前朝向
    {
        if (faceDirection != null)
        {
            Vector2 direction = (faceDirection - currentDirection).normalized;      //只需要方向

            //使怪物播放朝向坐标的动画
            parameter.animator.SetFloat("Move X", direction.x);
            parameter.animator.SetFloat("Move Y", direction.y);
        }
    }


    //检测玩家是否超出追击范围
    public bool CheckOutside() 
    {
        float minX = Mathf.Min(parameter.chasePoints[0].position.x, parameter.chasePoints[1].position.x);
        float minY = Mathf.Min(parameter.chasePoints[0].position.y, parameter.chasePoints[1].position.y);
        float maxX = Mathf.Max(parameter.chasePoints[0].position.x, parameter.chasePoints[1].position.x);
        float maxY = Mathf.Max(parameter.chasePoints[0].position.y, parameter.chasePoints[1].position.y);

        return parameter.target.position.x < minX || parameter.target.position.x > maxX || parameter.target.position.y < minY || parameter.target.position.y > maxY;
    }


    //受击相关
    public void TakeDamage(int damage)      //受击时调用此函数
    {
        TransitionState(StateType.Hit);
        parameter.health -= damage;
    }

    public void GetHit(Vector2 direction)      //受击时调用此函数，参数为玩家攻击时面对的方向
    {
        parameter.isHit = true;

        //使怪物播放朝向玩家的反方向的动画
        parameter.animator.SetFloat("Move X", -direction.x);
        parameter.animator.SetFloat("Move Y", -direction.y);

        m_HitDirection = direction;
    }

    protected void DetectHit()
    {
        if (parameter.isHit)
        {
            m_Position = (Vector2)m_Rigidbody2d.position + m_HitDirection * parameter.HitSpeed * Time.deltaTime;      //使怪物向攻击方向移动
            m_Rigidbody2d.MovePosition(m_Position);

            if (m_Info.normalizedTime >= 0.6f)     //0.6秒后取消受击移动
            {
                parameter.isHit = false;
            }
        }
    }

    public void DestroyAfterDeath()      //动画事件，摧毁物体
    {
        Destroy(gameObject);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = other.transform;     //储存玩家的位置信息
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = null;     //玩家退出范围时清空
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);    //设置攻击范围的圆心和半径
    }
}
