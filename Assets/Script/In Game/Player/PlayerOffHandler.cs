using BleizEntertainment.Maps.death;
using System;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CameraZoom))]
    [RequireComponent(typeof(WrapMain))]
    public class PlayerOffHandler : MonoBehaviour
    {
        [field: Header("references")]
        [field: Header("Collision")]
        [field: SerializeField] public PlayerCapsuleCollidersUtilities collidersUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData layerData { get; private set; }
        [field: Header("Animation")]
        [field: SerializeField] public PlayerAnimationData animationData { get; private set; }
        [field: Header("Users Settings")]
        [field: SerializeField] public GeneralSettingsData userSettingsData { get; private set; }
        public Rigidbody rigidBody { get; private set; }
        public CinemachineInputAxisController cinemachineController { get; private set; }
        public Animator animator { get; private set; }
        public Transform mainCameraTransform { get; private set; }
        public PlayerInput Input { get; private set; }
        [SerializeField] private CharacterSOController characterController;
        public PlayerStateMachine playerStateMachine { get; protected set; }
        public PlayerStateMovementReusableData reusableData { get; private set; }
        public GameObject[] currentCharacters = new GameObject[4];
        //please for any public content do not send information about the reason of the death
        public static Action<int,string> damageReceive = delegate { };
        public static Action<int,bool,bool> HealReceive = delegate { };
        public static Action<int,Vector3> allDead = delegate { };
        protected int currentCara,regionId = 0; 
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            reusableData = new PlayerStateMovementReusableData();
            characterController = GetComponent<CharacterSOController>();
            mainCameraTransform = Camera.main.transform;
            animationData.Initialize();
            collidersUtility.CapsuleCollidersUtility.Initialize(gameObject);
            Cursor.lockState = CursorLockMode.Locked;
            CharacterOffHandler.CharacterDead += DeathSwapChar;
            DeathSystem.wrapAndRespawn += wrap;
            cinemachineController = GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<CinemachineInputAxisController>();
        }
        private void OnDestroy()
        {
            Input.playerActions.UnlockCursor.performed -= Unlockcursor;
            Input.playerActions.UnlockCursor.canceled -= Lockcursor;
            Input.playerActions.SwitchCharater1.performed -= SwitchCharater1;
            Input.playerActions.SwitchCharater2.performed -= SwitchCharater2;
            Input.playerActions.SwitchCharater3.performed -= SwitchCharater3;
            Input.playerActions.SwitchCharater4.performed -= SwitchCharater4;
            DeathSystem.wrapAndRespawn -= wrap;
            CharacterOffHandler.CharacterDead -= DeathSwapChar;
        }
        public void Spawn()
        {
            InitliazeCollider(currentCharacters[0]);
            playerStateMachine = new(this, currentCharacters[0].GetComponent<CharacterOffHandler>(), reusableData);
            playerStateMachine.ChangeState(playerStateMachine.idlingState);
        }
        private void Start()
        {
            Input = GetComponent<PlayerInput>();
            Input.playerActions.UnlockCursor.performed += Unlockcursor;
            Input.playerActions.UnlockCursor.canceled += Lockcursor;
            Input.playerActions.SwitchCharater1.performed += SwitchCharater1;
            Input.playerActions.SwitchCharater2.performed += SwitchCharater2;
            Input.playerActions.SwitchCharater3.performed += SwitchCharater3;
            Input.playerActions.SwitchCharater4.performed += SwitchCharater4;
            characterController.Spawn();
        }
        private void Unlockcursor(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cinemachineController.enabled = false;
            Input.playerActions.PlayerZoom.Disable();
        }
        private void Lockcursor(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cinemachineController.enabled = true;
            Input.playerActions.PlayerZoom.Enable();
        }
        #region Character
        private void DeathSwapChar()
        {

            foreach (GameObject character in currentCharacters)
            {
                if (character != null)
                {
                    CharacterOffHandler offHandler = character.GetComponent<CharacterOffHandler>();
                    if (offHandler.IsAlive)
                    {
                        swapCharTo(offHandler.AssignedNumber);
                        return;
                    }
                }
            }
            allDead?.Invoke(regionId,this.transform.position);
        }
        private void SwitchCharater1(InputAction.CallbackContext context)
        {
            swapCharTo(0);
        }
        private void SwitchCharater2(InputAction.CallbackContext context)
        {
            swapCharTo(1);
        }
        private void SwitchCharater3(InputAction.CallbackContext context)
        {
            swapCharTo(2);
        }
        private void SwitchCharater4(InputAction.CallbackContext context)
        {
            swapCharTo(3);
        }
        private bool swapCharTo(int newChar)
        {
            if (characterController.ListCurrentChara[newChar] == null)
            {
                Debug.LogError($"PLAYER SYS : Player tried change to {newChar} but not any character is found on this possition and player is still {characterController.ListCurrentChara[currentCara].CharacterInfoData.ChatacterName} ");
                return false;
            }
            if (characterController.ListCurrentChara[newChar] == characterController.ListCurrentChara[currentCara])
            {
                Debug.LogError($"PLAYER SYS : Player tried change to {newChar} but the player is already {characterController.ListCurrentChara[currentCara].CharacterInfoData.ChatacterName} ");
                return false;
            }
            if (!currentCharacters[newChar].GetComponent<CharacterOffHandler>().IsAlive)
            {
                Debug.LogError($"PLAYER SYS : Player tried change to {newChar} but the character {characterController.ListCurrentChara[newChar].CharacterInfoData.ChatacterName} is dead ");
                return false;
            }
            Input.DisableActionFor(Input.playerActions.SwitchCharater1, 1);
            Input.DisableActionFor(Input.playerActions.SwitchCharater2, 1);
            Input.DisableActionFor(Input.playerActions.SwitchCharater3, 1);
            Input.DisableActionFor(Input.playerActions.SwitchCharater4, 1);
            int lastSTM = currentCara;
            Debug.Log($"PLAYER SYS : Player changing to {characterController.ListCurrentChara[newChar].CharacterInfoData.ChatacterName} and was {characterController.ListCurrentChara[currentCara].CharacterInfoData.ChatacterName} ");
            currentCara = newChar;
            characterController.characterSelecterUI.ChangeLightUI(lastSTM, currentCara);
            Debug.Log(currentCara + " " + lastSTM);
            ActivateStateMachine(newChar);
            return true;
        }
        #endregion
        #region ColliderUtilites
        public void InitliazeCollider(GameObject character)
        {
            collidersUtility.CapsuleCollidersUtility.DefaultColliderData.Height = characterController.ListCurrentChara[currentCara].CharacterInfoData.characterHeight;
            collidersUtility.CapsuleCollidersUtility.Initialize(character);
            collidersUtility.CapsuleCollidersUtility.CalculateCapsuleColliderDimension();
        }
        #endregion
        #region stateMachine
        private void ActivateStateMachine(int newSTM)
        {
            if (newSTM == 0) { currentCharacters[0].SetActive(true); Deactivate(newSTM); return; }
            if (newSTM == 1) { currentCharacters[1].SetActive(true); Deactivate(newSTM); return; }
            if (newSTM == 2) { currentCharacters[2].SetActive(true); Deactivate(newSTM); return; }
            if (newSTM == 3) { currentCharacters[3].SetActive(true); Deactivate(newSTM); return; }
        }
        private void Deactivate(int newstm)
        {
            if (currentCharacters[0].activeSelf && 0 != newstm) { currentCharacters[0].SetActive(false); }
            if (currentCharacters[1] != null) if (currentCharacters[1].activeSelf && 1 != newstm) currentCharacters[1].SetActive(false);
            if (currentCharacters[2] != null) if (currentCharacters[2].activeSelf && 2 != newstm) currentCharacters[2].SetActive(false);
            if (currentCharacters[3] != null) if (currentCharacters[3].activeSelf && 3 != newstm) currentCharacters[3].SetActive(false);
        }
        private void Update()
        {
            playerStateMachine?.HandleInput();
            playerStateMachine?.Update();
        }
        private void FixedUpdate()
        {
            playerStateMachine?.PhysicUpdate();
        }
        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            playerStateMachine?.OnAnimationEnterEvent();
        }
        public void OnMovementStateAnimationExitEvent()
        {
            playerStateMachine?.OnAnimationExitEvent();
        }
        public void OnMovementStateAnimationTransitionEvent()
        {
            playerStateMachine?.OnAnimationTransitionEvent();
        }
        private void OnTriggerEnter(Collider collider)
        {
            playerStateMachine.OnTriggerEnter(collider);
        }
        private void OnTriggerExit(Collider collider)
        {
            playerStateMachine.OnTriggerExit(collider);
        }
        #endregion

        #region Vitals Component
        public static void hitTaken(int dmg,string entityORreason)
        {
            Debug.Log(dmg);
            damageReceive.Invoke(dmg,entityORreason);
        }
        public static void healReceive(int IncomingHealing, bool HealingOverwite)
        {
            HealReceive.Invoke(IncomingHealing, HealingOverwite,false);
        }
        void wrap(Vector3 wrapPos)
        {
            this.transform.position = wrapPos;
        }
        #endregion
    }
}
