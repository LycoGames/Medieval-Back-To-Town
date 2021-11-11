using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using RPG.Core;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSmoothTime = 0.2f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (health.IsDead()) return;

        if (HandleAttack()) return;
        if (anim.GetBool("isAiming"))
        {
            return;
        }
        Move();
        Jump();
    }

    private bool HandleAttack()
    {
        return isGrounded && GetComponent<Fighter>().AttackBehaviour();
    }

    private void Move()
    {
        

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(moveX, 0, moveZ).normalized;


        if (isGrounded)
        {
            if (Walk()) { }
            else if (Run()) { }
            else if (Idle()) { }

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
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, isGrounded ? turnSmoothTime : turnSmoothTime * 3);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void ApplyGravity()
    {
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
}