using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float defaultDistance = 6f;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxtDistance = 6f;
        [SerializeField] private float smoothing = 4f;
        [SerializeField] private float zoomSensivity = 1f;
        private CinemachinePositionComposer framingTransposer;
        [SerializeField] private PlayerInput inputProvider;
        private float currentTargetDistance;

        private void Awake()
        {
            framingTransposer = GetComponent<CinemachinePositionComposer>();
            inputProvider = GetComponent<PlayerInput>();
            currentTargetDistance = defaultDistance;
        }

        private void Update()
        {
            Zoom();
        }

        private void Zoom()
        {
            var vector2 = inputProvider.playerActions.PlayerZoom.ReadValue<float>();
            float zoomValue = vector2* zoomSensivity;

            currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minDistance, maxtDistance);

            float currentDistance = framingTransposer.CameraDistance;
            if (currentDistance == currentTargetDistance) return;
            float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);
            framingTransposer.CameraDistance = lerpedZoomValue;
        }
    }
}
