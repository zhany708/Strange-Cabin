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

    int m_CurrentPrimaryWeaponNum = 0;      //ʹ��ɫ��Ϸ��ʼĬ��װ��ذ��
    //int m_CurrentSecondaryWeaponNum = 0;
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
        //Inventory.SecondaryWeapon[m_CurrentSecondaryWeaponNum].SetActive(false);

        //����Ҫ����������ͨ��SetActive�����������/�������µĹ���״̬
        int newWeaponNum = CheckNum(weapon);        //��ȡ�µ���������

        Inventory.PrimaryWeapon[newWeaponNum].SetActive(true);      //����������

        PrimaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack", Inventory.PrimaryWeapon[newWeaponNum].GetComponent<Weapon>());       //�����¹���״̬
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
    #endregion

    #region Animation Event Functions
    private void DestroyPlayerAfterDeath()      //���ڶ����¼����ݻ�����
    {
        Destroy(gameObject);   
    }
    #endregion
}