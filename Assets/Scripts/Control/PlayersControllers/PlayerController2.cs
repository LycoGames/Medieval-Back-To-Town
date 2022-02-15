using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using RPG.Core;
using RPG.Saving;
using UnityEngine.EventSystems;

public abstract class PlayerController2 : MonoBehaviour, ISaveable
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSmoothTime = 0.2f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private CameraRotater cameraRotater;


    private bool isAttackPressed = false;
    private bool isRunnning = false;
    private bool isJumpPressed = false;

    private Animator anim;
    private float turnSmoothVelocity;
    private float moveSpeed;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private Vector3 moveDir;

    private Camera myCamera;
    private Health health;

    float timeOfAnimation;

    private CharacterController controller;

    float angle;

    bool isAttacking = false;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        timeOfAnimation = GetComponent<Animator>().runtimeAnimatorController.animationClips[3].length;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        InputListener();
        if (health.IsDead()) return;

        if (!InteractWithUI())
        {
            if (HandleAttack()) return;
        }

        if (anim.GetBool("isAiming")) return;

        if (!isAttacking)
        {
            Move();
            Jump();
        }
    }

    private bool InteractWithUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            cameraRotater.FreezeCameraRotation();
            return true;
        }

        cameraRotater.UnfreezeCameraRotation();
        return false;
    }


    private bool HandleAttack()
    {
        if (Attack()) return true;

        return false;
    }

    private void Move()
    {
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (isGrounded)
        {
            if (Walk())
            {
            }
            else if (Run())
            {
            }
            else if (Idle())
            {
            }

            moveDirection *= moveSpeed;
        }

        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            UpdateRotation();
            moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    private void UpdateRotation()
    {
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg +
                            Camera.main.transform.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
            isGrounded ? turnSmoothTime : turnSmoothTime * 3);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            isGrounded = false;
        }
    }

    private bool Idle()
    {
        if (moveDirection == Vector3.zero)
        {
            anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            return true;
        }

        return false;
    }

    private bool Walk()
    {
        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
            anim.SetFloat("Speed", moveSpeed, 0.1f, Time.deltaTime);
            return true;
        }

        return false;
    }

    private bool Run()
    {
        if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
            anim.SetFloat("Speed", moveSpeed, 0.1f, Time.deltaTime);
            return true;
        }

        return false;
    }

    public virtual bool Attack()
    {
        if (isAttackPressed)
        {
            StartCoroutine(WaitTillAttackEnd());

            return true;
        }

        return false;
    }

    private IEnumerator WaitTillAttackEnd()
    {
        StartAttacking();
        yield return new WaitForSeconds(timeOfAnimation);
        StopAttacking();
    }

    private void StartAttacking()
    {
        anim.SetFloat("Speed", 0, 0.5f, Time.deltaTime);
        anim.SetTrigger("attack");
        anim.applyRootMotion = true;
        isAttacking = true;
    }

    private void StopAttacking()
    {
        isAttacking = false;
        anim.applyRootMotion = false;
        anim.ResetTrigger("attack");
    }

    private void InputListener()
    {
        if (Input.GetKeyDown(attackKey))
            isAttackPressed = true;

        else if (Input.GetKeyUp(attackKey))
            isAttackPressed = false;

        if (Input.GetKeyDown(jumpKey))
            isJumpPressed = true;
        else if (Input.GetKeyUp(jumpKey))
            isJumpPressed = false;

        if (Input.GetKeyDown(runKey))
            isRunnning = true;
        else if (Input.GetKeyUp(runKey))
            isRunnning = false;

        /* if (Input.GetKeyDown(KeyCode.I))
         {
             if (Cursor.lockState == CursorLockMode.Locked)
             {
                 Cursor.lockState = CursorLockMode.None;
             }
             else
             {
                 Cursor.lockState = CursorLockMode.Locked;
             }
         }*/
    }

    public object CaptureState()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["position"] = new SerializableVector3(transform.position);
        Debug.Log(transform.position);
        data["rotation"] = new SerializableVector3(transform.eulerAngles);
        return data;
    }

    public void RestoreState(object state)
    {
        Dictionary<string, object> data = (Dictionary<string, object>) state;
        transform.position = ((SerializableVector3) data["position"]).ToVector();
        transform.eulerAngles = ((SerializableVector3) data["rotation"]).ToVector();
    }
}