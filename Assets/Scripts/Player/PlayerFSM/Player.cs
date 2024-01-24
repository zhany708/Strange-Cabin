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


    int m_CurrentPrimaryWeaponNum = 0;      //使角色游戏开始默认装备匕首
    int m_CurrentSecondaryWeaponNum = 0;

    bool m_FirstFrame = true;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        FootAnimator = transform.Find("PlayerFoot").GetComponent<Animator>();   //获取脚上的动画器组件

        Core = GetComponentInChildren<Core>();      //从子物体那调用Core脚本

        PrimaryWeapon = transform.Find("PrimaryWeapon").GetComponentInChildren<Weapon>();
        SecondaryWeapon = transform.Find("SecondaryWeapon").GetComponentInChildren<Weapon>();

        StateMachine = new PlayerStateMachine();

        //初始化各状态
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

        StateMachine.Initialize(IdleState);     //初始化状态为闲置
    }

    private void Update()
    {
        //Core.LogicUpdate();     //获取当前速度

        if (m_FirstFrame)       //防止第一帧角色异常翻转。ToDO:后续每当暂停游戏时，也需要防止恢复后第一帧角色异常翻转
        {
            m_FirstFrame = false;
            return;
        }

        PlayerFlip();   //持续检测是否翻转玩家

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
        int newWeaponNum = CheckNum(weapon);        //获取新的武器计数

        //将需要更换的武器通过SetActive激活，并根据主/副生成新的攻击状态
        if (isPrimary)      //激活新武器于主手
        {
            Inventory.PrimaryWeapon[m_CurrentPrimaryWeaponNum].SetActive(false);
            Inventory.PrimaryWeapon[newWeaponNum].SetActive(true);      

            PrimaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Idle", Inventory.PrimaryWeapon[newWeaponNum].GetComponent<Weapon>());       //激活新攻击状态

            m_CurrentPrimaryWeaponNum = newWeaponNum;   //重新设置当前武器计数
        }

        else     //激活新武器于副手
        {
            Inventory.SecondaryWeapon[m_CurrentSecondaryWeaponNum].SetActive(false);
            Inventory.SecondaryWeapon[newWeaponNum].SetActive(true);      

            SecondaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Idle", Inventory.SecondaryWeapon[newWeaponNum].GetComponent<Weapon>());  

            m_CurrentSecondaryWeaponNum = newWeaponNum;
        }
    }


    private int CheckNum(GameObject weapon)    //主武器和副武器的顺序一样，所以无需区分计数
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
        FacingNum = InputHandler.ProjectedMousePos.x < transform.position.x ? -1 : 1;     //如果鼠标坐标位于玩家左侧，则翻转玩家

        m_PlayerFlip.FlipX(FacingNum);
    }
    #endregion

    #region Animation Event Functions
    private void DestroyPlayerAfterDeath()      //用于动画事件，摧毁物体
    {
        Destroy(gameObject);   
    }
    #endregion
}