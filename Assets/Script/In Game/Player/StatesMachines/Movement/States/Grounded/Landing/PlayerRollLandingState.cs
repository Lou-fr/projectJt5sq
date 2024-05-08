using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerRollLandingState : PlayerLandingState
    {
        public PlayerRollLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            stateMachine.reasubleData.MovementSpeedModifier = movementData.RollData.SpeedModifier;
            base.Enter();
            stateMachine.reasubleData.ShouldSprint = false;
            StartAnimation(stateMachine._Player.animationData.RollParameterHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.RollParameterHash);
        }
        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            if (stateMachine.reasubleData.MovementInput != Vector2.zero)
            {
                return;
            }

            RotateTowardsTargetRotation();
        }
        public override void OnAnimationTransitionEvent()
        {
            if (stateMachine.reasubleData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.mediumStoppingState);

                return;
            }

            OnMove();
        }
        protected override void OnJumpStarted(InputAction.CallbackContext context) { }
    }
}
