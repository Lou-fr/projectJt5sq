using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerMovingState : PlayerGroundedState
    {
        public PlayerMovingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            StartAnimation(stateMachine._Player.animationData.movingParameterHash);
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.movingParameterHash);
        }
    }
}
