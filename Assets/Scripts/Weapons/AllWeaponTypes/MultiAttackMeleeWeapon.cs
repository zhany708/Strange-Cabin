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

    private void Update()
    {
        m_AttackCounterResetTimer.Tick();   //持续对计时器进行计时
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        m_AttackCounterResetTimer.OnTimerDone += ResetAttackCounter;    //触发事件（计时器到达目标时间）时重置连击
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        m_AttackCounterResetTimer.OnTimerDone -= ResetAttackCounter;    //触发事件（计时器到达目标时间）时重置连击
    }





    public override void EnterWeapon()
    {
        m_AttackCounterResetTimer.StopTimer();      //攻击状态中暂停计时器，防止攻击动画还没结束就重置连击

        base.EnterWeapon();

        baseAnimator.SetInteger("AttackCounter", CurrentAttackCounter);
        weaponAnimator.SetInteger("AttackCounter", CurrentAttackCounter);
    }

    public override void ExitWeapon()
    {
        base.ExitWeapon();

        CurrentAttackCounter++;    //增加攻击计数以让玩家进行二连击
        m_AttackCounterResetTimer.StartTimer();      //攻击结束后开始计时器
    }



    protected void ResetAttackCounter() => CurrentAttackCounter = 0;     //重置连击数
}
