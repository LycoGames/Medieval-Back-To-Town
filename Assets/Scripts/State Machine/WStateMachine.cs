using InputSystem;
using UnityEngine;

public class WStateMachine : MonoBehaviour
{
    //TODO console çıktılarını in game hale getirilecek ekrananın sağ tarafından akacak
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
        get { return currentState; }
        set { currentState = value; }
    }

    public WStateFactory States
    {
        get { return states; }
    }

    public Inputs Input
    {
        get { return input; }
    }

    //Camera
    public Camera MainCamera
    {
        get { return mainCamera; }
    }

    public bool LockCameraPosition
    {
        get { return lockCameraPosition; }
    }

    public GameObject CinemachineCameraTarget
    {
        get { return cinemachineCameraTarget; }
    }

    public float TopClamp
    {
        get { return topClamp; }
    }

    public float BottomClamp
    {
        get { return bottomClamp; }
    }

    public float CameraAngleOverride
    {
        get { return cameraAngleOverride; }
    }

    public float GroundedOffset
    {
        get { return groundedOffset; }
    }

    public float GroundedRadius
    {
        get { return groundedRadius; }
    }

    public bool Grounded
    {
        get { return grounded; }
        set { grounded = value; }
    }


    public LayerMask GroundLayers
    {
        get { return groundLayers; }
    }

    public bool HasAnimator
    {
        get { return hasAnimator; }
    }

    public Animator Animator
    {
        get { return animator; }
    }

    //animation

    public int AnimIDSpeed
    {
        get { return animIDSpeed; }
    }

    public int AnimIDGrounded
    {
        get { return animIDGrounded; }
    }

    public int AnimIDJump
    {
        get { return animIDJump; }
    }

    public int AnimIDFreeFall
    {
        get { return animIDFreeFall; }
    }

    public int AnimIDMotionSpeed
    {
        get { return animIDMotionSpeed; }
    }public int AnimIDInCombat
    {
        get { return animIDInCombat; }
    }

    public float FallTimeout
    {
        get { return fallTimeout; }
    }

    //player
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float AnimationBlend
    {
        get { return animationBlend; }
        set { animationBlend = value; }
    }

    public float TargetRotation
    {
        get { return targetRotation; }
        set { targetRotation = value; }
    }

    public float RotationVelocity
    {
        get { return rotationVelocity; }
        set { rotationVelocity = value; }
    }

    public float RotationSmoothTime
    {
        get { return rotationSmoothTime; }
    }

    public float VerticalVelocity
    {
        get { return verticalVelocity; }
        set { verticalVelocity = value; }
    }

    public float TerminalVelocity
    {
        get { return terminalVelocity; }
        set { terminalVelocity = value; }
    }

    public float JumpTimeoutDelta
    {
        get { return jumpTimeoutDelta; }
        set { jumpTimeoutDelta = value; }
    }

    public float FallTimeoutDelta
    {
        get { return fallTimeoutDelta; }
        set { fallTimeoutDelta = value; }
    }

    public float TargetSpeed
    {
        get { return targetSpeed; }
        set { targetSpeed = value; }
    }

    public float SpeedChangeRate
    {
        get { return speedChangeRate; }
        set { speedChangeRate = value; }
    }

    //Jump And Gravity
    public float JumpHeight
    {
        get { return jumpHeight; }
    }

    public float Gravity
    {
        get { return gravity; }
    }

    public float JumpTimeout
    {
        get { return jumpTimeout; }
    }

    //Move
    public CharacterController Controller
    {
        get { return controller; }
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