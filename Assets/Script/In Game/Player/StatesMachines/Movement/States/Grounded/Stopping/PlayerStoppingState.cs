using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerStoppingState : PlayerGroundedState
    {
        public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            stateMachine.reasubleData.MovementSpeedModifier = 0f;
            base.Enter();
            StartAnimation(stateMachine._Player.animationData.stoppingParameterHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.stoppingParameterHash);
        }
        public override void PhysicUpdate()
        {
            base.PhysicUpdate();

            RotateTowardsTargetRotation();
            if (!IsMovingHorizontally())
            {
                return;
            }
            DecelerateHorizontally();
        }
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            stateMachine.ChangeState(stateMachine.idlingState);
        }
        #region Reusable Methods
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();
            stateMachine._Player.Input.playerActions.movement.started += OnMovementStarted;
        }
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            stateMachine._Player.Input.playerActions.movement.started += OnMovementStarted;
        }


        #endregion
        #region Input Methods
        protected override void OnMovementCancel(InputAction.CallbackContext context)
        {
        }
        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion
    }
}
