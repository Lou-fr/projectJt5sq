using System;
using UnityEngine;

namespace BleizEntertainment
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        [field: Header("references")]
        [field: SerializeField] public PlayerSO data { get; private set; }
        [field: Header("Collision")]
        [field: SerializeField] public PlayerCapsuleCollidersUtilities collidersUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData layerData { get; private set; }
        [field: Header("Animation")]
        [field: SerializeField] public PlayerAnimationData animationData { get; private set; }
        public Rigidbody rigidBody { get; private set; }
        public Animator animator { get; private set; }
        public Transform mainCameraTransform { get; private set; }
        public PlayerInput Input { get; private set; }
        private PlayerStateMachine playerStateMachine;
        private void Awake()
        {
            WeaponsDataClass weaponsDataClass = data.combatData.WeaponsDataClass;
            rigidBody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            playerStateMachine = new PlayerStateMachine(this);
            mainCameraTransform = Camera.main.transform;
            animationData.Initialize();
            collidersUtility.CapsuleCollidersUtility.Initialize(gameObject);
            Cursor.lockState = CursorLockMode.Locked;
            if (weaponsDataClass.IsMelee)
            {
                if (weaponsDataClass.IsHeavy) {playerStateMachine.PlayerCombatHeavyMeleeInstantiate(); return; }
                playerStateMachine.PlayerCombatMeleeInstantiate();return;
            }
        }
        private void OnDestroy()
        {
            Input.playerActions.UnlockCursor.performed -= Unlockcursor;
            Input.playerActions.UnlockCursor.canceled -= Lockcursor;
        }

        private void OnTriggerEnter(Collider collider)
        {
            playerStateMachine.OnTriggerEnter(collider);
        }
        private void OnTriggerExit(Collider collider)
        {
            playerStateMachine.OnTriggerExit(collider);
        }
        private void OnValidate()
        {
            collidersUtility.CapsuleCollidersUtility.Initialize(gameObject);
            collidersUtility.CapsuleCollidersUtility.CalculateCapsuleColliderDimension();
        }
        private void Start()
        {
            Input = GetComponent<PlayerInput>();
            Input.playerActions.UnlockCursor.performed += Unlockcursor;
            Input.playerActions.UnlockCursor.canceled += Lockcursor;
            playerStateMachine.ChangeState(playerStateMachine.idlingState);
        }
        private void Update()
        {
            playerStateMachine.HandleInput();
            playerStateMachine.Update();
        }
        private void FixedUpdate()
        {
            playerStateMachine.PhysicUpdate();
        }
        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            playerStateMachine.OnAnimationEnterEvent();
        }
        public void OnMovementStateAnimationExitEvent()
        {
            playerStateMachine.OnAnimationExitEvent();
        }
        public void OnMovementStateAnimationTransitionEvent()
        {
            playerStateMachine.OnAnimationTransitionEvent();
        }

        private void Unlockcursor(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Input.playerActions.Look.Disable();
        }
        private void Lockcursor(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Input.playerActions.Look.Enable();
        }
    }
}
