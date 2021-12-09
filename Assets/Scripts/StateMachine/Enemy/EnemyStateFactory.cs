public class EnemyStateFactory
{
    EnemyStateMachine context;

    public EnemyStateFactory(EnemyStateMachine currentContext)
    {
        context = currentContext;
    }

    public EnemyBaseState Patrol()
    {
        return new EnemyPatrolState(context, this);
    }
    public EnemyBaseState Chase()
    {
        return new EnemyChaseState(context, this);
    }
    public EnemyBaseState Attack()
    {
        return new EnemyAttackState(context, this);
    }
}
