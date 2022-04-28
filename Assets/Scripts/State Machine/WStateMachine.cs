using System;
using System.Collections.Generic;
using InputSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WStateMachine : MonoBehaviour
{
    [Header("Player")] [Tooltip("Move speed of the character in m/s")] [SerializeField]
    private float moveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")] [SerializeField]
    private float sprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)] [SerializeField]
    private float rotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")] [SerializeField]
    private float speedChangeRate = 10.0f;


    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")] [SerializeField]
    private float gravity = -15.0f;


    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")] [SerializeField]
    private float fallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    [SerializeField]
    private bool grounded = true;

    [Tooltip("Useful for rough ground")] [SerializeField]
    float groundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")] [SerializeField]
    float groundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")] [SerializeField]
    LayerMask groundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField]
    GameObject cinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")] [SerializeField]
    float topClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")] [SerializeField]
    float bottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    [SerializeField]
    float cameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")] [SerializeField]
    bool lockCameraPosition = false;

    //state variables
    private WBaseState currentSubState;

    //player
    private float rotationVelocity;

    //animation IDs

    //Jump

    private Animator animator;

    private ActionStore actionStore;


    //getters and setters

    public WBaseState CurrentState { get; set; }

    public WStateFactory States { get; set; }

    public Inputs Input { get; set; }

    //Camera
    public Camera MainCamera { get; set; }

    public bool LockCameraPosition
    {
        get => lockCameraPosition;
        set => lockCameraPosition = value;
    }

    public GameObject CinemachineCameraTarget
    {
        get => cinemachineCameraTarget;
        set => cinemachineCameraTarget = value;
    }

    public float TopClamp
    {
        get => topClamp;
        set => topClamp = value;
    }

    public float BottomClamp
    {
        get => bottomClamp;
        set => bottomClamp = value;
    }

    public float CameraAngleOverride
    {
        get => cameraAngleOverride;
        set => cameraAngleOverride = value;
    }

    public float GroundedOffset
    {
        get => groundedOffset;
        set => groundedOffset = value;
    }

    public float GroundedRadius
    {
        get => groundedRadius;
        set => groundedRadius = value;
    }

    public bool Grounded
    {
        get => grounded;
        set => grounded = value;
    }


    public LayerMask GroundLayers
    {
        get => groundLayers;
        set => groundLayers = value;
    }

    public bool HasAnimator { get; set; }

    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    //animation

    public int AnimIDSpeed { get; set; }

    public int AnimIDGrounded { get; set; }

    public int AnimIDJump { get; set; }

    public int AnimIDFreeFall { get; set; }

    public int AnimIDMotionSpeed { get; set; }

    public int AnimIDInCombat { get; set; }

    public float FallTimeout
    {
        get => fallTimeout;
        set => fallTimeout = value;
    }

    //player
    public float Speed { get; set; }

    public float AnimationBlend { get; set; }

    public float TargetRotation { get; set; } = 0.0f;

    public float RotationVelocity
    {
        get => rotationVelocity;
        set => rotationVelocity = value;
    }

    public float RotationSmoothTime
    {
        get => rotationSmoothTime;
        set => rotationSmoothTime = value;
    }

    public float VerticalVelocity { get; set; }

    public float TerminalVelocity { get; set; } = 53.0f;

    public float JumpTimeoutDelta { get; set; }

    public float FallTimeoutDelta { get; set; }

    public float TargetSpeed { get; set; }

    public float SpeedChangeRate
    {
        get => speedChangeRate;
        set => speedChangeRate = value;
    }

    public float Gravity
    {
        get => gravity;
        set => gravity = value;
    }

    //Move
    public CharacterController Controller { get; set; }

    public AIConversant InteractableNpc { get; set; }
    public bool CanMove { get; set; }

    public bool IsAttacking { get; set; }

    private const float Threshold = 0.01f;

    //cinemachine
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;


    public List<GameObject> MainUiArray { get; private set; }


    private void Awake()
    {
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }

        MainUiArray = new List<GameObject>();
        MainUiArray.AddRange(GameObject.FindGameObjectsWithTag("MainUI"));

        States = new WStateFactory(this);
        CurrentState = States.WAppState();
        CurrentState.EnterState();
    }

    void Start()
    {
        HasAnimator = TryGetComponent(out animator);
        Controller = GetComponent<CharacterController>();
        Input = GetComponent<Inputs>();
        actionStore = GetComponent<ActionStore>();

        AssignAnimationIDs();
        CanMove = true;
    }

    void Update()
    {
        CurrentState.UpdateStates();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        AnimIDSpeed = Animator.StringToHash("Speed");
        AnimIDGrounded = Animator.StringToHash("Grounded");
        AnimIDJump = Animator.StringToHash("Jump");
        AnimIDFreeFall = Animator.StringToHash("FreeFall");
        AnimIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        AnimIDInCombat = Animator.StringToHash("InCombat");
    }

    public void SetSpeedToIdle()
    {
        TargetSpeed = 0f;
    }

    public void SetSpeedToWalk()
    {
        TargetSpeed = moveSpeed;
    }

    public void SetSpeedToRun()
    {
        TargetSpeed = sprintSpeed;
    }

    public void Move()
    {
        if (!CanMove)
            return;
        Vector3 targetDirection = Quaternion.Euler(0.0f, TargetRotation, 0.0f) * Vector3.forward;

        // move the player
        Controller.Move(targetDirection.normalized * (Speed * Time.deltaTime) +
                        new Vector3(0.0f, VerticalVelocity, 0.0f) * Time.deltaTime);
    }

    public void RotatePlayerToMoveDirection()
    {
        // normalise input direction
        Vector3 inputDirection = new Vector3(Input.move.x, 0.0f, Input.move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving

        TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                         MainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetRotation,
            ref rotationVelocity,
            rotationSmoothTime);

        // rotate to face input direction relative to camera position
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (Input.look.sqrMagnitude >= Threshold && !LockCameraPosition)
        {
            cinemachineTargetYaw += Input.look.x * Time.deltaTime;
            cinemachineTargetPitch += Input.look.y * Time.deltaTime;
        }

        // clamp our rotations so our values are limited 360 degrees
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(
            cinemachineTargetPitch + cameraAngleOverride,
            cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void OnAbility1(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !IsAttacking)
        {
            actionStore.Use(0, gameObject);
        }
    }

    public void OnAbility2(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !IsAttacking)
        {
            actionStore.Use(1, gameObject);
        }
    }

    public void OnAbility3(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !IsAttacking)
        {
            actionStore.Use(2, gameObject);
        }
    }

    public void OnAbility4(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !IsAttacking)
        {
            actionStore.Use(3, gameObject);
        }
    }

    public void OnAbility5(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !IsAttacking)
        {
            actionStore.Use(4, gameObject);
        }
    }

    public void OnAbility6(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !IsAttacking)
        {
            actionStore.Use(5, gameObject);
        }
    }
}