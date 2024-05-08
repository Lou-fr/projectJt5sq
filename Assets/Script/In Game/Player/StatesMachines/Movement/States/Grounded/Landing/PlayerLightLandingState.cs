using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        public PlayerLightLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.reasubleData.CurrentJumpForce = airborneData.jumpData.StationnaryForce;
            ResetVelocity();
        }
        public override void Update()
        {
            base.Update();
            if(stateMachine.reasubleData.MovementInput == Vector2.zero)
            {
                return;
            }
            OnMove();
        }
        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            if (!IsMovingHorizontally())
            {
                return;
            }

            ResetVelocity();
        }
        public override void OnAnimationTransitionEvent()
        {
            stateMachine.ChangeState(stateMachine.idlingState);
        }
    }
}
