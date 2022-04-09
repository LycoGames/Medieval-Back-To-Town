using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryState : BaseState
{
    public InventoryState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        ctx.CanMove = false;
        Debug.Log("Inventory State Enter");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        if (ctx.InventoryUi.activeInHierarchy == false)
        {
            SwitchState(factory.FreeState());
        }

        Debug.Log("Inventory State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Inventory State Exit");
    }

    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState()
    {
    }
}