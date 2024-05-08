namespace BleizEntertainment
{
    public class PlayerHeavyStoppingState : PlayerStoppingState
    {
        public PlayerHeavyStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.reasubleData.MovementDecelerationforce = movementData.StopData.HeavyDecelerationForce;
            stateMachine.reasubleData.CurrentJumpForce = airborneData.jumpData.StrongForce;
            StartAnimation(stateMachine._Player.animationData.hStopParameterHash);
        }
        protected override void OnMove()
        {
            if (stateMachine.reasubleData.ShouldWalk)
            {
                return;
            }
            stateMachine.ChangeState(stateMachine.runningState);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.hStopParameterHash);
        }
    }
}
