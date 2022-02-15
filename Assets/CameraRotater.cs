using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraRotater : MonoBehaviour
{
    private CinemachineFreeLook freeLookCamera;
    private float xAxisMaxSpeed;
    private float yAxisMaxSpeed;

    private void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        xAxisMaxSpeed = freeLookCamera.m_XAxis.m_MaxSpeed;
        yAxisMaxSpeed = freeLookCamera.m_YAxis.m_MaxSpeed;
    }

    public void FreezeCameraRotation()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0;
    }

    public void UnfreezeCameraRotation()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = xAxisMaxSpeed;
        freeLookCamera.m_YAxis.m_MaxSpeed = yAxisMaxSpeed;
    }
}