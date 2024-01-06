using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhangYu.Weapons;


public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerHitState HitState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }

    [SerializeField]
    SO_PlayerData m_PlayerData;
    #endregion

    #region Components
    public Core Core { get; private set; }

    public PlayerInputHandler InputHandler { get; private set; }
    #endregion

    #region Other Variable
    Weapon m_PrimaryWeapon;
    Weapon m_SecondaryWeapon;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();      //从子物体那调用Core脚本

        m_PrimaryWeapon = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        m_SecondaryWeapon = transform.Find("SecondaryWeapon").GetComponent<Weapon>();

        StateMachine = new PlayerStateMachine();

        //初始化各状态
        IdleState = new PlayerIdleState(this, StateMachine, m_PlayerData, "Idle");      
        MoveState = new PlayerMoveState(this, StateMachine, m_PlayerData, "Move");
        HitState = new PlayerHitState(this, StateMachine, m_PlayerData, "Hit");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack", m_PrimaryWeapon);
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack", m_SecondaryWeapon);
    }

    private void Start()
    {
        InputHandler = GetComponent<PlayerInputHandler>();

        StateMachine.Initialize(IdleState);     //初始化状态为闲置
    }

    private void Update()
    {
        //Core.LogicUpdate();     //获取当前速度

        StateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }
    #endregion

    #region Other Functions
    private void DestroyPlayer()    //用于动画帧事件
    {
        Destroy(gameObject);
    }
    #endregion
}