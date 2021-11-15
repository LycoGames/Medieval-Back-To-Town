using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    private EnemyBaseState currentState;
    private EnemyStateMachine ctx;
    private EnemyStateFactory factory;

    protected EnemyStateMachine Ctx { get { return ctx; } }
    protected EnemyStateFactory Factory { get { return factory; } }
    protected EnemyBaseState CurrentState { get { return currentState; } }

    public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
    {
        ctx = currentContext;
        factory = enemyStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    protected void SwitchState(EnemyBaseState newState)
    {
        // current state exits state
        ExitState();

        // new state enters state
        newState.EnterState();

        Ctx.CurrentState = newState;
    }
}
