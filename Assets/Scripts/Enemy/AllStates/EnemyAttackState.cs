

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

    public override void LogicUpdate()      //��д�߼�������ʹ���˲����ڹ���״̬�н����ܻ�״̬
    {
        if (core.AnimatorInfo.IsName("Attack") && core.AnimatorInfo.normalizedTime >= 0.95f)
        {
            stateMachine.ChangeState(enemy.ChaseState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemyCombat.SetIsHit(false);     //���û�����д��룬��ô�����ڹ���״̬�б����еĻ�����ǿ���ڹ���״̬����������ܻ�״̬
        enemy.SetCanAttack(false);
        enemy.AttackTimer.StartTimer();     //����������ʼ��ʱ
    }
}
