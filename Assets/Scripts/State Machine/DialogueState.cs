using UnityEngine;

public class DialogueState : BaseState
{
    private PlayerConversant playerConversant;
    private AIConversant aiConversant;

    public DialogueState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Dialogue State Enter");
        playerConversant = ctx.GetComponent<PlayerConversant>();
        aiConversant = ctx.InteractableNPC.GetComponent<AIConversant>();
        playerConversant.StartDialogue(aiConversant.GetDialogue());
        ctx.LockCameraPosition = true;
        ctx.AimUI.SetActive(false);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("Dialogue State Update");
    }

    public override void ExitState()
    {
        Debug.Log("Dialogue State Exit");
        ctx.LockCameraPosition = false;
        ctx.AimUI.SetActive(true);
    }

    public override void CheckSwitchStates()
    {
        if (!playerConversant.IsActive())
            SwitchState(factory.FreeState());
    }

    public override void InitializeSubState()
    {
    }
}