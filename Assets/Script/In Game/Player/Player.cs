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
        public Transform MainCameraTransform { get; private set; }
        public PlayerInput Input { get; private set; }
        private PlayerMovementStateMachine movementStateMachine;
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            movementStateMachine = new PlayerMovementStateMachine(this);

            MainCameraTransform = Camera.main.transform;
            animationData.Initialize();
            collidersUtility.CapsuleCollidersUtility.Initialize(gameObject);
        }
        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnter(collider);
        }
        private void OnTriggerExit(Collider collider)
        {
            movementStateMachine.OnTriggerExit(collider);
        }
        private void OnValidate()
        {
            collidersUtility.CapsuleCollidersUtility.Initialize(gameObject);
            collidersUtility.CapsuleCollidersUtility.CalculateCapsuleColliderDimension();
        }
        private void Start()
        {
            Input = GetComponent<PlayerInput>();
            movementStateMachine.ChangeState(movementStateMachine.idlingState);
        }
        private void Update()
        {
            movementStateMachine.HandleInput();
            movementStateMachine.Update();
        }
        private void FixedUpdate()
        {
            movementStateMachine.PhysicUpdate();
        }
        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            movementStateMachine.OnAnimationEnterEvent();
        }
        public void OnMovementStateAnimationExitEvent()
        {
            movementStateMachine.OnAnimationExitEvent();
        }
        public void OnMovementStateAnimationTransitionEvent()
        {
            movementStateMachine.OnAnimationTransitionEvent();
        }
    }
}
