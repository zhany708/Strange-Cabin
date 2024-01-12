public class PlayerStateMachine
{
    public PlayerState currentState {  get; private set; }  //����һ������Getter��һ��˽��Setter���������

    public void Initialize(PlayerState startingState)
    {
        currentState = startingState;
        currentState.Enter();   
    }

    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
