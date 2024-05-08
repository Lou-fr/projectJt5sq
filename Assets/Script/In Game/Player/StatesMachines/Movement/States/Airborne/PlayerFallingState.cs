using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerFallingState : PlayerAriborneState
    {
        private PlayerFallData fallData;
        private Vector3 playerPosOnEnter;
        public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            fallData = airborneData.fallData;
        }
        #region IState
        public override void Enter()
        {
            base.Enter();
            stateMachine.reasubleData.MovementSpeedModifier = 0f;
            playerPosOnEnter = stateMachine._Player.transform.position;
            ResetVerticalVelocity();
            StartAnimation(stateMachine._Player.animationData.FallParameterHash);
            stateMachine._Player.Input.playerActions.movement.Disable();
        }
        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            LimitVerticalVelocity();
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.FallParameterHash);
            stateMachine._Player.Input.playerActions.movement.Enable();

        }
        #endregion
        protected override void ResetSprintState()
        {

        }
        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance = playerPosOnEnter.y - stateMachine._Player.transform.position.y;
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
            if(playerVerticalVelocity.y >= -fallData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitedVelocity = new Vector3(0f, -fallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);
            stateMachine._Player.rigidBody.AddForce(limitedVelocity,ForceMode.VelocityChange);
        }
        #endregion
    }
}
