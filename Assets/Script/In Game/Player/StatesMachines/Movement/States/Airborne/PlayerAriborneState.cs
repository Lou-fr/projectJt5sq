using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerAriborneState : PlayerMovementStates
    {
        public PlayerAriborneState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            ResetSprintState();
            StartAnimation(stateMachine.Player.animationData.airborneParameterHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.animationData.airborneParameterHash);
        }
        #region Reusable Methods
        protected override void OnContactWithGround(Collider collider)
        {
            stateMachine.ChangeState(stateMachine.idlingState);
        }
        protected virtual void ResetSprintState()
        {
            stateMachine.reasubleData.ShouldSprint = false;
        }
        #endregion
        protected override void SwitchCharater1(InputAction.CallbackContext context) { }
        protected override void SwitchCharater2(InputAction.CallbackContext context) { }
        protected override void SwitchCharater3(InputAction.CallbackContext context) { }
        protected override void SwitchCharater4(InputAction.CallbackContext context) { }
    }
}
