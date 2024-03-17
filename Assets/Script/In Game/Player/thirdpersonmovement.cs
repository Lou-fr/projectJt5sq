using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using FishNet.Object;
using FishNet.Connection;

public class thirdpersonmovement : NetworkBehaviour
{
    [SerializeField] BleizInputManager _input;
    [SerializeField] private CharacterController controller;
    private float speed = 6f;
    private float gravity = -9.81f;
    private Vector3 velocity;
    private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform groundcheck;
    private float groundDistance = 0.4f;
    private float jumpHeight = 3f;
    [SerializeField] private GameObject CinemachineCameraTarget;
    [SerializeField] private LayerMask groundmask;
    [SerializeField] private InputActionReference mouvement;
    [SerializeField] private InputActionReference jump;
    bool isGrounded;
    private PlayerInput _playerInput;
    private bool IsCurrentDeviceMouse
#if UNITY_STANDALONE
        = false;
#else
        =true;
#endif

    //cinemachine
    [SerializeField] private InputActionReference _press;
    public bool LockCameraPosition = false;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private const float _threshold = 0.01f;
    public float CameraAngleOverride = 0.0f;

    public override void OnStartNetwork()
    {
        if(!base.Owner.IsLocalClient) return;
        cam = GameObject.FindAnyObjectByType<Camera>().GetComponent<Transform>();
        GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = transform.GetChild(0).transform;
    }
    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);

        _playerInput = GetComponent<PlayerInput>();
        _playerInput.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        Grounded();
        Move();
    }
    private void LateUpdate()
    {
        CameraRotation();
        FallUnderGround();
    }
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.LookInput.sqrMagnitude >= _threshold && !LockCameraPosition
#if UNITY_STANDALONE 
             && _press.action.IsPressed() 
#endif
            )
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.LookInput.x * deltaTimeMultiplier *-1;
            _cinemachineTargetPitch += _input.LookInput.y * deltaTimeMultiplier *-1;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private void Move()
    {
        Vector2 _movedir = _input.MoveInput;
        Vector3 direction = new Vector3(_movedir.x, 0f, _movedir.y).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targatAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targatAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targatAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        if (_input.SpaceIsPressed.Equals(1) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void Grounded()
    {
        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundmask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
     

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void FallUnderGround()
    {
        if (controller.transform.position.y < -20)
        {
            controller.transform.position = new Vector3(0, 10, 0);
        }
    }
}