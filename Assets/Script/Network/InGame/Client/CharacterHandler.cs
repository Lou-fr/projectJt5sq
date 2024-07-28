using UnityEngine;

namespace BleizEntertainment
{
    #if BleizOnline
    public class CharacterHandler : MonoBehaviour
    {
        private int AssignedNumber;
        bool init;
        private PlayerHandler PlayerHandler;
        public Animator AssignedAnimator { get; private set; }
        public ClientNetworkAnimator assignedNetworkAnimator { get; private set; }
        private PlayerAnimationEventTrigger AssignedTrigger;
        public PlayerSO AssignedCharacter { get; private set; }
        public PlayerStateReusableCombatData CombatData { get; private set; } = new PlayerStateReusableCombatData();
        bool Activate = false;
        public void Init(PlayerHandler player, int assignedNumber, PlayerSO assignedCharacter, PlayerInput input, PlayerLayerData layerdata, PlayerAnimationData animationData, ulong playerId, bool activate = false)
        {
            AssignedNumber = assignedNumber;
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : Initating as character n°{AssignedNumber}");
#endif
            PlayerHandler = player;
            Activate = activate;
            AssignedCharacter = assignedCharacter;
            AssignedAnimator = GetComponent<Animator>();
            assignedNetworkAnimator = GetComponent<ClientNetworkAnimator>();
            AssignedTrigger = GetComponent<PlayerAnimationEventTrigger>();
            AssignedTrigger.Init(AssignedAnimator, PlayerHandler);
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : First stage complete|n°{AssignedNumber}");
#endif
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : all stage complete, ready to go|n°{AssignedNumber}");
#endif
            this.gameObject.name = AssignedCharacter.CharacterInfoData.ChatacterName + assignedNumber + playerId;
        }
        private void InitializeWeaponClass(PlayerStateMachine stateMachine)
        {
            WeaponsDataClass weaponsDataClass = AssignedCharacter.combatData.WeaponsDataClass;
            if (weaponsDataClass.IsMelee)
            {
                if (weaponsDataClass.IsHeavy) { stateMachine.PlayerCombatHeavyMeleeInstantiate(); return; }
                stateMachine.PlayerCombatMeleeInstantiate(); return;
            }
        }

        public void OnDestroy()
        {
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : Destroy request inboud clearing all component|n°{AssignedNumber}");
#endif
            AssignedAnimator = null;
            AssignedCharacter = null;
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : Destroying ... |n°{AssignedNumber}");
#endif
        }
        private void OnDisable()
        {
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : Deactivating |n°{AssignedNumber}");
#endif
        }
        void OnEnable()
        {
            if (!init) return;
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : Activating |n°{AssignedNumber}");
#endif
#if UNITY_EDITOR

#endif
            PlayerStateMachine stm = PlayerHandler.StateMachine;
            PlayerHandler.InitliazeCollider(this.gameObject);
            stm.ChangeCharacter(this);
            InitializeWeaponClass(stm);
        }
    }
    #endif
}
