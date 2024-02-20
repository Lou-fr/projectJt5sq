using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonZoom : MonoBehaviour
{
    [SerializeField] private BleizInputManager _input;
    [SerializeField] private float ZoomSpeed = 1.0f ;
    [SerializeField] private CinemachineFreeLook freeLook;
    [SerializeField] private float zoomAceleration = 2.5f;
    [SerializeField] private float zoomInnerRange = 3;
    [SerializeField] private float zoomOuterRange = 10;

    private float cureentMiddleRigRadius = 10f;
    private float newMiddleRigRadius = 10f;
    private void Update()
    {
        AdjustCameraZoomIndex();
    }
    private void LateUpdate()
    {
        UpdateZoomLevel();
    }
    private void UpdateZoomLevel()
    {
        if (cureentMiddleRigRadius == newMiddleRigRadius) { return; }
        cureentMiddleRigRadius = Mathf.Lerp(cureentMiddleRigRadius, newMiddleRigRadius, zoomAceleration * Time.deltaTime);
        cureentMiddleRigRadius = Mathf.Clamp(cureentMiddleRigRadius, zoomInnerRange, zoomOuterRange);

        freeLook.m_Orbits[1].m_Radius = cureentMiddleRigRadius;

    }
    public void AdjustCameraZoomIndex()
    {
        if (_input.ZoomCameraInput == 0.0f) { return; }

        if (_input.ZoomCameraInput < 0)
        {
            newMiddleRigRadius = cureentMiddleRigRadius + ZoomSpeed;
        }
        if (_input.ZoomCameraInput > 0)
        {
            newMiddleRigRadius = cureentMiddleRigRadius - ZoomSpeed;
        }
    }
}
