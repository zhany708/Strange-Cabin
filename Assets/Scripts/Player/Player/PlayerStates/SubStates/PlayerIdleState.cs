public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, SO_PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }


    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();       //������ٶȹ���
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (input.x != 0f || input.y != 0f)
        {
            stateMachine.ChangeState(player.MoveState);
        }
    }
}
