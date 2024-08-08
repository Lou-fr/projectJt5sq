using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerRunningState : PlayerMovingState
    {
        private float startTime;
        private PlayerSprintData sprintData;
        public PlayerRunningState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            sprintData = movementData.SprintData;
        }
        #region IState Methods

        public override void Enter()
        {
            StartAnimation(stateMachine._Player.animationData.runParameterHash);
            base.Enter();
            stateMachine.reasubleData.MovementSpeedModifier = movementData.RunData.speedModifier;
            stateMachine.reasubleData.CurrentJumpForce = airborneData.jumpData.MediumForce;
            startTime = Time.time;
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.runParameterHash);
        }
        public override void Update()
        {
            base.Update();
            if (!stateMachine.reasubleData.ShouldWalk) return;
            if (Time.time > startTime + sprintData.RunToWalkTime) return;
            StopRunning();

        }

        private void StopRunning()
        {
            if (stateMachine.reasubleData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.idlingState);
                return;
            }
            stateMachine.ChangeState(stateMachine.walkingState);
        }
        #endregion
        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            stateMachine.ChangeState(stateMachine.walkingState);
        }
        protected override void OnMovementCancel(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.mediumStoppingState);
        }
        #endregion
    }
}
