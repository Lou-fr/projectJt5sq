using System;
using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerStateMachine : StateMachine
    {
        public CharacterOffHandler CharacterHandler { get; protected set; }//For mutliplayer swicht back to CharacterHandler
        public PlayerOffHandler Player { get; protected set; }//For mutliplayer swicht back to playerhandler
        public PlayerSO Character { get; }
        public Animator assignedAnimator { get; private set; }
        //public ClientNetworkAnimator assignedNetworkAnimator { get; private set; }
        public PlayerStateMovementReusableData reasubleData { get; }
        public PlayerStateReusableCombatData reasubleCombatData { get; private set; }
        public PlayerIdlingState idlingState { get; }
        public PlayerWalkingState walkingState { get; }
        public PlayerRunningState runningState { get; }
        public PlayerSprintingState sprintingState { get; }
        public PlayerDashingState dashingState { get; }
        public PlayerLightStoppingState lightStoppingState { get; }
        public PlayerMediumStoppingState mediumStoppingState { get; }
        public PlayerHeavyStoppingState heavyStoppingState { get; }
        public PlayerJumpingState jumpingState { get; }
        public PlayerFallingState fallingState { get; }
        public PlayerLightLandingState lightLandingState { get; }
        public PlayerHardFallingState hardFallingState { get; }
        public PlayerRollLandingState rollLandingState { get; }

        public MeleeBaseAtack MeleeAtackState { get; private set; }
        public ChargeMeleeAtack ChargeMeleeAtackState { get; private set; }
        public ChargeAttackHeavyMelee ChargeHeavyMeleeAtackState { get; private set; }
        public PlayerStateMachine(PlayerOffHandler player,CharacterOffHandler characterHandler, PlayerStateMovementReusableData _reusableData)
        {
#if UNITY_EDITOR || DEBUG
            Debug.Log($"STATE MACHINE : Initating state machine", characterHandler);
#endif
            Player = player;
            CharacterHandler = characterHandler;
            Character = characterHandler.AssignedCharacter;
            reasubleData = _reusableData;
            assignedAnimator = characterHandler.AssignedAnimator;
            reasubleCombatData = characterHandler.CombatData;
            //assignedNetworkAnimator = characterHandler.assignedNetworkAnimator;
            sprintingState = new PlayerSprintingState(this);
            idlingState = new PlayerIdlingState(this);
            walkingState = new PlayerWalkingState(this);
            runningState = new PlayerRunningState(this);
            dashingState = new PlayerDashingState(this);
            lightStoppingState = new PlayerLightStoppingState(this);
            mediumStoppingState = new PlayerMediumStoppingState(this);
            heavyStoppingState = new PlayerHeavyStoppingState(this);
            jumpingState = new PlayerJumpingState(this);
            fallingState = new PlayerFallingState(this);
            lightLandingState = new PlayerLightLandingState(this);
            hardFallingState = new PlayerHardFallingState(this);
            rollLandingState = new PlayerRollLandingState(this);
            Debug.Log($"STATE MACHINE : All primary state as been cached, state machine intiated", characterHandler);
        }
        public void PlayerCombatMeleeInstantiate()
        {
#if UNITY_EDITOR
            Debug.Log($"STATE MACHINE : Caching melee combat state");
#endif
            MeleeAtackState = new MeleeBaseAtack(this);
            ChargeMeleeAtackState = new ChargeMeleeAtack(this);
        }
        public void PlayerCombatHeavyMeleeInstantiate()
        {
#if UNITY_EDITOR
            Debug.Log($"STATE MACHINE : Caching melee heavy combat state");
#endif
            MeleeAtackState = new MeleeBaseAtack(this);
            ChargeHeavyMeleeAtackState = new ChargeAttackHeavyMelee(this);
        }
        public void UnloadCombatState()
        {
#if UNITY_EDITOR
            Debug.Log($"STATE MACHINE : DeCaching combat state");
#endif
            if (MeleeAtackState != null)MeleeAtackState = null;
            if(ChargeHeavyMeleeAtackState != null) ChargeHeavyMeleeAtackState = null;
            if(ChargeMeleeAtackState != null) ChargeMeleeAtackState = null;
#if UNITY_EDITOR
            Debug.Log($"STATE MACHINE : DeCaching combat state succes");
#endif
        }
        public void ChangeCharacter(CharacterOffHandler activeCharacter)
        {
            ChangeState(idlingState);
            UnloadCombatState();
            CharacterHandler = activeCharacter;
            assignedAnimator = activeCharacter.AssignedAnimator;
            //assignedNetworkAnimator = activeCharacter.assignedNetworkAnimator;
            reasubleCombatData = activeCharacter.CombatData;

#if UNITY_EDITOR
            Debug.Log($"STATE MACHINE : Active character is now {activeCharacter.AssignedCharacter.CharacterInfoData.ChatacterName} ");
#endif
        }
    }
}
