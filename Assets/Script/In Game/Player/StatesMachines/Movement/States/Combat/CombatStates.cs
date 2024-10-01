using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class CombatStates : PlayerGroundedState
    {
        public CombatStates(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }
        protected override void OnAttackStarted(InputAction.CallbackContext context) { }
        protected override void OnDashStarted(InputAction.CallbackContext context) { }
        protected override void OnJumpStarted(InputAction.CallbackContext context) { }
    }
}
