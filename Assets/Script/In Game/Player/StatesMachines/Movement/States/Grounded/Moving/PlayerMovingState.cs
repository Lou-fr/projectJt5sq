using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerMovingState : PlayerGroundedState
    {
        public PlayerMovingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine._Player.animationData.movingParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.movingParameterHash);
        }
    }
}
