using UnityEngine;
using Cinemachine;
public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSmoothTime = 0.2f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    //Aim Variables
    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;
    [SerializeField] CinemachineVirtualCamera aimCamera;
    [SerializeField] Cinemachine.AxisState xAxis;
    [SerializeField] Cinemachine.AxisState yAxis;
    [Header("Aiming Sens")]
    [SerializeField] float turnSpeed = 15;
    [SerializeField] float aimDuration = 0.3f;
    [SerializeField] Transform cameraLookAt;
    [Header("Aiming Movement")]
    [SerializeField] float acceleration = 2f;
    [SerializeField] float deceleration = 2f;
    [SerializeField] float maximumWalkVelocity = 0.5f;
    [SerializeField] float maximumRunVelocity = 2f;

    private Fighter fighter;
    private float moveSpeed;
    private bool isJumpPressed;
    private bool isBasicAttackPressed;
    private bool isFalling;
    private bool isRunPressed;
    private Animator anim;
    private float turnSmoothVelocity;
    //private float moveSpeed;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private Vector3 moveDir;
    private Camera mainCamera;

    private Camera myCamera;
    private Health health;

    private CharacterController controller;
    private Transform playerTransform;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    int velocityZHash;
    int velocityXHash;

    float angle;

    // state Variables
    PlayerBaseState currentState;
    PlayerStateFactory states;

    //getters and setters
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public PlayerStateFactory States { get { return states; } }
    public CharacterController CharacterController { get { return controller; } set { controller = value; } }
    public Animator Animator { get { return anim; } }
    public Fighter Fighter { get { return fighter; } }
    public bool IsJumpPressed { get { return isJumpPressed; } }
    public bool IsRunPressed { get { return isRunPressed; } }
    public bool IsBasicAttackPressed { get { return isBasicAttackPressed; } }
    public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
    public bool IsFalling { get { return isFalling; } set { isFalling = value; } }
    public bool IsAimPressed { get { return IsAiming(); } }
    public bool IsAimReleased { get { return IsAimingStopped(); } }
    public float VelocityY { get { return velocity.y; } set { velocity.y = value; } }
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
    public Vector3 MoveDirection { get { return moveDirection; } }
    public bool IsMovementPressed { get { return moveDirection != Vector3.zero; } }
    public float JumpHeight { get { return jumpHeight; } }
    public float Gravity { get { return gravity; } }
    //public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float WalkSpeed { get { return walkSpeed; } }
    public float RunSpeed { get { return runSpeed; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public int VelocityZHash { get { return velocityZHash; } set { velocityZHash = value; } }
    public int VelocityXHash { get { return velocityXHash; } set { velocityXHash = value; } }
    public float VelocityZ { get { return velocityZ; } set { velocityZ = value; } }
    public float VelocityX { get { return velocityX; } set { velocityX = value; } }

    public Cinemachine.CinemachineVirtualCamera AimCamera { get { return aimCamera; } }
    public Canvas AimCanvas { get { return aimCanvas; } }
    public Canvas ThirdPersonCanvas { get { return thirdPersonCanvas; } }
    public Cinemachine.AxisState XAxis { get { return xAxis; } }
    public Cinemachine.AxisState YAxis { get { return yAxis; } }
    public float TurnSpeed { get { return turnSpeed; } set { turnSpeed = value; } }
    public float AimDuration { get { return aimDuration; } set { aimDuration = value; } }
    public Transform CameraLookAt { get { return cameraLookAt; } }
    public Camera MainCamera { get { return mainCamera; } }
    public Transform PlayerTransform { get { return playerTransform; } }
    public float Acceleration { get { return acceleration; } }
    public float Deceleration { get { return deceleration; } }
    public float MaximumRunVelocity { get { return maximumRunVelocity; } }
    public float MaximumWalkVelocity { get { return maximumWalkVelocity; } }
    public float RotationAngle { get { return angle; } }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        playerTransform = transform;
        fighter = GetComponent<Fighter>();
        mainCamera = Camera.main;
        states = new PlayerStateFactory(this);
        aimCanvas.enabled = false;
        currentState = states.Grounded();
        currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        Listener();
        UpdateRotationAngle();
        currentState.UpdateStates();
        ApplyGravity();
    }

    private void Listener()
    {
        //Jump key Listener
        isJumpPressed = Input.GetKeyDown(jumpKey);

        //Run key listener
        isRunPressed = Input.GetKey(runKey);

        //Basic attack key listener
        isBasicAttackPressed = Input.GetKeyDown(KeyCode.Mouse0);

        //check is grounded
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        //take player movement inputs
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        //Mouse Pos
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

    }

    private void UpdateRotationAngle()
    {
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, isGrounded ? turnSmoothTime : turnSmoothTime * 3);
        //transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private bool IsAiming()
    {
        return (Input.GetKeyDown(KeyCode.Mouse1));
    }

    private bool IsAimingStopped()
    {
        return (Input.GetKeyUp(KeyCode.Mouse1));
    }

    /*private void StartAim()
    {
        aimCamera.Priority += 2;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
        animator.SetBool("isAiming", true);
        animator.applyRootMotion = true;
        //print("holding right 1");
    }

    private void CancelAim()
    {
        aimCamera.Priority -= 2;
        thirdPersonCanvas.enabled = true;
        aimCanvas.enabled = false;
        animator.SetBool("isAiming", false);
        animator.applyRootMotion = false;
        //print("released right 2");
    }*/
}
