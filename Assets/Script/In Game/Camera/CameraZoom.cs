using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private PlayerInput inputProvider;
        [SerializeField] private float defaultDistance = 6f;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxtDistance = 6f;
        [SerializeField] private float smoothing = 4f;
        [SerializeField] private float zoomSensivity = 1f;
        private CinemachinePositionComposer framingTransposer;
        private float currentTargetDistance;

        private void Awake()
        {
            framingTransposer = GameObject.Find("FollowCamera").GetComponent<CinemachinePositionComposer>();
            currentTargetDistance = defaultDistance;
            inputProvider.playerActions.PlayerZoom.performed += Zoom;
        }
        private void OnDestroy()
        {
            inputProvider.playerActions.PlayerZoom.performed -= Zoom;
        }
        private void Update()
        {

        }

        private void Zoom(InputAction.CallbackContext context)
        {
            var inputAxis = context.ReadValue<Single>();
            float zoomValue = inputAxis* zoomSensivity;

            currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minDistance, maxtDistance);

            float currentDistance = framingTransposer.CameraDistance;
            if (currentDistance == currentTargetDistance) return;
            float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);
            framingTransposer.CameraDistance = lerpedZoomValue;
        }
    }
}
