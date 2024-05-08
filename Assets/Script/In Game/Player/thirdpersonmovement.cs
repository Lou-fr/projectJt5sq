using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using Fusion;

public class thirdpersonmovement : NetworkBehaviour
{
    private Vector3 velocity;
    private float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform groundcheck;
    [SerializeField] private GameObject CinemachineCameraTarget;
    //[SerializeField] private LayerMask groundmask;
    [SerializeField] private InputActionReference mouvement;
    [SerializeField] private InputActionReference jump;
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
    [Networked] private TickTimer life {get;set;}

    public void Init()
    {
        if(HasStateAuthority) return;
        cam = GameObject.FindAnyObjectByType<Camera>().GetComponent<Transform>();
        CinemachineVirtualCamera = GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        cinemachine3Rd = CinemachineVirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        CinemachineVirtualCamera.Follow = transform.GetChild(0).transform;
        CinemachineVirtualCamera.LookAt = transform.GetChild(0).transform;
    }
    public override async void Spawned()
    {
        base.Spawned();
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
        playerTx = transform;
        SaveManager.SavePosition +=SavePlayer;
        SavePlayer();
        _playerInput = GetComponent<PlayerInput>();
        bleizInputManager = GetComponent<BleizInputManager>();
    }
    private void CheckStarterCondition(Vector3 position)
    {
        if(!HasStateAuthority)return;
        if(startedPosistion.y == -666)return;
        Debug.Log(isLobbyOwner +" "+IsSaveHaveBeenLoaded);
        if(!IsSaveHaveBeenLoaded){if(!isLobbyOwner){/*{if(this.gameObject.transform.position != position || controller.gameObject.transform.position != position || controller.transform.position != position){Debug.LogError("The possition hasn't setup");controller.transform.position = position;return;}else{startedCondition=true;return;}*/ReadyForDeployment?.Invoke();startedCondition=true;return; }}
        if(!IsSaveHaveBeenLoaded)return;
        if(this.gameObject.transform.position != position || controller.gameObject.transform.position != position || controller.transform.position != position){Debug.LogError("The possition hasn't setup");controller.transform.position = position;return;}
        lastPos= playerTx.position;
        groundOffsetY = groundCheckY;
        ceilingOffsetY = ceilingCheckY;
        startedCondition=true;
        _playerInput.enabled = true;
        bleizInputManager.enabled = true;
        ReadyForDeployment?.Invoke();
    }
    //From Alucard-Jay Github, remastered to fit our objective
    [Header("External Component")]
    [SerializeField] BleizInputManager _input;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform playerTx;
    [Header("Move Settings")]
    [SerializeField]private float walkSpeed = 6f;
    [SerializeField]private float sprintSpeed = 10f;
    [SerializeField]private float airSpeed= 3f;
    [SerializeField]private float gravity =9.81f;
    [SerializeField]private float jumpHeight = 2.5f;
    [Header("Grounded and celing Settings")]
    [SerializeField]private LayerMask castingMask;
    [SerializeField]private float groundCheckY=0.33f;
    [SerializeField]private float ceilingCheckY=1.83f;
    [SerializeField]private float sphereCastRadius = 0.25f;
    [SerializeField]private float sphereCastDistance =0.5f;
    [SerializeField]private float raycastLength = 0.5f;// secondary raycasts (match to sphereCastDistance)
    [SerializeField]private Vector3 rayOriginOffset1 = new Vector3(-0.2f, 0f, 0.16f);
    [SerializeField]private Vector3 rayOriginOffset2 = new Vector3(0.2f, 0f, -0.16f);
    [Header("Reference Variable")]
    public float xRotation = 0f;
    [Space(1)]
    [SerializeField]private float lastSpeed = 0; 
    [SerializeField]private Vector3 fauxGravity = Vector3.zero;     
    [SerializeField]private Vector3 lastPos = Vector3.zero;
    [SerializeField]private bool isGrounded = false;
    [SerializeField]private bool isWallInFront = false;
    [Space(1)]
    public float gorundSlopeAngle = 0f;
    public Vector3 groundSlopeDir = Vector3.zero;
    private float groundOffsetY = 0;
    public bool isSlipping = false;
    [Space(1)]
    public bool isCeiling = false;
    private float ceilingOffsetY = 0;
    private float defaultHeight = 0;
    [SerializeField]private Vector3 _dir;
    [Space(1)]
    [Header("Debuging Option")]
    public bool showGizmos= true;
    void Update()
    {
        if(!HasStateAuthority) return;
        if(startedCondition is false){Debug.Log(controller.transform.position.ToString());;CheckStarterCondition(startedPosistion);return;}
        //Grounded();
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
        float h = defaultHeight;
        float nextSpeed = 0f;
        Vector3 calc;
        Vector3 lastDirection;
        Vector3 move;

        float currSpeed = (playerTx.position - lastPos).magnitude/Time.deltaTime;
        currSpeed = ( currSpeed < 0 ? 0 - currSpeed : currSpeed );
        GroundCheck();

        isSlipping = (gorundSlopeAngle > controller.slopeLimit ? true : false);
        ceilingCheck();
        if(controller.isGrounded && !isCeiling && _input.SprintIsPressed)
        {
            nextSpeed= sprintSpeed;
        }
        lastDirection = playerTx.position - lastPos;
        lastPos= playerTx.position;
        lastDirection =lastDirection.normalized;
        lastDirection.y =0f;
        wallCheck(lastDirection);
        if(direction.magnitude >= 0.1f && controller.isGrounded)
        {
            float targatAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targatAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            move = Quaternion.Euler(0f, targatAngle, 0f) * Vector3.forward;
            if(move.magnitude > 1f)
                {
                    move = move.normalized;
                }
            if(!_input.SprintIsPressed)nextSpeed = walkSpeed;
        }else
        {
            if(controller.isGrounded)move=Vector3.zero;
            else 
            {
                if(!isWallInFront)
                {
                    move=lastDirection;
                    nextSpeed=airSpeed;
                }
                else
                {
                    move = Vector3.zero;
                }
                
            }
            
        }
        float speed;
        if(controller.isGrounded)
        {
            if(isSlipping)
            {
                Vector3 sloppeRight = Quaternion.LookRotation(Vector3.right)*groundSlopeDir;
                float dot = Vector3.Dot(sloppeRight,playerTx.right);
                move = sloppeRight * (dot > 0 ? _input.MoveInput.x : -_input.MoveInput.x);
                nextSpeed = Mathf.Lerp( currSpeed, sprintSpeed, 5f * Time.deltaTime );
                float mag = fauxGravity.magnitude;
                calc = Vector3.Slerp( fauxGravity, groundSlopeDir * sprintSpeed, 4f * Time.deltaTime );
                fauxGravity = calc.normalized * mag;
            }else
            {
                // reset angular fauxGravity movement
                fauxGravity.x = 0;
                fauxGravity.z = 0;

                if ( fauxGravity.y < 0 ) // constant grounded gravity
                {
                    //fauxGravity.y = -1f;
                    fauxGravity.y = Mathf.Lerp( fauxGravity.y, -1f, 4f * Time.deltaTime );
                }
            }
            if ( !isCeiling && _input.SpaceIsPressed) // jump
            {
                fauxGravity.y = Mathf.Sqrt( jumpHeight * -2f * gravity );
            }
            float lerpFactor = ( lastSpeed > nextSpeed ? 4f : 2f );
            speed = Mathf.Lerp( lastSpeed, nextSpeed, lerpFactor * Time.deltaTime );
        }
        else // no friction, speed changes slower
        {
            speed = Mathf.Lerp( lastSpeed, nextSpeed, 0.125f * Time.deltaTime );
        }
        if ( isCeiling )
        {
            if ( fauxGravity.y > 0 )
            {
                fauxGravity.y = -1f; // 0;
            }
        }
        lastSpeed = speed;
        fauxGravity.y += gravity * Time.deltaTime;
        calc = move * speed * Time.deltaTime;
        calc += fauxGravity * Time.deltaTime;
        controller.Move(calc);
    }
    private void wallCheck(Vector3 dir)
    {
        /*Vector3 origin = new Vector3(playerTx.position.x, playerTx.position.y + 1f, playerTx.position.z);
        _dir = dir;
        _dir = _dir.normalized;
        RaycastHit hit;
        if(Physics.Raycast(origin,_dir,out hit,Mathf.Infinity,castingMask))
        {
            #if UNITY_EDITOR
                if (showGizmos) {Debug.DrawLine(origin, _dir * hit.distance, Color.red);}
            #endif
            if(hit.distance < 1f)isWallInFront=true;
            else isWallInFront=false;
        }else isWallInFront=false;*/
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding with "+other.gameObject.name);
        if(other.gameObject.tag == "Ground"){return;}
        Debug.Log("Hit a wall");
        isWallInFront=true;
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Ground")return;
        if(isWallInFront)isWallInFront=false;
    }
    private void ceilingCheck()
    {
        Vector3 origin = new Vector3( playerTx.position.x, playerTx.position.y + ceilingOffsetY, playerTx.position.z );
        isCeiling = Physics.CheckSphere( origin, sphereCastRadius, castingMask );
    }

