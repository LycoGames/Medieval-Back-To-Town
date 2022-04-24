public abstract class WBaseState
{
    protected bool isRootState = false;
    protected WStateMachine ctx;
    protected WStateFactory factory;

    public WBaseState currentSuperState;
    public WBaseState currentSubState;

    public WBaseState(WStateMachine currentContext, WStateFactory stateFactory)
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

    protected void SwitchState(WBaseState newState)
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

    protected void SetSuperState(WBaseState newSuperState)
    {
        currentSuperState = newSuperState;
    }

    protected void SetSubState(WBaseState newSubState)
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}