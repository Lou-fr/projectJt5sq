using System;
using UnityEngine;

namespace BleizEntertainment
{
    public class CharacterOffHandler : MonoBehaviour
    {
        public int AssignedNumber{get;private set;}
        bool init;
        private PlayerOffHandler PlayerHandler;
        public Animator AssignedAnimator { get; private set; }
        private PlayerAnimationEventTrigger AssignedTrigger;
        public CharacterSO AssignedCharacter { get; private set; }
        public PlayerStateReusableCombatData CombatData { get; private set; } = new PlayerStateReusableCombatData();
        public static Action<float,bool> healthPercetange= delegate {};
        public static Action CharacterDead= delegate {};
        public static Action CharacterRespawn= delegate {};
        protected int health;
        protected int maxHealth;
        public bool IsAlive { get; protected set; } = true;
        bool Activate;
        public void Init(PlayerOffHandler player, int assignedNumber, CharacterSO assignedCharacter, PlayerInput input, PlayerLayerData layerdata, PlayerAnimationData animationData, bool activate = false)
        {
            AssignedNumber = assignedNumber;
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : Initating as character n°{AssignedNumber}");
#endif
            PlayerHandler = player;
            AssignedCharacter = assignedCharacter;
            maxHealth = assignedCharacter.CharacterInfoData.BaseCharacterHp;
            PlayerOffHandler.damageReceive += Hit;
            PlayerOffHandler.HealReceive += heal;
            health =maxHealth;
            AssignedAnimator = GetComponent<Animator>();
            AssignedTrigger = GetComponent<PlayerAnimationEventTrigger>();
            AssignedTrigger.Init(AssignedAnimator, PlayerHandler);
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : First stage complete|n°{AssignedNumber}");
#endif
            this.gameObject.name = AssignedCharacter.CharacterInfoData.ChatacterName + assignedNumber;
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : all stage complete, ready to go|n°{AssignedNumber}");
#endif
            init=true;
            if(assignedNumber==0) activate=true;
            Activate = activate;
            if (activate) return;
            this.gameObject.SetActive(false);
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
            PlayerOffHandler.HealReceive -= heal;
            PlayerOffHandler.damageReceive -= Hit;
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
            Activate = false;
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : Deactivating |n°{AssignedNumber}");
#endif
        }
        void OnEnable()
        {
            if (!init||!IsAlive) return;
            Activate = true;
#if DEBUG || UNITY_EDITOR
            Debug.Log($"CHARACTER SYS : Activating |n°{AssignedNumber}");
#endif
#if UNITY_EDITOR

#endif
            PlayerStateMachine stm = PlayerHandler.playerStateMachine;
            PlayerHandler.InitliazeCollider(this.gameObject);
            stm.ChangeCharacter(this);
            InitializeWeaponClass(stm);
        }
        #region Character Vitals
        void Hit(int damage, string entityORreason)
        {
            if (!Activate)return;
            health =health-damage;
            healthPercetange?.Invoke(HealthToHealthPercentage(health, maxHealth),false);
            if (0 >= health) { IsAlive = false; Debug.LogWarning($"{AssignedCharacter.CharacterInfoData.ChatacterName} is dead,with the following reason: {entityORreason}; Damage receive {damage}"); CharacterDead.Invoke();  }
            Debug.Log(health);
        }
        void heal(int healing,bool healOverwrite = false)
        {
            if(!Activate && !healOverwrite) return;
            if (!IsAlive) CharacterRespawn?.Invoke();
            health = health+healing;
            if(health > maxHealth) health = maxHealth;
            healthPercetange?.Invoke(HealthToHealthPercentage(health, maxHealth),healOverwrite);
        }
        float HealthToHealthPercentage(int health, int maxHealth)
        {
            return (health * 100) / maxHealth;
        }
        #endregion
    }
}
