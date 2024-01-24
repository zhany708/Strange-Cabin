using System;
using UnityEngine;
using ZhangYu.Utilities;


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
    public Animator FootAnimator { get; private set; }



    public Core Core { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    public Weapon PrimaryWeapon {  get; private set; }
    public Weapon SecondaryWeapon {  get; private set; }


    Flip m_PlayerFlip;
    #endregion

    #region Other Variable
    public int FacingNum = 1;


    int m_CurrentPrimaryWeaponNum = 0;      //ʹ��ɫ��Ϸ��ʼĬ��װ��ذ��
    int m_CurrentSecondaryWeaponNum = 0;

    bool m_FirstFrame = true;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        FootAnimator = transform.Find("PlayerFoot").GetComponent<Animator>();   //��ȡ���ϵĶ��������

        Core = GetComponentInChildren<Core>();      //���������ǵ���Core�ű�

        PrimaryWeapon = transform.Find("PrimaryWeapon").GetComponentInChildren<Weapon>();
        SecondaryWeapon = transform.Find("SecondaryWeapon").GetComponentInChildren<Weapon>();

        StateMachine = new PlayerStateMachine();

        //��ʼ����״̬
        IdleState = new PlayerIdleState(this, StateMachine, m_PlayerData, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, m_PlayerData, "Idle");
        HitState = new PlayerHitState(this, StateMachine, m_PlayerData, "Hit");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Idle", PrimaryWeapon);
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Idle", SecondaryWeapon);
    }

    private void Start()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        Inventory = GetComponent<PlayerInventory>();

        m_PlayerFlip = new Flip(transform);

        StateMachine.Initialize(IdleState);     //��ʼ��״̬Ϊ����
    }

    private void Update()
    {
        //Core.LogicUpdate();     //��ȡ��ǰ�ٶ�

        if (m_FirstFrame)       //��ֹ��һ֡��ɫ�쳣��ת��ToDO:����ÿ����ͣ��Ϸʱ��Ҳ��Ҫ��ֹ�ָ����һ֡��ɫ�쳣��ת
        {
            m_FirstFrame = false;
            return;
        }

        PlayerFlip();   //��������Ƿ�ת���

        StateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }
    #endregion

    #region Other Functions
    public void ChangeWeapon(GameObject weapon, bool isPrimary)
    {
        int newWeaponNum = CheckNum(weapon);        //��ȡ�µ���������

        //����Ҫ����������ͨ��SetActive�����������/�������µĹ���״̬
        if (isPrimary)      //����������������
        {
            Inventory.PrimaryWeapon[m_CurrentPrimaryWeaponNum].SetActive(false);
            Inventory.PrimaryWeapon[newWeaponNum].SetActive(true);      

            PrimaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Idle", Inventory.PrimaryWeapon[newWeaponNum].GetComponent<Weapon>());       //�����¹���״̬

            m_CurrentPrimaryWeaponNum = newWeaponNum;   //�������õ�ǰ��������
        }

        else     //�����������ڸ���
        {
            Inventory.SecondaryWeapon[m_CurrentSecondaryWeaponNum].SetActive(false);
            Inventory.SecondaryWeapon[newWeaponNum].SetActive(true);      

            SecondaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Idle", Inventory.SecondaryWeapon[newWeaponNum].GetComponent<Weapon>());  

            m_CurrentSecondaryWeaponNum = newWeaponNum;
        }
    }


    private int CheckNum(GameObject weapon)    //�������͸�������˳��һ���������������ּ���
    {
        int WeaponNum = 0;
        for (int i = 0; i < Inventory.PrimaryWeapon.Length; i++)
        {
            if (weapon.ToString() == Inventory.PrimaryWeapon[i].ToString())
            {
                WeaponNum = i;
            }
        }

        return WeaponNum;
    }


    private void PlayerFlip()
    {
        FacingNum = InputHandler.ProjectedMousePos.x < transform.position.x ? -1 : 1;     //����������λ�������࣬��ת���

        m_PlayerFlip.FlipX(FacingNum);
    }
    #endregion

    #region Animation Event Functions
    private void DestroyPlayerAfterDeath()      //���ڶ����¼����ݻ�����
    {
        Destroy(gameObject);   
    }
    #endregion
}