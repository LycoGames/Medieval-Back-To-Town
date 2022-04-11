using InputSystem;
using UnityEngine;

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

    [Space(10)] [Tooltip("The height the player can jump")] [SerializeField]
    private float jumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")] [SerializeField]
    private float gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    [SerializeField]
    private float jumpTimeout = 0.50f;

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
    private WBaseState currentState;
    private WBaseState currentSubState;
    private WStateFactory states;

    //player
    private float speed;
    private float animationBlend;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;
    private float targetSpeed;

    //animation IDs
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;
    private int animIDInCombat;

    //Jump
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    private Animator animator;
    private CharacterController controller;
    private Camera mainCamera;
    private Inputs input;

    private bool hasAnimator;


    //getters and setters

    public WBaseState CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    public WStateFactory States
    {
        get => states;
        set => states = value;
    }

    public Inputs Input
    {
        get => input;
        set => input = value;
    }

    //Camera
    public Camera MainCamera
    {
        get => mainCamera;
        set => mainCamera = value;
    }

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

    public bool HasAnimator
    {
        get => hasAnimator;
        set => hasAnimator = value;
    }

    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    //animation

    public int AnimIDSpeed
    {
        get => animIDSpeed;
        set => animIDSpeed = value;
    }

    public int AnimIDGrounded
    {
        get => animIDGrounded;
        set => animIDGrounded = value;
    }

    public int AnimIDJump
    {
        get => animIDJump;
        set => animIDJump = value;
    }

    public int AnimIDFreeFall
    {
        get => animIDFreeFall;
        set => animIDFreeFall = value;
    }

    public int AnimIDMotionSpeed
    {
        get => animIDMotionSpeed;
        set => animIDMotionSpeed = value;
    }

    public int AnimIDInCombat
    {
        get => animIDInCombat;
        set => animIDInCombat = value;
    }

    public float FallTimeout
    {
        get => fallTimeout;
        set => fallTimeout = value;
    }

    //player
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float AnimationBlend
    {
        get => animationBlend;
        set => animationBlend = value;
    }

    public float TargetRotation
    {
        get => targetRotation;
        set => targetRotation = value;
    }

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

    public float VerticalVelocity
    {
        get => verticalVelocity;
        set => verticalVelocity = value;
    }

    public float TerminalVelocity
    {
        get => terminalVelocity;
        set => terminalVelocity = value;
    }

    public float JumpTimeoutDelta
    {
        get => jumpTimeoutDelta;
        set => jumpTimeoutDelta = value;
    }

    public float FallTimeoutDelta
    {
        get => fallTimeoutDelta;
        set => fallTimeoutDelta = value;
    }

    public float TargetSpeed
    {
        get => targetSpeed;
        set => targetSpeed = value;
    }

    public float SpeedChangeRate
    {
        get => speedChangeRate;
        set => speedChangeRate = value;
    }

    //Jump And Gravity
    public float JumpHeight
    {
        get => jumpHeight;
        set => jumpHeight = value;
    }

    public float Gravity
    {
        get => gravity;
        set => gravity = value;
    }

    public float JumpTimeout
    {
        get => jumpTimeout;
        set => jumpTimeout = value;
    }

    //Move
    public CharacterController Controller
    {
        get => controller;
        set => controller = value;
    }

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        states = new WStateFactory(this);
        currentState = states.WAppState();
        currentState.EnterState();
    }

    void Start()
    {
        hasAnimator = TryGetComponent(out animator);
        controller = GetComponent<CharacterController>();
        input = GetComponent<Inputs>();

        AssignAnimationIDs();

        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }

    void Update()
    {
        currentState.UpdateStates();
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");        
        animIDInCombat = Animator.StringToHash("InCombat");
        
    }

    public void SetSpeedToIdle()
    {
        targetSpeed = 0f;
    }

    public void SetSpeedToWalk()
    {
        targetSpeed = moveSpeed;
    }

    public void SetSpeedToRun()
    {
        targetSpeed = sprintSpeed;
    }

    public void Move()
    {
        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        // move the player
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                        new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }

    public void RotatePlayerToMoveDirection()
    {
        // normalise input direction
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving

        targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                         mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
            ref rotationVelocity,
            rotationSmoothTime);

        // rotate to face input direction relative to camera position
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }
}