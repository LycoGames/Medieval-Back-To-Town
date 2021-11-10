using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    [SerializeField] float turnSpeed = 15;
    [SerializeField] float aimDuration = 0.3f;
    [SerializeField] Transform cameraLookAt;
    [SerializeField] Cinemachine.AxisState xAxis;
    [SerializeField] Cinemachine.AxisState yAxis;
    // Start is called before the first frame update
    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;   
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.deltaTime);
    }
}
