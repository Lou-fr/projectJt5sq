using Unity.Cinemachine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
#if BleizOnline
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CameraZoom))]
    [RequireComponent(typeof(CharacterSOController))]
    public class PlayerHandler : NetworkBehaviour
    {
        [field: Header("references")]
        [field: SerializeField] public Transform instantiatePoint { get; private set; }
        [field: Header("Collision")]
        [field: SerializeField] public PlayerCapsuleCollidersUtilities collidersUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData layerData { get; private set; }
        [field: Header("Animation")]
        [field: SerializeField] public PlayerAnimationData animationData { get; private set; }
        [field: Header("Users Settings")]
        [field: SerializeField] public GeneralSettingsData userSettingsData { get; private set; }
        [Header("camera look at point")]
        [SerializeField] private GameObject cinemachineLookAtPoint;
        public Rigidbody rigidBody { get; private set; }
        public CinemachineInputAxisController cinemachineController { get; private set; }
        public NetworkAnimator networkAnimator { get; private set; }
        public Transform mainCameraTransform { get; private set; }
        public PlayerInput Input { get; private set; }
        [SerializeField] private CharacterSOController characterController;
        public PlayerStateMovementReusableData reusabledata { get; private set; }
        public PlayerStateMachine StateMachine { get; protected set; }
        private GameObject[] currentCharacter = new GameObject[4];
        protected int CurrentCaraHealth;
        protected int CurrentCara = 0;
        //Action call
        private void Awake()
        {

        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner) return;
            Input = GetComponent<PlayerInput>();
            Input.inputActions.Disable();
            reusabledata = new PlayerStateMovementReusableData();
            characterController = GetComponent<CharacterSOController>();
            rigidBody = GetComponent<Rigidbody>();
            networkAnimator = GetComponentInChildren<NetworkAnimator>();
            mainCameraTransform = Camera.main.transform;
            animationData.Initialize();
            Cursor.lockState = CursorLockMode.Locked;
            cinemachineController = GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<CinemachineInputAxisController>();
            GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<CinemachineCamera>().Target.TrackingTarget = cinemachineLookAtPoint.transform;
            GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<CinemachineCamera>().Target.LookAtTarget = cinemachineLookAtPoint.transform;
            characterController.SpawnServerRpc(this, characterController.GetListCurrentCharacter(), this.OwnerClientId);
        }
        #region Initiation
        /*public void Initliaze(string CharNumber)
        {
            if (!IsOwner) return;
            PlayerSO[] charcterList = characterController.ListCurrentChara;

#if DEBUG || UNITY_EDITOR
            Debug.Log($"PLAYER SYS : Initating character sys n°1 as {charcterList[0].CharacterInfoData.ChatacterName}");
#endif
            currentCharacter[0] = Instantiate(charcterList[0].CharacterInfoData.AssociatedSkin, this.transform);
            currentCharacter[0].AddComponent<CharacterHandler>();
            InitliazeCollider(currentCharacter[0]);
            if (CharNumber >= 2)
            {
#if DEBUG || UNITY_EDITOR
                Debug.Log($"PLAYER SYS : Initating character sys n°2 as {charcterList[1].CharacterInfoData.ChatacterName}");
#endif
                currentCharacter[1] = Instantiate(charcterList[1].CharacterInfoData.AssociatedSkin, instantiatePoint);
                currentCharacter[1].AddComponent<CharacterHandler>();
            }
            if (CharNumber >= 3)
            {
#if DEBUG || UNITY_EDITOR
                Debug.Log($"PLAYER SYS : Initating character sys n°3 as {charcterList[2].CharacterInfoData.ChatacterName}");
#endif
                currentCharacter[2] = Instantiate(charcterList[2].CharacterInfoData.AssociatedSkin, instantiatePoint);
                currentCharacter[2].AddComponent<CharacterHandler>();
            }
            if (CharNumber >= 4)
            {
#if DEBUG || UNITY_EDITOR
                Debug.Log($"PLAYER SYS : Initating character sys n°2 as {charcterList[3].CharacterInfoData.ChatacterName}");
#endif
                currentCharacter[3] = Instantiate(charcterList[3].CharacterInfoData.AssociatedSkin, instantiatePoint);
                currentCharacter[3].AddComponent<CharacterHandler>();
            }
            characterController.SpawnCharacter(currentCharacter,this.gameObject, this.OwnerClientId);
        }*/
        public void init(GameObject @object,int index)
        {
            currentCharacter[index] = @object;
            if (index == 3) 
            { 
                Ready(index);
            }
        }
        public void Ready(int index)
        {
            if (!IsOwner) return;
            if (index != currentCharacter.Length) return;
            PlayerSO[] charcterList = characterController.ListCurrentChara;
            StateMachine = new PlayerStateMachine(this, currentCharacter[0].GetComponent<CharacterHandler>(), reusabledata);
            StateMachine.ChangeState(StateMachine.idlingState);
            Input.inputActions.Enable();

        }
        public void InitliazeCollider(GameObject chara)
        {
            //PlayerStateMachine.State += handleChangingState;
            collidersUtility.CapsuleCollidersUtility.DefaultColliderData.Height = characterController.ListCurrentChara[CurrentCara].CharacterInfoData.characterHeight;
            collidersUtility.CapsuleCollidersUtility.Initialize(chara);
            collidersUtility.CapsuleCollidersUtility.CalculateCapsuleColliderDimension();
        }

