using UnityEngine;
using ZhangYu.Utilities;


public class MultiAttackMeleeWeapon : MeleeWeapon
{
    [SerializeField] private float m_AttackCounterResetCooldown;

    private Timer m_AttackCounterResetTimer;



    protected override void Awake()
    {
        base.Awake();

        m_AttackCounterResetTimer = new Timer(m_AttackCounterResetCooldown);
    }

    protected override void Update()
    {
        base.Update();

        m_AttackCounterResetTimer.Tick();   //�����Լ�ʱ�����м�ʱ
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        m_AttackCounterResetTimer.OnTimerDone += ResetAttackCounter;    //�����¼�����ʱ������Ŀ��ʱ�䣩ʱ��������
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        m_AttackCounterResetTimer.OnTimerDone -= ResetAttackCounter;    //�����¼�����ʱ������Ŀ��ʱ�䣩ʱ��������
    }





    public override void EnterWeapon()
    {
        m_AttackCounterResetTimer.StopTimer();      //����״̬����ͣ��ʱ������ֹ����������û��������������

        base.EnterWeapon();

        animator.SetInteger("AttackCounter", CurrentAttackCounter);
    }

    public override void ExitWeapon()
    {
        base.ExitWeapon();

        CurrentAttackCounter++;    //���ӹ�������������ҽ��ж�����
        m_AttackCounterResetTimer.StartTimer();      //����������ʼ��ʱ��
    }



    protected void ResetAttackCounter() => CurrentAttackCounter = 0;     //����������
}
