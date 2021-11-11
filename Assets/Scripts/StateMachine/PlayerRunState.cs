using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState() { }

    public override void UpdateState()
    {
        ChechSwitchStates();
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void ChechSwitchStates() { }
}
