using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;
    CinemachineVirtualCamera aimCamera;
    Animator animator;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        aimCamera = GetComponent<CinemachineVirtualCamera>();
        aimCanvas.enabled = false;
    }
    private void Update()
    {
        //Rotate();
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartAim();

        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            CancelAim();
        }
    }

    private void StartAim()
    {
        //thirdCam.SetActive(false);
        //"aimCamera.SetActive(true);
        aimCamera.Priority += 2;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
        animator.SetBool("isAiming", true);
        animator.applyRootMotion = true;
        //print("holding right 1");
    }

    private void CancelAim()
    {
        //thirdCam.SetActive(true);
        //aimCamera.SetActive(false);
        aimCamera.Priority -= 2;
        thirdPersonCanvas.enabled = true;
        aimCanvas.enabled = false;
        animator.SetBool("isAiming", false);
        animator.applyRootMotion = false;
        //print("released right 2");
    }
}
