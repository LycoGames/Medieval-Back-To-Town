using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject thirdCam;
    [SerializeField] GameObject aimCamera;

    private Transform parent;
    private void Start()
    {
        parent = transform.parent;
        aimCamera.SetActive(false);
    }
    private void Update()
    {
        Rotate();
        if (Input.GetMouseButton(1) && !aimCamera.activeInHierarchy)
        {
            thirdCam.SetActive(false);
            aimCamera.SetActive(true);
            print("holding right 1");

        }
        else if (!Input.GetMouseButton(1) && !thirdCam.activeInHierarchy)
        {
            thirdCam.SetActive(true);
            aimCamera.SetActive(false);
            print("released right 2");
        }
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    }



}
