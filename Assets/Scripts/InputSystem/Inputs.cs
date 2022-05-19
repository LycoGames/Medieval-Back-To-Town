using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class Inputs : MonoBehaviour
    {
        [Header("Character Input Values")] public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool roll;
        public bool sprint;
        public bool basicAttack;
        public bool inventoryShowHide;
        public bool interaction;
        [Header("Movement Settings")] public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
        [Header("Mouse Cursor Settings")] public bool cursorLocked = true;
        public bool cursorInputForLook = true;
#endif

        public void OnMove(InputAction.CallbackContext ctx)
        {
            MoveInput(ctx.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            if (cursorInputForLook)
            {
                LookInput(ctx.ReadValue<Vector2>());
            }
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            JumpInput(ctx.ReadValueAsButton());
        }

        public void OnInteraction(InputAction.CallbackContext ctx)
        {
            InteractionInput(ctx.ReadValueAsButton());
        }

        public void OnRoll(InputAction.CallbackContext ctx)
        {
            RollInput(ctx.ReadValueAsButton());
        }

        public void OnSprint(InputAction.CallbackContext ctx)
        {
            SprintInput(ctx.ReadValueAsButton());
        }

        public void OnBasicAttack(InputAction.CallbackContext ctx)
        {
            BasicAttackInput(ctx.ReadValueAsButton());
        }

        public void OnInventoryShowHideInput(InputAction.CallbackContext ctx)
        {
            InventoryShowHideInput(ctx.started);
        }


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void InteractionInput(bool newJumpState)
        {
            interaction = newJumpState;
        }

        public void RollInput(bool newRollState)
        {
            roll = newRollState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void BasicAttackInput(bool newBasicAttackState)
        {
            basicAttack = newBasicAttackState;
        }

        public void InventoryShowHideInput(bool newInventoryShowHideState)
        {
            inventoryShowHide = newInventoryShowHideState;
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

#if !UNITY_IOS || !UNITY_ANDROID

        /*  private void OnApplicationFocus(bool hasFocus)
          {
              SetCursorState(cursorLocked);
          }
  
          private void SetCursorState(bool newState)
          {
              Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
          }*/

#endif
    }
}