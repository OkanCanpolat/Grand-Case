using Zenject;

public class SpinStateMachine
{
    public ISpinState CurrentState;

    public SpinStateMachine([Inject(Id = "Idle")] ISpinState initialState)
    {
        CurrentState = initialState;
        CurrentState.OnEnter();
    }
    public void ChangeState(ISpinState state)
    {
        CurrentState?.OnExit();
        CurrentState = state;
        CurrentState.OnEnter();
    }
}
