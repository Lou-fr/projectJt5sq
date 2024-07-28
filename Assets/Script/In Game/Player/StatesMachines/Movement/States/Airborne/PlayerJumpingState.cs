using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerJumpingState : PlayerAriborneState
    {
        private bool shoulKeepRotating;
        private PlayerJumpData jumpData;
        private bool canStartFalling;
        public PlayerJumpingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            jumpData = airborneData.jumpData;
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.reasubleData.RotationData = jumpData.RotationData;
            stateMachine.reasubleData.MovementSpeedModifier = 0f;
            shoulKeepRotating = stateMachine.reasubleData.MovementInput != Vector2.zero;
            stateMachine.reasubleData.MovementDecelerationforce = jumpData.DecelerationForce;
            Jump();
        }
        public override void Update()
        {
            base.Update();
            if (!canStartFalling && IsMovingUp(0f) && IsMovingDown(0.5f))
            {
                canStartFalling = true;
            }

            if (canStartFalling)
            {
                return;
            }

            stateMachine.ChangeState(stateMachine.fallingState);
        }
        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            if(shoulKeepRotating)
            {
                RotateTowardsTargetRotation();
            }
            if (IsMovingUp())
            {
                DecelerateVertically();
            }
        }
        public override void Exit()
        {
            base.Exit();
            SetBaseRotationData();
            canStartFalling = false;
        }
        #region Reusable Methods
        protected override void ResetSprintState()
        {
            
        }
        #endregion
        #region Main Methods
        private void Jump()
        {
            Vector3 jumpForce = stateMachine.reasubleData.CurrentJumpForce;
            Vector3 jumpDirection = stateMachine.CharacterHandler.transform.forward;
            if (shoulKeepRotating)
            {
                jumpDirection = GetTargetRotationDirection(stateMachine.reasubleData.CurrentTargetRotation.y);
            }
            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;
            Vector3 capsuleColliderCenterInWWorldSpace = stateMachine.Player.collidersUtility.CapsuleCollidersUtility.capsuleColliderData.collider.bounds.center;
            Ray downwardRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWWorldSpace, Vector3.down);
            if (Physics.Raycast(downwardRayFromCapsuleCenter, out RaycastHit hit, jumpData.JumpToGroundTayDistance,stateMachine.Player.layerData.groundLayer,QueryTriggerInteraction.Ignore ))
            {
                float groundAndle = Vector3.Angle(hit.normal,-downwardRayFromCapsuleCenter.direction);
                if (IsMovingUp())
                {
                    float forceModifier = jumpData.JumpForceModifierOnSlopeUpWards.Evaluate(groundAndle);
                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;

                }
                if (IsMovingDown())
                {
                    float forceModifier = jumpData.JumpForceModifierOnSlopeDownWards.Evaluate(groundAndle);
                    jumpForce.y *= forceModifier;
                }
            }

            ResetVelocity();
            stateMachine.Player.rigidBody.AddForce(jumpForce, ForceMode.VelocityChange);
        }
        #endregion
    }
}
