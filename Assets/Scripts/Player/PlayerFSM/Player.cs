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
    public Animator FootAnimator { get; private set; }



    public Core Core { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    #endregion

    #region Other Variable
    Weapon m_PrimaryWeapon;
    Weapon m_SecondaryWeapon;

    int m_CurrentPrimaryWeaponNum = 0;      //ʹ��ɫ��Ϸ��ʼĬ��װ��ذ��
    int m_CurrentSecondaryWeaponNum = 0;

    int m_FacingNum = 1;
    bool m_FirstFrame = true;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        FootAnimator = transform.Find("PlayerFoot").GetComponent<Animator>();   //��ȡ���ϵĶ��������

        Core = GetComponentInChildren<Core>();      //���������ǵ���Core�ű�

        m_PrimaryWeapon = transform.Find("PrimaryWeapon").GetComponentInChildren<Weapon>();
        m_SecondaryWeapon = transform.Find("SecondaryWeapon").GetComponentInChildren<Weapon>();

        StateMachine = new PlayerStateMachine();

        //��ʼ����״̬
        IdleState = new PlayerIdleState(this, StateMachine, m_PlayerData, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, m_PlayerData, "Idle");
        HitState = new PlayerHitState(this, StateMachine, m_PlayerData, "Hit");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Idle", m_PrimaryWeapon);
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Idle", m_SecondaryWeapon);
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

        if (m_FirstFrame)       //��ֹ��һ֡��ɫ�쳣��ת��ToDO:����ÿ����ͣ��Ϸʱ��Ҳ��Ҫ��ֹ�ָ����һ֡��ɫ�쳣��ת
        {
            m_FirstFrame = false;
            return;
        }

        Flip();   //��������Ƿ�ת���

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

            PrimaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack", Inventory.PrimaryWeapon[newWeaponNum].GetComponent<Weapon>());       //�����¹���״̬

            m_CurrentPrimaryWeaponNum = newWeaponNum;   //�������õ�ǰ��������
        }

        else     //�����������ڸ���
        {
            Inventory.SecondaryWeapon[m_CurrentSecondaryWeaponNum].SetActive(false);
            Inventory.SecondaryWeapon[newWeaponNum].SetActive(true);      

            SecondaryAttackState = new PlayerAttackState(this, StateMachine, m_PlayerData, "Attack", Inventory.SecondaryWeapon[newWeaponNum].GetComponent<Weapon>());  

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


    private void Flip()
    {
        m_FacingNum = InputHandler.ProjectedMousePos.x < transform.position.x ? -1 : 1;     //����������λ�������࣬��ת���

        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * m_FacingNum, transform.localScale.y, transform.localScale.z);      //���ڷ�ת��ɫ
    }
    #endregion

    #region Animation Event Functions
    private void DestroyPlayerAfterDeath()      //���ڶ����¼����ݻ�����
    {
        Destroy(gameObject);   
    }
    #endregion
}