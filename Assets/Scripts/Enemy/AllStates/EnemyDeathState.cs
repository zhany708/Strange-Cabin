

public class EnemyDeathState : EnemyState
{
    public EnemyDeathState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void Enter()
    {
        enemy.Parameter.Target = null;      //����������Target�������㣬��ֹ����bug
        enemyCombat.enabled = false;     //ȡ��������ײ�򣬷�ֹ���ֱ�ʬ����
    }

    public override void LogicUpdate() { }      //��д�߼�������ʹ���˲���������״̬�н����ܻ�״̬

    public override void Exit()
    {
        base.Exit();

        enemyCombat.SetIsHit(false);    //���ܻ���������Ϊfalse����ֹ�������¼����û�ܻ��ͽ����ܻ�״̬
    }
}
