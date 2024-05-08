using Cinemachine;
using UnityEngine;

namespace BleizEntertainment
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float defaultDistance = 6f;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxtDistance = 6f;
        [SerializeField] private float smoothing = 4f;
        [SerializeField] private float zoomSensivity = 1f;
        private CinemachineFramingTransposer framingTransposer;
        private CinemachineInputProvider inputProvider;
        private float currentTargetDistance;

        private void Awake()
        {
            framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            inputProvider = GetComponent<CinemachineInputProvider>();
            currentTargetDistance = defaultDistance;
        }

        private void Update()
        {
            Zoom();
        }

        private void Zoom()
        {
            float zoomValue = inputProvider.GetAxisValue(2) * zoomSensivity;

            currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minDistance, maxtDistance);

            float currentDistance = framingTransposer.m_CameraDistance;
            if (currentDistance == currentTargetDistance) return;
            float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);
            framingTransposer.m_CameraDistance = lerpedZoomValue;
        }
    }
}
