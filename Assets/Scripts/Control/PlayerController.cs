using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using RPG.Core;

public class PlayerController : MonoBehaviour, IPunObservable
{
    public static PlayerController localPlayer;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSmoothTime = 0.2f;

    private float turnSmoothVelocity;
    private float moveSpeed;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private Vector3 moveDir;
    private GameObject myFollowCamera;
    private Camera myCamera;
    float angle;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    private CharacterController controller;
    private Animator anim;

    //Networking

    PhotonView myPV;

    private void Start()
    {
        myPV = GetComponent<PhotonView>();
        if (myPV.IsMine)
        {
            localPlayer = this;
        }
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        myFollowCamera = transform.parent.GetChild(1).gameObject;
        myCamera = transform.parent.GetChild(2).GetComponent<Camera>();

        if (!myPV.IsMine)
        {
            myFollowCamera.gameObject.SetActive(false);
            myCamera.gameObject.SetActive(false);
            return;
        }
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        if (!myPV.IsMine) { return; }

        if (HandleAttack()) return;

        Move();
    }

    private bool HandleAttack()
    {
        return isGrounded && GetComponent<Fighter>().AttackBehaviour();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(moveX, 0, moveZ).normalized;


        if (isGrounded)
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngel = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngel, ref turnSmoothVelocity, isGrounded ? turnSmoothTime : turnSmoothTime * 2);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngel, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void Idle()
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", moveSpeed, 0.1f, Time.deltaTime);
    }
    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", moveSpeed, 0.1f, Time.deltaTime);
    }
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        isGrounded = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(angle);
        }
        else
        {
            angle = (float)stream.ReceiveNext();
        }
    }
}