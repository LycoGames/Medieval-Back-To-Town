public abstract class BaseState
{
    protected bool isRootState = false;
    protected StateMachine ctx;
    protected StateFactory factory;

    protected BaseState currentSuperState;
    protected BaseState currentSubState;

    public BaseState(StateMachine currentContext, StateFactory stateFactory)
    {
        ctx = currentContext;
        factory = stateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (currentSubState != null)
        {
            currentSubState.UpdateStates();
        }
    }

    public void ExitStates()
    {
        ExitState();
        if (currentSubState != null)
        {
            currentSubState.ExitStates();
        }
    }

    protected void SwitchState(BaseState newState)
    {
        ExitState();

        newState.EnterState();

        if (isRootState)
        {
            ctx.CurrentState = newState;
        }
        else if (currentSuperState != null)
        {
            currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(BaseState newSuperState)
    {
        currentSuperState = newSuperState;
    }

    protected void SetSubState(BaseState newSubState)
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}