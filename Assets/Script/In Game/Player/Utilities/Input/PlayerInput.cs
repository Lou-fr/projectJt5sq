using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerInput : MonoBehaviour
    {
        public InputMaster inputActions { get; private set; }
        public InputMaster.PlayerActions playerActions { get; private set; }
        private void Awake()
        {
            inputActions = new InputMaster();
            playerActions = inputActions.player;
            gameObject.GetComponent<CameraZoom>().Init(playerActions);
        }
        private void OnEnable()
        {
            inputActions.Enable();
        }
        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void DisableActionFor(InputAction action, float seconds)
        {
            StartCoroutine(DisableAction(action, seconds));
        }
        private IEnumerator DisableAction(InputAction action, float seconds)
        {
            action.Disable();

            yield return new WaitForSeconds(seconds);

            action.Enable();
        }
    }
}
