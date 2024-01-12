

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.AttackTimer.StopTimer();     //攻击期间停止计时
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (core.AnimatorInfo.IsName("Attack") && core.AnimatorInfo.normalizedTime >= 0.95f)
        {
            stateMachine.ChangeState(enemy.ChaseState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.SetCanAttackFalse();
        enemy.AttackTimer.StartTimer();     //攻击结束后开始计时
    }
}
