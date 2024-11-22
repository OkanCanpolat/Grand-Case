using Zenject;

public class BoardStateMachine 
{
    public IBoardState CurrentState;

    public BoardStateMachine([Inject(Id = "ReadySwipe")] IBoardState initialState)
    {
        CurrentState = initialState;
        CurrentState.OnEnter();
    }
    public void ChangeState(IBoardState state)
    {
        CurrentState?.OnExit();
        CurrentState = state;
        CurrentState.OnEnter();
    }
}
