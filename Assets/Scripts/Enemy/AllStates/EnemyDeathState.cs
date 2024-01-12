

public class EnemyDeathState : EnemyState
{
    public EnemyDeathState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void Enter()
    {
        enemy.Parameter.Target = null;      //敌人死亡后将Target坐标清零，防止出现bug
    }
}
