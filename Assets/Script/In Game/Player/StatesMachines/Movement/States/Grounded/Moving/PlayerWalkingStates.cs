using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class PlayerWalkingState : PlayerMovingState
    {
        public PlayerWalkingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods

        public override void Enter()
        {
            StartAnimation(stateMachine._Player.animationData.walkParameterHash);
            base.Enter();
            stateMachine.reasubleData.MovementSpeedModifier = movementData.WalkData.speedModifier;
            stateMachine.reasubleData.CurrentJumpForce = airborneData.jumpData.WeakForce;
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.walkParameterHash);
        }
        protected override void OnMovementCancel(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.lightStoppingState);
        }
        #endregion
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            stateMachine.ChangeState(stateMachine.runningState);
        }

    }
}
