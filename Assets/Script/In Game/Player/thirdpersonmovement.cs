using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using FishNet.Object;
using FishNet.Connection;
using System;

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
    private float jumpHeight = 2f;
    [SerializeField] private GameObject CinemachineCameraTarget;
    [SerializeField] private LayerMask groundmask;
    [SerializeField] private InputActionReference mouvement;
    [SerializeField] private InputActionReference jump;
    bool isGrounded;
    private PlayerInput _playerInput;
    BleizInputManager bleizInputManager;
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
    float _cinemachineMinZoom = 6.0f;
    float _cinemachineMaxZoom = 10.0f;
    CinemachineVirtualCamera CinemachineVirtualCamera;
    Cinemachine3rdPersonFollow cinemachine3Rd;
    [SerializeField]float _cameraZoomModifier =32.0f;
    //Multiplayer Info (to rework)
    bool startedCondition = false;
    bool IsSaveHaveBeenLoaded = false;
    Vector3 startedPosistion = new Vector3(0,-666,0);
    bool isLobbyOwner = false;
    public static Action ReadyForDeployment = delegate{};

    public override  void OnStartNetwork()
    {
        if(!base.Owner.IsLocalClient) return;
        cam = GameObject.FindAnyObjectByType<Camera>().GetComponent<Transform>();
        CinemachineVirtualCamera = GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        cinemachine3Rd = CinemachineVirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        CinemachineVirtualCamera.Follow = transform.GetChild(0).transform;
        CinemachineVirtualCamera.LookAt = transform.GetChild(0).transform;
    }
    public override async void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        Debug.Log(IsOwner);
        //When multiplayer across player will be on
        /*if(await LobbyManager.IsCurrentLobbyIsPlayerOwned()){
            isLobbyOwner = true;
            //LobbyManager.SendHostPosition += SaveHostPosition;
            var position = await SaveManager.LoadPlayerPosition();
            Debug.Log(controller.transform.position.ToString());
            Debug.Log(position.ToString());
            if(!IsOwner)return;
            controller.transform.position = position;
            Debug.Log(controller.transform.position.ToString());
            if(this.gameObject.transform.position != position || controller.gameObject.transform.position != position || controller.transform.position != position){Debug.LogError("The possition hasn't setup");controller.transform.position = position;}
            startedPosistion = position;
            IsSaveHaveBeenLoaded=true;
            SavePlayer();
            SaveManager.SavePosition +=SavePlayer;
        }else
        {
            isLobbyOwner = false;
            Vector3 temp = new Vector3 (10f, 0f, 10f);
            //temp = LobbyManager.ReadHostPosition();
            //here will be the script to teleperot the joining player 
            startedPosistion = temp;
        }*/
        //FOR DISABLE MULTIPLAYER WITH OTHER PLAYER
        var position = await SaveManager.LoadPlayerPosition();
        controller.transform.position = position;
        if(this.gameObject.transform.position != position || controller.gameObject.transform.position != position || controller.transform.position != position){Debug.LogError("The possition hasn't setup");controller.transform.position = position;}
        startedPosistion = position;
        IsSaveHaveBeenLoaded=true;
        isLobbyOwner = true;
        SaveManager.SavePosition +=SavePlayer;
        SavePlayer();
        _playerInput = GetComponent<PlayerInput>();
        bleizInputManager = GetComponent<BleizInputManager>();
    }
    private void CheckStarterCondition(Vector3 position)
    {
        if(!IsOwner)return;
        if(startedPosistion.y == -666)return;
        Debug.Log(isLobbyOwner +" "+IsSaveHaveBeenLoaded);
        if(!IsSaveHaveBeenLoaded){if(!isLobbyOwner){/*{if(this.gameObject.transform.position != position || controller.gameObject.transform.position != position || controller.transform.position != position){Debug.LogError("The possition hasn't setup");controller.transform.position = position;return;}else{startedCondition=true;return;}*/ReadyForDeployment?.Invoke();startedCondition=true;return; }}
        if(!IsSaveHaveBeenLoaded)return;
        if(this.gameObject.transform.position != position || controller.gameObject.transform.position != position || controller.transform.position != position){Debug.LogError("The possition hasn't setup");controller.transform.position = position;return;}
        startedCondition=true;
        _playerInput.enabled = true;
        bleizInputManager.enabled = true;
        ReadyForDeployment?.Invoke();
    }

    void Update()
    {
        if(!IsOwner) return;
        if(startedCondition is false){Debug.Log(controller.transform.position.ToString());;CheckStarterCondition(startedPosistion);return;}
        Grounded();
        Move();
    }
    private void LateUpdate()
    {
        CameraRotation();
        FallUnderGround();
        if(!(_input.ZoomCameraInput ==0.00f)){CameraZoom();}
    }
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.LookInput.sqrMagnitude >= _threshold && !LockCameraPosition
#if UNITY_STANDALONE 
             && _input.MouseIsClicked
#endif
            )
        {
            //Don't multiply mouse input by Time.deltaTime;
            float Multiplier = IsCurrentDeviceMouse ?  1.0f:Sensivity_Controller.Sensivity*Time.smoothDeltaTime;

            _cinemachineTargetYaw += _input.LookInput.x * Multiplier *-1;
            _cinemachineTargetPitch += _input.LookInput.y * Multiplier *-1;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }
    private void CameraZoom()
    {
        cinemachine3Rd.CameraDistance = Math.Clamp(cinemachine3Rd.CameraDistance+ (_input.InvertScroll ?_input.ZoomCameraInput : -_input.ZoomCameraInput) /_cameraZoomModifier,_cinemachineMinZoom,_cinemachineMaxZoom);
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
            controller.Move(moveDir.normalized * speed * Time.deltaTime* (_input.SprintIsPressed ? 1.5f :1.0f));
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
    public override void OnStopClient()
    {
        //LobbyManager.SendHostPosition -= SaveHostPosition;
        var currentPos = controller.transform.position;
        Debug.Log("Position saving, is Lobby owner ?");
        if(!isLobbyOwner)return;
        Debug.Log("Position saving");
        SaveManager.SavePlayerPosition(currentPos);
        SaveManager.SavePosition -=SavePlayer;
        Debug.Log("Position saved");
    }
    void SavePlayer()
    {
        if(!IsOwner)return;
        Debug.Log("Initiating...");
        if(startedCondition is false){StartCoroutine(SaveManager.Coutdown(10));;return;}
        StartCoroutine(SaveManager.Coutdown(60));
        SaveManager.SavePlayerPosition(controller.transform.position);
    }
    //Don't work due to API rate limitation
    /*void SaveHostPosition()
    {
        #pragma warning disable
        LobbyManager.SaveHostPositionAsLobbyData(controller.transform.position);
        #pragma warning restore
    }*/
}