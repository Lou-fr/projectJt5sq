using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerStateMachine : StateMachine
    {
        public Player _Player { get; }
        public PlayerStateMovementReusableData reasubleData { get; }
        public PlayerStateReusableCombatData reasubleCombatData { get; }
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
        public PlayerStateMachine(Player player)
        {
            _Player = player;
            reasubleData = new PlayerStateMovementReusableData();
            reasubleCombatData = new PlayerStateReusableCombatData();
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
        }
        public void PlayerCombatMeleeInstantiate()
        {
            Debug.Log("Caching melee combat state");
            MeleeAtackState = new MeleeBaseAtack(this);
            ChargeMeleeAtackState = new ChargeMeleeAtack(this);
        }
        public void PlayerCombatHeavyMeleeInstantiate()
        {
            Debug.Log("Caching melee heavy combat state");
            MeleeAtackState = new MeleeBaseAtack(this);
            ChargeHeavyMeleeAtackState = new ChargeAttackHeavyMelee(this);
        }
    }
}
