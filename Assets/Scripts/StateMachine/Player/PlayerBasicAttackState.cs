using UnityEngine;

public class PlayerBasicAttackState : PlayerBaseState
{
    public PlayerBasicAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("Walk");
    }

    public override void UpdateState()
    {
        if (Ctx.Animator.GetFloat("Speed") != Ctx.WalkSpeed)
            Ctx.Animator.SetFloat("Speed", Ctx.WalkSpeed, 0.1f, Time.deltaTime);

        Vector3 moveDir = Quaternion.Euler(0f, Ctx.RotationAngle, 0f) * Vector3.forward;
        Ctx.CharacterController.Move(moveDir.normalized * Ctx.WalkSpeed * Time.deltaTime);
        Ctx.PlayerTransform.rotation = Quaternion.Euler(0f, Ctx.RotationAngle, 0f);
        CheckSwitchStates();
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
    }
}
