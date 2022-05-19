using UnityEngine;
using UnityEngine.InputSystem;

public class WUiState : WBaseState
{
    //TODO Paneller kaydırılacak
    public WUiState(WStateMachine currentContext, WStateFactory stateFactory) : base(currentContext, stateFactory)
    {
    }

    public override void EnterState()
    {
        ctx.CanMove = false;
        Cursor.lockState = CursorLockMode.Confined;
        Debug.Log("Ui State Enter");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Ui State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Ui State Exit");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void CheckSwitchStates()
    {
        foreach (GameObject ui in ctx.MainUiArray)
        {
            if (ui.activeInHierarchy)
                return;
        }

        SwitchState(factory.WFreeState());
    }

    public override void InitializeSubState()
    {
    }
}