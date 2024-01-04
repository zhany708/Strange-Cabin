using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    PlayerData m_PlayerData;
    #endregion

    #region Components
    public Core Core { get; private set; }

    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    #endregion

    #region Other Variable

    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();      //从子物体那调用Core脚本

        StateMachine = new PlayerStateMachine();

        //初始化各状态
        IdleState = new PlayerIdleState(this, StateMachine, m_PlayerData, "Idle");      
        MoveState = new PlayerMoveState(this, StateMachine, m_PlayerData, "Move");
        HitState = new PlayerHitState(this, StateMachine, m_PlayerData, "Hit");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack");
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack");
    }

    private void Start()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        Inventory = GetComponent<PlayerInventory>();

        PrimaryAttackState.SetWeapon(Inventory.weapon[(int)CombatInputs.primary]);      //初始化主武器

        StateMachine.Initialize(IdleState);     //初始化状态为闲置

        m_PlayerData.CurrentHealth = m_PlayerData.MaxHealth;        //每次游戏开始时重置角色的当前生命值
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
}