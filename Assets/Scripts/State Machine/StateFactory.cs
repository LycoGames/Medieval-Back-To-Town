public class StateFactory
{
    private StateMachine context;

    public StateFactory(StateMachine currentContext)
    {
        context = currentContext;
    }
    
    public BaseState AppState()
    {
        return new AppState(context, this);
    }

    public BaseState GroundedState()
    {
        return new GroundedState(context, this);
    }

    public BaseState FreeState()
    {
        return new FreeState(context, this);
    }

    public BaseState IdleState()
    {
        return new IdleState(context, this);
    }

    public BaseState WalkState()
    {
        return new WalkState(context, this);
    }

    public BaseState RunState()
    {
        return new RunState(context, this);
    }

    public BaseState CombatState()
    {
        return new CombatState(context, this);
    }

    public BaseState CombatIdleState()
    {
        return new CombatIdleState(context, this);
    }

    public BaseState CombatWalkState()
    {
        return new CombatWalkState(context, this);
    }

    public BaseState CombatRunState()
    {
        return new CombatRunState(context, this);
    }

    public BaseState RollState()
    {
        return new RollState(context, this);
    }

    public BaseState InAirState()
    {
        return new InAirState(context, this);
    }
}