namespace BleizEntertainment
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        public PlayerLightStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.reasubleData.MovementDecelerationforce = movementData.StopData.LightDecelerationForce;
            stateMachine.reasubleData.CurrentJumpForce = airborneData.jumpData.WeakForce;
        }
    }
}
