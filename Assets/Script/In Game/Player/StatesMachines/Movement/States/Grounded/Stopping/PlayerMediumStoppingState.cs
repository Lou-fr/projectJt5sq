namespace BleizEntertainment
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        public PlayerMediumStoppingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.reasubleData.MovementDecelerationforce = movementData.StopData.MediumDecelerationForce;
            stateMachine.reasubleData.CurrentJumpForce = airborneData.jumpData.MediumForce;
            StartAnimation(stateMachine._Player.animationData.mStopParameterHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.mStopParameterHash);
        }
    }
}
