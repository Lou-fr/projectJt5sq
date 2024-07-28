using System;
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
        private GeneralReusableSettingsData settingsData;
        private InputMaster.PlayerActions playerAction;
        private CinemachinePositionComposer framingTransposer;
        private float currentTargetDistance;
        public void Init(InputMaster.PlayerActions playeraction)
        {
            framingTransposer = GameObject.Find("FollowCamera").GetComponent<CinemachinePositionComposer>();
            currentTargetDistance = defaultDistance;
            settingsData = new GeneralReusableSettingsData();//GetComponent<Player>().userSettingsData;
            playerAction = playeraction;
            playerAction.PlayerZoom.performed += Zoom;
        }
        private void OnDestroy()
        {
            playerAction.PlayerZoom.performed -= Zoom;
        }

        private void Zoom(InputAction.CallbackContext context)
        {
            var inputAxis = context.ReadValue<Single>();
            float zoomValue = inputAxis* settingsData.UserZoomSensivity;

            currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minDistance, maxtDistance);

            float currentDistance = framingTransposer.CameraDistance;
            if (currentDistance == currentTargetDistance) return;
            float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);
            framingTransposer.CameraDistance = lerpedZoomValue;
        }
    }
}
