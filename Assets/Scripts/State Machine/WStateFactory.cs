public class WStateFactory
{
    private WStateMachine context;

    public WStateFactory(WStateMachine currentContext)
    {
        context = currentContext;
    }
    
    public WBaseState WAppState()
    {
        return new WAppState(context, this);
    }

    public WBaseState WGroundedState()
    {
        return new WGroundedState(context, this);
    }

    public WBaseState WFreeState()
    {
        return new WFreeState(context, this);
    }

    public WBaseState WIdleState()
    {
        return new WIdleState(context, this);
    }

    public WBaseState WWalkState()
    {
        return new WWalkState(context, this);
    }

    public WBaseState WRunState()
    {
        return new WRunState(context, this);
    }

    public WBaseState WCombatState()
    {
        return new WCombatState(context, this);
    }

    public WBaseState WCombatIdleState()
    {
        return new WCombatIdleState(context, this);
    }

    public WBaseState WCombatWalkState()
    {
        return new WCombatWalkState(context, this);
    }

    public WBaseState WCombatRunState()
    {
        return new WCombatRunState(context, this);
    }

    public WBaseState WRollState()
    {
        return new WRollState(context, this);
    }

    public WBaseState WInAirState()
    {
        return new WInAirState(context, this);
    }
}