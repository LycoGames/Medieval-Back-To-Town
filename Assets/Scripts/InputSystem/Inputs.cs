using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

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

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnInteraction(InputValue value)
        {
            InteractionInput(value.isPressed);
        }

        public void OnRoll(InputValue value)
        {
            RollInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnBasicAttack(InputValue value)
        {
            BasicAttackInput(value.isPressed);
        }

        public void OnInventoryShowHideInput(InputValue value)
        {
            InventoryShowHideInput(value.isPressed);
        }
#else
        // old input sys if we do decide to have it (most likely wont)...
#endif

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