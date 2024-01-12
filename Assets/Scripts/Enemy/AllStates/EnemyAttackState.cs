

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.AttackTimer.StopTimer();     //�����ڼ�ֹͣ��ʱ
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
        enemy.AttackTimer.StartTimer();     //����������ʼ��ʱ
    }
}
