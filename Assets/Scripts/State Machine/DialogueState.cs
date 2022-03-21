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
        SetRequiredComponents();
        StartDialogue();
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
        Cursor.visible = false;
        ctx.AimUI.SetActive(true);
        if (aiConversant.IsRepeatableDialogue())
            aiConversant.SetInteractable(true);
    }

    public override void CheckSwitchStates()
    {
        if (!playerConversant.IsActive())
            SwitchState(factory.FreeState());
    }

    public override void InitializeSubState()
    {
    }

    private void StartDialogue()
    {
        playerConversant.StartDialogue(aiConversant.GetDialogue());
        aiConversant.SetInteractable(false);
        ctx.LockCameraPosition = true;
        Cursor.visible = true;
        ctx.AimUI.SetActive(false);
        playerConversant.ResetUI();
    }

    private void SetRequiredComponents()
    {
        playerConversant = ctx.GetComponent<PlayerConversant>();
        aiConversant = ctx.InteractableNPC.GetComponent<AIConversant>();
    }
}