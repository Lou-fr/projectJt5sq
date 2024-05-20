using UnityEngine;

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
            StartAnimation(stateMachine._Player.animationData.airborneParameterHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.airborneParameterHash);
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
    }
}
