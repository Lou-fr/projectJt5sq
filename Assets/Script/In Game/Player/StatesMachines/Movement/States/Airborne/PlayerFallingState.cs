using System;
using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerFallingState : PlayerAriborneState
    {
        private PlayerFallData fallData;
        private Vector3 playerPosOnEnter;
        public static Action<float> FallDistance = delegate { };
        public PlayerFallingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            fallData = airborneData.fallData;
        }
        #region IState
        public override void Enter()
        {
            base.Enter();
            stateMachine.reasubleData.MovementSpeedModifier = 0f;
            playerPosOnEnter = stateMachine.CharacterHandler.transform.position;
            ResetVerticalVelocity();
            StartAnimation(stateMachine.Player.animationData.FallParameterHash);
            stateMachine.Player.Input.playerActions.movement.Disable();
        }
        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            LimitVerticalVelocity();
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.animationData.FallParameterHash);
            stateMachine.Player.Input.playerActions.movement.Enable();

        }
        #endregion
        protected override void ResetSprintState()
        {

        }
        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance = playerPosOnEnter.y - stateMachine.CharacterHandler.transform.position.y;
            FallDistance?.Invoke(fallDistance);
            Debug.Log(fallDistance);
            if (fallDistance < fallData.MinimuToBeConsideredHardFall)
            {
                stateMachine.ChangeState(stateMachine.lightLandingState);
                return;
            }
            if (stateMachine.reasubleData.ShouldWalk && !stateMachine.reasubleData.ShouldSprint || stateMachine.reasubleData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.hardFallingState);
                return;
            }
            stateMachine.ChangeState(stateMachine.rollLandingState);
        }
        #region Main
        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
            if (playerVerticalVelocity.y >= -fallData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitedVelocity = new Vector3(0f, -fallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);
            stateMachine.Player.rigidBody.AddForce(limitedVelocity, ForceMode.VelocityChange);
        }
        #endregion
    }
}
