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
    //基础信息
    public float health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleDuration;      //待机时长
    public Transform[] patrolPoints;    //巡逻范围
    public float stoppingDistance;      //敌人与玩家的最小距离

    //攻击相关
    public Transform target;        //玩家的坐标
    public LayerMask targetLayer;
    public Transform[] chasePoints;     //追击范围
    public Transform attackPoint;   //攻击范围的圆心位置
    public float attackArea;        //圆的半径参数
    public float attackInterval;    //攻击间隔

    //受击相关
    public bool isHit;
    public float HitSpeed;      //受击移动速度

    public Animator animator;
}





public abstract class EnemyFSM : MonoBehaviour
{
    public Parameter parameter;
    public GameObject FireBallPrefab;       //获取火球预制件


    protected Rigidbody2D m_Rigidbody2d;
    AnimatorStateInfo m_Info;
    Vector2 m_HitDirection;     //受击方向
    Vector2 m_Position;     //用于受击移动

    Vector2 m_LeftDownPosition;     //用于随机生成巡逻坐标
    Vector2 m_RightTopPosition;

    IState m_CurrentState;
    Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();     //使用字典注册所有状态

    float m_LastAttackTime;     //上次攻击的时间



    protected void Awake()
    {
        parameter.animator = GetComponent<Animator>();
        m_Rigidbody2d = GetComponent<Rigidbody2D>();

        m_LeftDownPosition = parameter.patrolPoints[0].transform.position;      //在脚本中储存巡逻点
        m_RightTopPosition = parameter.patrolPoints[1].transform.position;

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

    protected void Start()
    {
        m_Info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        m_LastAttackTime = -parameter.attackInterval;

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
        DetectHit();
                           
    }

    protected void FixedUpdate()
    {
        m_CurrentState.OnUpdate();      //持续执行当前状态的OnUpdate函数                           
    }



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

    //更改当前朝向
    public void FaceTo(Vector2 faceDirection, Vector2 currentDirection)     
    {
        if (faceDirection != null)
        {
            Vector2 direction = (faceDirection - currentDirection).normalized;      //只需要方向

            //使怪物播放朝向坐标的动画
            parameter.animator.SetFloat("MoveX", direction.x);
            parameter.animator.SetFloat("MoveY", direction.y);
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
    public void EnemyTakeDamage(int damage)      //受击时调用此函数
    {
        TransitionState(StateType.Hit);
        parameter.health -= damage;
    }

    public void EnemyGetHit(Vector2 direction)      //受击时调用此函数，参数为玩家攻击时面对的方向
    {
        parameter.isHit = true;

        //使怪物播放朝向玩家的反方向的动画
        parameter.animator.SetFloat("MoveX", -direction.x);
        parameter.animator.SetFloat("MoveY", -direction.y);

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

    public void DestroyEnemyAfterDeath()      //动画事件，摧毁物体
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);       //摧毁敌人的父物体，也将摧毁父物体的所有子物体
        }
    }



    //攻击相关
    public void FireBallLaunch(Transform target)
    {
        Vector2 attackX = parameter.animator.GetFloat("MoveX") > Mathf.Epsilon? Vector2.right : Vector2.left;      //根据动画参数MoveX判断敌人朝向
        float deviation = Mathf.Abs(parameter.animator.GetFloat("MoveY")) >= Mathf.Abs(parameter.animator.GetFloat("MoveX")) ? 0f : 0.2f;     //偏离参数（根据敌人面朝方向决定偏离嘴部多少）
        Vector2 attackPosition = m_Rigidbody2d.position + Vector2.up * 0.8f + attackX * deviation;       //火球生成位置在y轴上应位于头部，x轴上应偏离敌人的位置（嘴部发射）
        

        float angle = Mathf.Atan2((target.position.y + 0.5f - attackPosition.y), (target.position.x - attackPosition.x)) * Mathf.Rad2Deg;      //计算火球与玩家中心之间的夹角

        GameObject FireBallObject = Instantiate(FireBallPrefab, attackPosition, Quaternion.Euler(0, 0, angle));      //生成火球

        FireBall fireBall = FireBallObject.GetComponent<FireBall>();        //调用火球脚本
        fireBall.Launch(target.position + Vector3.up * 0.5f - FireBallObject.transform.position, 150);        //朝角色中心方向发射火球
    }




    //各种物理检测
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


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Furniture") || other.gameObject.CompareTag("Wall"))     
        {
            TransitionState(StateType.Idle);        //与家具或墙触发碰撞时切换成闲置状态    
        } 
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);    //设置攻击范围的圆心和半径
    }



    //获取成员变量
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


    //设置成员变量
    public void SetLastAttackTime(float currentTime)
    {
        m_LastAttackTime = currentTime;
    }
}