    private void GroundCheck()
    {
        Vector3 origin = new Vector3(playerTx.position.x,playerTx.position.y + groundOffsetY,playerTx.position.z);
        RaycastHit hit;

        if(Physics.SphereCast(origin,sphereCastRadius,Vector3.down,out hit, sphereCastDistance,castingMask))
        {
            gorundSlopeAngle =Vector3.Angle(hit.normal,Vector3.up);
            Vector3 temp = Vector3.Cross(hit.normal,Vector3.down);
            groundSlopeDir = Vector3.Cross(temp,hit.normal);
            isGrounded=true;
        }else
        {
            isGrounded=false;
        }
        RaycastHit slopeHit1;
        RaycastHit slopeHit2;
        if(Physics.Raycast(origin+rayOriginOffset1,Vector3.down,out slopeHit1,raycastLength))
        {
            #if UNITY_EDITOR
            if (showGizmos) {Debug.DrawLine(origin + rayOriginOffset1, slopeHit1.point, Color.red);}
            #endif
            float angleOne = Vector3.Angle(slopeHit1.normal,Vector3.up);
            if(Physics.Raycast(origin+rayOriginOffset2,Vector3.down,out slopeHit2,raycastLength))
                {
                    #if UNITY_EDITOR
                        if (showGizmos) { Debug.DrawLine(origin + rayOriginOffset2, slopeHit2.point, Color.red); }
                    #endif
                    float angleTwo = Vector3.Angle(slopeHit2.normal,Vector3.up);
                    float[] tempArray= new float[] {gorundSlopeAngle, angleOne, angleTwo};
                    System.Array.Sort(tempArray);
                    gorundSlopeAngle = tempArray[1];
                }else
                {
                    float average = (gorundSlopeAngle + angleOne) / 2;
		            gorundSlopeAngle = average;
                }
        }
    }

    /*private void Grounded()
    {
        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundmask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }*/
     

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
    /*public override void OnStopClient()
    {
        //LobbyManager.SendHostPosition -= SaveHostPosition;
        var currentPos = controller.transform.position;
        Debug.Log("Position saving, is Lobby owner ?");
        if(!isLobbyOwner)return;
        Debug.Log("Position saving");
        SaveManager.SavePlayerPosition(currentPos);
        SaveManager.SavePosition -=SavePlayer;
        Debug.Log("Position saved");
    }*/
    void SavePlayer()
    {
        if(!HasStateAuthority)return;
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