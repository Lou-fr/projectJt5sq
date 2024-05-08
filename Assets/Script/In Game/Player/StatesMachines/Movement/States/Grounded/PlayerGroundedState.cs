using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerGroundedState : PlayerMovementStates
    {
        private SlopeData slopeData;
        public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            slopeData = stateMachine._Player.collidersUtility.CapsuleCollidersUtility.slopeData;
        }
        public override void Enter()
        {
            base.Enter();
            UpdateShouldSprintState();
            StartAnimation(stateMachine._Player.animationData.GroundedParameterHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.GroundedParameterHash);
        }
        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            Float();
        }

        private void Float()
        {
            Vector3 capsuleCollideCenterInWorldSpace = stateMachine._Player.collidersUtility.CapsuleCollidersUtility.capsuleColliderData.collider.bounds.center;
            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleCollideCenterInWorldSpace, Vector3.down);
            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, slopeData.FloatRayDistance, stateMachine._Player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);
                if (slopeSpeedModifier == 0) return;
                float distanceToFloatinPoint = stateMachine._Player.collidersUtility.CapsuleCollidersUtility.capsuleColliderData.ColliderCenterInLocalSpace.y * stateMachine._Player.transform.localScale.y - hit.distance;
                if (distanceToFloatinPoint == 0) return;
                float ammountToLift = distanceToFloatinPoint * slopeData.StepReachForce - GetPlayerVerticalVelocity().y;
                Vector3 liftForce = new Vector3(0f, ammountToLift, 0f);
                stateMachine._Player.rigidBody.AddForce(liftForce, ForceMode.VelocityChange);
            }

        }

        private float SetSlopeSpeedModifierOnAngle(float groundAngle)
        {
            float slopeSpeedModifier = movementData.SlopeSppedAngles.Evaluate(groundAngle);
            stateMachine.reasubleData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;
            return slopeSpeedModifier;
        }

        private bool IsThereGroundUnderneath()
        {
            PlayerTriggerColliderData triggerColliderData = stateMachine._Player.collidersUtility.TriggerColliderData;
            Vector3 groundColliderCenterInWorldSpace = triggerColliderData.GroundCheckCollider.bounds.center;
            Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, triggerColliderData.GroundCheckColliderVerticalExtents, triggerColliderData.GroundCheckCollider.transform.rotation, stateMachine._Player.layerData.groundLayer, QueryTriggerInteraction.Ignore);
            return overlappedGroundColliders.Length > 0;
        }
        #region ReusableData
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();
            stateMachine._Player.Input.playerActions.movement.canceled += OnMovementCancel;
            stateMachine._Player.Input.playerActions.Dash.started += OnDashStarted;
            stateMachine._Player.Input.playerActions.Jump.started += OnJumpStarted;
        }
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            stateMachine._Player.Input.playerActions.movement.canceled -= OnMovementCancel;
            stateMachine._Player.Input.playerActions.Dash.started -= OnDashStarted;
            stateMachine._Player.Input.playerActions.Jump.started += OnJumpStarted;
        }
        protected virtual void OnMove()
        {
            if (stateMachine.reasubleData.ShouldSprint)
            {
                stateMachine.ChangeState(stateMachine.sprintingState);
                return;
            }
            if (stateMachine.reasubleData.ShouldWalk) { stateMachine.ChangeState(stateMachine.walkingState); return; }
            stateMachine.ChangeState(stateMachine.runningState);
        }
        protected override void OnContactWithGroundExited(Collider collider)
        {
            if (IsThereGroundUnderneath())
            {
                return;
            }
            Vector3 capsuleColliderCenterInWorldSpace = stateMachine._Player.collidersUtility.CapsuleCollidersUtility.capsuleColliderData.collider.bounds.center;
            Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - stateMachine._Player.collidersUtility.CapsuleCollidersUtility.capsuleColliderData.ColliderVerticalExtents, Vector3.down);
            if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, movementData.GroundToFallRayDistance, stateMachine._Player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
            {
                OnFall();
            }
        }

        protected virtual void OnFall()
        {
            stateMachine.ChangeState(stateMachine.fallingState);
        }

        private void UpdateShouldSprintState()
        {
            if (!stateMachine.reasubleData.ShouldSprint)
            {
                return;
            }
            if (stateMachine.reasubleData.MovementInput != Vector2.zero)
            {
                return;
            }
            stateMachine.reasubleData.ShouldSprint = false;
        }
        #endregion
        #region Input Methods
        protected virtual void OnMovementCancel(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.idlingState);
        }
        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.dashingState);
        }
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.jumpingState);
        }

        #endregion
    }
}
