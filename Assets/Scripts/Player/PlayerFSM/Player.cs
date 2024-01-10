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
    SO_PlayerData m_PlayerData;
    #endregion

    #region Components
    public Core Core { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    #endregion

    #region Other Variable
    Weapon m_PrimaryWeapon;
    Weapon m_SecondaryWeapon;

    int m_CurrentPrimaryWeaponNum;
    int m_CurrentSecondaryWeaponNum;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();      //从子物体那调用Core脚本

        m_PrimaryWeapon = transform.Find("PrimaryWeapon").GetComponentInChildren<Weapon>();
        m_SecondaryWeapon = transform.Find("SecondaryWeapon").GetComponentInChildren<Weapon>();

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
        Inventory = GetComponent<PlayerInventory>();

        CheckWeaponNum();

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
    public void GenerateNewAttackState(Weapon weapon)
    {
        /*
        if (Inventory.weaponCount == 0)      //根据武器计数更改角色攻击状态中的武器变量
        {
            PrimaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack", weapon);

            Debug.Log("You have new Primary Attack State!");
        }
        else
        {
            SecondaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack", weapon);
        }
        */
    }

    public void ChangeWeapon(GameObject weapon)
    {
        Inventory.PrimaryWeapon[m_CurrentPrimaryWeaponNum].SetActive(false);
        Inventory.SecondaryWeapon[m_CurrentSecondaryWeaponNum].SetActive(false);

        //需要实现：将需要更换的武器通过SetActive激活，并根据主/副生成新的攻击状态
    }


    private void CheckWeaponNum()       //确定当前武器在库存中的索引
    {
        for (int i = 0; i < Inventory.PrimaryWeapon.Length; i++)       
        {
            if (m_PrimaryWeapon.ToString() == Inventory.PrimaryWeapon[i].ToString())
            {
                m_CurrentPrimaryWeaponNum = i;
            }
            else if (m_SecondaryWeapon.ToString() == Inventory.PrimaryWeapon[i].ToString())
            {
                m_CurrentSecondaryWeaponNum = i;
            }
        }
    }
    #endregion

    #region Animation Event Functions
    private void DestroyPlayerAfterDeath()      //用于动画事件，摧毁物体
    {
        Destroy(gameObject);   
    }
    #endregion
}