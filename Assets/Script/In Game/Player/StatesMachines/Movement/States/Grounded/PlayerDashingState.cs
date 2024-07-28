using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData _dashData;
        private float startTime;
        private int consecutiveDashedUsed;
        private bool shouldKeepRotating;
        public PlayerDashingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            _dashData = movementData.dashData;
        }
        public override void Enter()
        {
            stateMachine.reasubleData.MovementSpeedModifier = _dashData.SpeedModifier;
            base.Enter();
            StartAnimation(stateMachine.Player.animationData.dashParameterHash);
            stateMachine.reasubleData.RotationData = _dashData.RotationData;
            stateMachine.reasubleData.CurrentJumpForce = airborneData.jumpData.StrongForce;
            AddForceOnTransitionFromStationaryState();
            UpdateConsecutiveDashes();
            shouldKeepRotating = stateMachine.reasubleData.MovementInput != Vector2.zero;
            startTime = Time.time;
        }
        public override void Exit()
        {
            base.Exit();
            SetBaseRotationData();
            StopAnimation(stateMachine.Player.animationData.dashParameterHash);
        }

        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            if (stateMachine.reasubleData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.heavyStoppingState);
                return;
            }
            stateMachine.ChangeState(stateMachine.sprintingState);

        }
        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            if (!shouldKeepRotating) return;
            RotateTowardsTargetRotation();
        }
        #region Main Methods

        private void AddForceOnTransitionFromStationaryState()
        {
            if (stateMachine.reasubleData.MovementInput != Vector2.zero) return;
            Vector3 characterRotationDirection = stateMachine.Player.transform.forward;
            characterRotationDirection.y = 0f;
            UpdateTargetRotation(characterRotationDirection, false);
            stateMachine.Player.rigidBody.linearVelocity = characterRotationDirection * GetMovementSpeed();
        }
        private void UpdateConsecutiveDashes()
        {
            if (!IsConsecutive())
            {
                consecutiveDashedUsed = 0;
            }
            ++consecutiveDashedUsed;

            if (consecutiveDashedUsed == _dashData.ConsecutiveDashesLimitAmount)
            {
                consecutiveDashedUsed = 0;
                stateMachine.Player.Input.DisableActionFor(stateMachine.Player.Input.playerActions.Dash, _dashData.DashLimitReachedCooldown);
            }
        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + _dashData.TimeToBeConsiderConsecutive;
        }
        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();
            stateMachine.Player.Input.playerActions.movement.performed += OnMovementPerformed;
        }
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            stateMachine.Player.Input.playerActions.movement.performed += OnMovementPerformed;

        }
        #endregion
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            shouldKeepRotating = true;
        }
        protected override void OnMovementCancel(InputAction.CallbackContext context)
        {

        }
        protected override void OnDashStarted(InputAction.CallbackContext context)
        {

        }
    }
}
