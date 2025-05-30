using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerHardFallingState : PlayerLandingState
    {
        public PlayerHardFallingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine._Player.Input.playerActions.movement.Disable();
            ResetVelocity();
            StartAnimation(stateMachine._Player.animationData.hLandParameterHash);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine._Player.Input.playerActions.movement.Enable();
            StopAnimation(stateMachine._Player.animationData.hLandParameterHash);
        }
        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            if (!IsMovingHorizontally())
            {
                return;
            }

            ResetVelocity();
        }
        public override void OnAnimationExitEvent()
        {
            stateMachine._Player.Input.playerActions.movement.Enable();
        }
        public override void OnAnimationTransitionEvent()
        {
            stateMachine.ChangeState(stateMachine.idlingState);
        }
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();

            stateMachine._Player.Input.playerActions.movement.started += OnMovementStarted;
        }
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();

            stateMachine._Player.Input.playerActions.movement.started -= OnMovementStarted;
        }
        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            
        }
    }
}
