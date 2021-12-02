using UnityEngine;

public class PlayerAimState : PlayerBaseState
{
    public PlayerAimState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        //Change current camera to aim camera
        Debug.Log("Aim state entered");
        Ctx.AimCamera.Priority += 2;
        Ctx.AimCanvas.enabled = true;
        Ctx.ThirdPersonCanvas.enabled = false;
        Ctx.Animator.SetBool("isAiming", true);
        Ctx.Animator.applyRootMotion = true;

        Ctx.VelocityZHash = Animator.StringToHash("Velocity Z");
        Ctx.VelocityXHash = Animator.StringToHash("Velocity X");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, Ctx.AimColliderLayerMask))
        {
            Ctx.DebugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        Ctx.CameraLookAt.eulerAngles = new Vector3(Ctx.YAxis.Value, Ctx.XAxis.Value, 0);

        /*float yawCamera = Ctx.MainCamera.transform.rotation.eulerAngles.y;
        Ctx.PlayerTransform.rotation = Quaternion.Slerp(Ctx.PlayerTransform.rotation, Quaternion.Euler(0, yawCamera, 0), Ctx.TurnSpeed * Time.deltaTime);*/

        // aim alırken mouse pozisyonuna karakteri çevirme

        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = Ctx.transform.position.y;
        Vector3 aimDirection = (worldAimTarget - Ctx.transform.position).normalized;

        Ctx.transform.forward = Vector3.Lerp(Ctx.transform.forward, aimDirection, Time.deltaTime * 20f);

        //MOVEMENT AND ANIMATIONS
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        float currentMaxVelocity = runPressed ? Ctx.MaximumRunVelocity : Ctx.MaximumWalkVelocity;

        ChangeVelocity(forwardPressed, backPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);
        lockOrResetVelocity(forwardPressed, backPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);

        // set the parameters to our local variable values
        Ctx.Animator.SetFloat(Ctx.VelocityZHash, Ctx.VelocityZ);
        Ctx.Animator.SetFloat(Ctx.VelocityXHash, Ctx.VelocityX);
    }

    public override void ExitState()
    {
        //Aim durumu sonlandığında aim kamerasından çıkma
        Ctx.AimCamera.Priority -= 2;
        Ctx.ThirdPersonCanvas.enabled = true;
        Ctx.AimCanvas.enabled = false;
        Ctx.Animator.SetBool("isAiming", false);
        Ctx.Animator.applyRootMotion = false;
    }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsAimReleased)
        {
            //Idle condition
            if (!Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Idle());
            }
            //Run Condition
            else if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
            {
                SwitchState(Factory.Run());
            }
            //Walk condition
            else if (Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Walk());
            }
        }
    }

    /* AIM ALIRKEN DINLENEN VE KARAKTERE UYGULANAN VELOCITY DEGERLERI (AIM ALIRKEN HARAKET ETME) */

    void ChangeVelocity(bool forwardPressed, bool backPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {

        // if player presses forward, increate velocity in z direction
        if (forwardPressed && Ctx.VelocityZ < currentMaxVelocity)
        {
            Ctx.VelocityZ += Time.deltaTime * Ctx.Acceleration;
        }

        if (backPressed && Ctx.VelocityZ > -currentMaxVelocity)
        {
            Ctx.VelocityZ -= Time.deltaTime * Ctx.Acceleration;
        }

        // increase velocity in left direction
        if (leftPressed && Ctx.VelocityX > -currentMaxVelocity)
        {
            Ctx.VelocityX -= Time.deltaTime * Ctx.Acceleration;
        }

        //increase velocity in right direction
        if (rightPressed && Ctx.VelocityX < currentMaxVelocity)
        {
            Ctx.VelocityX += Time.deltaTime * Ctx.Acceleration;
        }

        // decrease Ctx.VelocityZ
        if (!forwardPressed && Ctx.VelocityZ > 0)
        {
            Ctx.VelocityZ -= Time.deltaTime * Ctx.Deceleration;
        }

        if (!backPressed && Ctx.VelocityZ < 0)
        {
            Ctx.VelocityZ += Time.deltaTime * Ctx.Deceleration;
        }

        //increase Ctx.VelocityX if left is not pressed and Ctx.VelocityX < 0
        if (!leftPressed && Ctx.VelocityX < 0)
        {
            Ctx.VelocityX += Time.deltaTime * Ctx.Deceleration;
        }

        //decrease Ctx.VelocityX if right is not pressed and Ctx.VelocityX > 0
        if (!rightPressed && Ctx.VelocityX > 0)
        {
            Ctx.VelocityX -= Time.deltaTime * Ctx.Deceleration;
        }
    }

    //handles reset and locking of velocity
    void lockOrResetVelocity(bool forwardPressed, bool backPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {

        // reset Ctx.VelocityZ
        if (!forwardPressed && !backPressed && Ctx.VelocityZ != 0 && (Ctx.VelocityZ > -0.05f && Ctx.VelocityZ < 0.05f))
        {
            Ctx.VelocityZ = 0f;
        }

        //reset Ctx.VelocityX
        if (!leftPressed && !rightPressed && Ctx.VelocityX != 0 && (Ctx.VelocityX > -0.05f && Ctx.VelocityX < 0.05f))
        {
            Ctx.VelocityX = 0f;
        }

        //locking forward
        if (forwardPressed && runPressed && Ctx.VelocityZ > currentMaxVelocity)
        {
            Ctx.VelocityZ = currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (forwardPressed && Ctx.VelocityZ > currentMaxVelocity)
        {
            Ctx.VelocityZ -= Time.deltaTime * Ctx.Deceleration;

            //round to the currentmaxvelocity if within offset
            if (Ctx.VelocityZ > currentMaxVelocity && Ctx.VelocityZ < (currentMaxVelocity + 0.05f))
            {
                Ctx.VelocityZ = currentMaxVelocity;
            }
        }

        //round to the currentMaxVelocity if within offset
        else if (forwardPressed && Ctx.VelocityZ < currentMaxVelocity && Ctx.VelocityZ > (currentMaxVelocity - 0.05f))
        {
            Ctx.VelocityZ = currentMaxVelocity;
        }

        //locking back
        if (backPressed && runPressed && Ctx.VelocityZ < -currentMaxVelocity)
        {
            Ctx.VelocityZ = -currentMaxVelocity;
        }

        else if (backPressed && Ctx.VelocityZ < -currentMaxVelocity)
        {
            Ctx.VelocityZ += Time.deltaTime * Ctx.Deceleration;

            //round to the currentmaxvelocity if within offset
            if (Ctx.VelocityZ < -currentMaxVelocity && Ctx.VelocityZ < (-currentMaxVelocity - 0.05f))
            {
                Ctx.VelocityZ = -currentMaxVelocity;
            }
        }

        //round to the currentMaxVelocity if within offset
        else if (backPressed && Ctx.VelocityZ > -currentMaxVelocity && Ctx.VelocityZ < (-currentMaxVelocity + 0.05f))
        {
            Ctx.VelocityZ = -currentMaxVelocity;
        }

        //locking left
        if (leftPressed && runPressed && Ctx.VelocityX < -currentMaxVelocity)
        {
            Ctx.VelocityX = -currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (leftPressed && Ctx.VelocityX < -currentMaxVelocity)
        {
            Ctx.VelocityX += Time.deltaTime * Ctx.Deceleration;

            //round to the currentmaxvelocity if within offset
            if (Ctx.VelocityX < -currentMaxVelocity && Ctx.VelocityX < (-currentMaxVelocity - 0.05f))
            {
                Ctx.VelocityX = -currentMaxVelocity;
            }
        }
        //round to the currentMaxVelocity if within offset
        else if (leftPressed && Ctx.VelocityX > -currentMaxVelocity && Ctx.VelocityX < (-currentMaxVelocity + 0.05f))
        {
            Ctx.VelocityX = -currentMaxVelocity;
        }

        //locking right
        if (rightPressed && runPressed && Ctx.VelocityX > currentMaxVelocity)
        {
            Ctx.VelocityX = currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (rightPressed && Ctx.VelocityX > currentMaxVelocity)
        {
            Ctx.VelocityX -= Time.deltaTime * Ctx.Deceleration;

            //round to the currentmaxvelocity if within offset
            if (Ctx.VelocityX > currentMaxVelocity && Ctx.VelocityX < (currentMaxVelocity + 0.05f))
            {
                Ctx.VelocityX = currentMaxVelocity;
            }
        }
        //round to the currentMaxVelocity if within offset
        else if (rightPressed && Ctx.VelocityX < currentMaxVelocity && Ctx.VelocityX > (currentMaxVelocity - 0.05f))
        {
            Ctx.VelocityX = currentMaxVelocity;
        }
    }
}
