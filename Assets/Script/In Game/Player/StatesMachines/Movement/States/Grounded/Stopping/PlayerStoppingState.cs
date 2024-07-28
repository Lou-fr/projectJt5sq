using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerStoppingState : PlayerGroundedState
    {
        public PlayerStoppingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }
        public override void Enter()
        {
            stateMachine.reasubleData.MovementSpeedModifier = 0f;
            base.Enter();
            StartAnimation(stateMachine.Player.animationData.stoppingParameterHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.animationData.stoppingParameterHash);
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
            stateMachine.Player.Input.playerActions.movement.started += OnMovementStarted;
        }
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            stateMachine.Player.Input.playerActions.movement.started += OnMovementStarted;
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
