using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerSprintData sprintData;
        private float startTime;
        private bool keepSprinting;
        private bool ShouldResetSprintingState;
        public PlayerSprintingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            sprintData = movementData.SprintData;
        }
        public override void Enter()
        {
            StartAnimation(stateMachine._Player.animationData.sprintParameterHash);
            base.Enter();
            stateMachine.reasubleData.MovementSpeedModifier = sprintData.SpeedModifier;
            stateMachine.reasubleData.CurrentJumpForce = airborneData.jumpData.StrongForce;
            ShouldResetSprintingState = true;
            startTime = Time.time;
        }
        public override void Exit()
        {
            base.Exit();
            if(ShouldResetSprintingState)
            {
                keepSprinting = false;
                stateMachine.reasubleData.ShouldSprint = false;
            }
            StopAnimation(stateMachine._Player.animationData.sprintParameterHash);
        }
        public override void Update()
        {
            base.Update();
            if (keepSprinting) return;
            if (Time.time < startTime + sprintData.SprintToRunTime)
            {
                return;
            }
            StopSprinting();
        }
        #region Main Methods
        private void StopSprinting()
        {
            if (stateMachine.reasubleData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.idlingState);
                return;
            }
            stateMachine.ChangeState(stateMachine.runningState);
        }
        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();
            stateMachine._Player.Input.playerActions.Sprint.performed += OnSprintPerformed;
        }
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            stateMachine._Player.Input.playerActions.Sprint.performed -= OnSprintPerformed;

        }
        protected override void OnMovementCancel(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.heavyStoppingState);
        }
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            ShouldResetSprintingState = false;
            base.OnJumpStarted(context);
        }
        protected override void OnFall()
        {
            ShouldResetSprintingState = false ;
            base.OnFall();
        }
        #endregion
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            keepSprinting = true;
            stateMachine.reasubleData.ShouldSprint = true;
        }
    }
}
