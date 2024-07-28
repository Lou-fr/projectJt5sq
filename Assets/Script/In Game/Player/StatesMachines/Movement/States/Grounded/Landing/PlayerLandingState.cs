using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerLandingState : PlayerGroundedState
    {
        public PlayerLandingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine.Player.animationData.landingParameterHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.animationData.landingParameterHash);
        }
    }
}
