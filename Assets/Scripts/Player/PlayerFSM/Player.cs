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
        Core = GetComponentInChildren<Core>();      //���������ǵ���Core�ű�

        m_PrimaryWeapon = transform.Find("PrimaryWeapon").GetComponentInChildren<Weapon>();
        m_SecondaryWeapon = transform.Find("SecondaryWeapon").GetComponentInChildren<Weapon>();

        StateMachine = new PlayerStateMachine();

        //��ʼ����״̬
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

        StateMachine.Initialize(IdleState);     //��ʼ��״̬Ϊ����
    }

    private void Update()
    {
        //Core.LogicUpdate();     //��ȡ��ǰ�ٶ�

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
        if (Inventory.weaponCount == 0)      //���������������Ľ�ɫ����״̬�е���������
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

        //��Ҫʵ�֣�����Ҫ����������ͨ��SetActive�����������/�������µĹ���״̬
    }


    private void CheckWeaponNum()       //ȷ����ǰ�����ڿ���е�����
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
    private void DestroyPlayerAfterDeath()      //���ڶ����¼����ݻ�����
    {
        Destroy(gameObject);   
    }
    #endregion
}