#if UNITY_EDITOR
        void Confirmation()
        {
            //used for debuging
        }
#endif
    #endregion

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (!IsOwner) return;
            Input.playerActions.UnlockCursor.performed -= Unlockcursor;
                Input.playerActions.SwitchCharater1.performed -= SwitchCharater1;
            Input.playerActions.SwitchCharater2.performed -= SwitchCharater2;
            Input.playerActions.SwitchCharater3.performed -= SwitchCharater3;
            Input.playerActions.SwitchCharater4.performed -= SwitchCharater4;
            Input.playerActions.UnlockCursor.canceled -= Lockcursor;
        }
        private void OnValidate()
        {

        }

        private void Start()
        {
            if (!IsOwner) return;
            Input.playerActions.UnlockCursor.performed += Unlockcursor;
            Input.playerActions.SwitchCharater1.performed += SwitchCharater1;
            Input.playerActions.SwitchCharater2.performed += SwitchCharater2;
            Input.playerActions.SwitchCharater3.performed += SwitchCharater3;
            Input.playerActions.SwitchCharater4.performed += SwitchCharater4;
            Input.playerActions.UnlockCursor.canceled += Lockcursor;
        }

        private void Unlockcursor(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cinemachineController.enabled = false;
            Input.playerActions.PlayerZoom.Disable();
        }
        private void Lockcursor(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cinemachineController.enabled = true;
            Input.playerActions.PlayerZoom.Enable();
        }

        #region Character
        private void SwitchCharater1(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            swapCharTo(0);
        }
        private void SwitchCharater2(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            swapCharTo(1);
        }
        private void SwitchCharater3(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            swapCharTo(2);
        }
        private void SwitchCharater4(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            swapCharTo(3);
        }
        private void swapCharTo(int newChar)
        {
            if (characterController.ListCurrentChara[newChar] == null)
            {
                Debug.LogError($"PLAYER SYS : Player tried change to {newChar} but not any character is found on this possition and player is still {characterController.ListCurrentChara[CurrentCara].CharacterInfoData.ChatacterName} ");
                return;
            }
            if (characterController.ListCurrentChara[newChar] == characterController.ListCurrentChara[CurrentCara])
            {
                Debug.LogError($"PLAYER SYS : Player tried change to {newChar} but the player is already {characterController.ListCurrentChara[CurrentCara].CharacterInfoData.ChatacterName} ");
                return;
            }
            Input.DisableActionFor(Input.playerActions.SwitchCharater1, 1);
            Input.DisableActionFor(Input.playerActions.SwitchCharater2, 1);
            Input.DisableActionFor(Input.playerActions.SwitchCharater3, 1);
            Input.DisableActionFor(Input.playerActions.SwitchCharater4, 1);
            int lastSTM = CurrentCara;
            Debug.Log($"PLAYER SYS : Player changing to {characterController.ListCurrentChara[newChar].CharacterInfoData.ChatacterName} and was {characterController.ListCurrentChara[CurrentCara].CharacterInfoData.ChatacterName} ");
            CurrentCara = newChar;

            Debug.Log(CurrentCara + " " + lastSTM);
            ActivateStateMachine(newChar);
        }
        #endregion
        #region stateMachine
        private void ActivateStateMachine(int newSTM)
        {
            if (newSTM == 0) { currentCharacter[0].SetActive(true); Deactivate(newSTM); return; }
            if (newSTM == 1) { currentCharacter[1].SetActive(true); Deactivate(newSTM); return; }
            if (newSTM == 2) { currentCharacter[2].SetActive(true); Deactivate(newSTM); return; }
            if (newSTM == 3) { currentCharacter[3].SetActive(true); Deactivate(newSTM); return; }
        }
        private void Deactivate(int newstm)
        {
            if (currentCharacter[0].activeSelf && 0 != newstm) { currentCharacter[0].SetActive(false); }
            if (currentCharacter[1] != null) if (currentCharacter[1].activeSelf && 1 != newstm) currentCharacter[1].SetActive(false);
            if (currentCharacter[2] != null) if (currentCharacter[2].activeSelf && 2 != newstm) currentCharacter[2].SetActive(false);
            if (currentCharacter[3] != null) if (currentCharacter[3].activeSelf && 3 != newstm) currentCharacter[3].SetActive(false);
        }
        private void Update()
        {
            StateMachine?.HandleInput();
            StateMachine?.Update();
        }
        private void FixedUpdate()
        {
            StateMachine?.PhysicUpdate();
        }
        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            StateMachine?.OnAnimationEnterEvent();
        }
        public void OnMovementStateAnimationExitEvent()
        {
            StateMachine?.OnAnimationExitEvent();
        }
        public void OnMovementStateAnimationTransitionEvent()
        {
            StateMachine?.OnAnimationTransitionEvent();
        }
        private void OnTriggerEnter(Collider collider)
        {
            StateMachine?.OnTriggerEnter(collider);
        }
        private void OnTriggerExit(Collider collider)
        {
            StateMachine?.OnTriggerExit(collider);
        }
        #endregion

    }
#endif
}
