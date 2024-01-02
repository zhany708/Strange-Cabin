using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }

    [SerializeField]
    PlayerData m_PlayerData;
    #endregion

    #region Components
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2d { get; private set; }
    #endregion

    #region Other Variable
    public Vector2 CurrentVelocity { get; private set; }
    public Vector2 LastVelocity { get; private set; }     //用于动画朝向

    Vector2 m_WorkSpace;
    #endregion


    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        //初始化各状态
        IdleState = new PlayerIdleState(this, StateMachine, m_PlayerData, "Idle");      
        MoveState = new PlayerMoveState(this, StateMachine, m_PlayerData, "Move");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack");
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack");
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        Rigidbody2d = GetComponent<Rigidbody2D>();

        InputHandler = GetComponent<PlayerInputHandler>();
        Inventory = GetComponent<PlayerInventory>();

        PrimaryAttackState.SetWeapon(Inventory.weapon[(int)CombatInputs.primary]);      //初始化主武器

        StateMachine.Initialize(IdleState);     //初始化状态为闲置
    }

    private void Update()
    {
        StateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }
    #endregion


    #region Setters
    public void SetVelocity(Vector2 velocity)
    {
        m_WorkSpace = velocity;
        Rigidbody2d.velocity = m_WorkSpace;     //移动的关键

        CurrentVelocity = m_WorkSpace;

        if (CurrentVelocity != Vector2.zero)        //防止玩家停止移动后角色固定朝向上
        {
            LastVelocity = CurrentVelocity.normalized;
        }

        Animator.SetFloat("MoveX", LastVelocity.x);     //根据按键输入为动画设置方向
        Animator.SetFloat("MoveY", LastVelocity.y);  
    }
    #endregion
}
