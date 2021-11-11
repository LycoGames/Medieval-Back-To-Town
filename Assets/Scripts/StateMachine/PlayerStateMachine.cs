using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool isJumpPressed;
    private bool isFalling;
    private bool isRunPressed;
    private Animator anim;
    private float turnSmoothVelocity;
    private float moveSpeed;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private Vector3 moveDir;

    private Camera myCamera;
    private Health health;

    private CharacterController controller;

    float angle;


    // state Variables
    PlayerBaseState currentState;
    PlayerStateFactory states;

    //getters and setters
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public CharacterController characterController { get { return controller; } set { controller = value; } }
    public Animator Animator { get { return anim; } }
    public bool IsJumpPressed { get { return isJumpPressed; } }
    public bool IsRunPressed { get { return isRunPressed; } }
    public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
    public bool IsFalling { get { return isFalling; } set { isFalling = value; } }
    public float VelocityY { get { return velocity.y; } set { velocity.y = value; } }
    public Vector3 Velocity { get { return velocity; } }
    public bool IsMovementPressed { get { return moveDirection != Vector3.zero; } }
    public float JumpHeight { get { return jumpHeight; } }
    public float Gravity { get { return gravity; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float WalkSpeed { get { return walkSpeed; } }
    public float RunSpeed { get { return runSpeed; } }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        Listener();
        currentState.UpdateStates();
        ApplyGravity();
        HandleMovement();
    }

    private void Listener()
    {
        isJumpPressed = Input.GetKeyDown(jumpKey);
        isRunPressed = Input.GetKey(runKey);
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

    }

    private void HandleMovement()
    {
        moveDirection *= moveSpeed;
        if (moveDirection.magnitude >= 0.1f)
        {
            moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            UpdateRotation();
        }
    }

    private void UpdateRotation()
    {
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, isGrounded ? turnSmoothTime : turnSmoothTime * 3);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
