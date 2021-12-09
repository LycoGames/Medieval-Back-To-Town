using UnityEngine;
public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSmoothTime = 0.2f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    float moveSpeed;
    bool isFalling;
    bool isRunPressed;
    bool isBasicAttackPressed;
    Animator anim;
    float turnSmoothVelocity;
    Vector3 moveDirection;
    Vector3 velocity;

    CharacterController controller;

    float angle;

    // state Variables
    PlayerBaseState currentState;
    PlayerStateFactory states;

    //getters and setters
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public CharacterController CharacterController { get { return controller; } set { controller = value; } }
    public Animator Animator { get { return anim; } }

    public bool IsRunPressed { get { return isRunPressed; } }
    public bool IsBasicAttackPressed { get { return isBasicAttackPressed; } }
    public bool IsMovementPressed { get { return moveDirection != Vector3.zero; } }

    public float WalkSpeed { get { return walkSpeed; } }
    public float RunSpeed { get { return runSpeed; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float RotationAngle { get { return angle; } }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        states = new PlayerStateFactory(this);

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
        //Run key listener
        isRunPressed = Input.GetKey(runKey);

        isBasicAttackPressed = Input.GetMouseButtonDown(0);

        //check is grounded
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        //take player movement inputs
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

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
}
