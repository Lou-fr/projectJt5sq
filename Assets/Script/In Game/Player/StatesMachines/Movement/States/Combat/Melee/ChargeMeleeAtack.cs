using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BleizEntertainment
{
    public class ChargeMeleeAtack : CombatStates
    {
        private PlayerBasicChargedAtack ChargedAtack;
        private float enterTime;
        public ChargeMeleeAtack(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            ChargedAtack = combatData.MeleeCombatData.BasicChargedAtack;
        }
        public override void Enter()
        {
            base.Enter();
            enterTime = Time.time;
            if(enterTime - stateMachine.reasubleCombatData.BasicChargeAttackEnterTime > ChargedAtack.TimeBitwinChargeAttack)
            {
                ChargeAttack();
                stateMachine.reasubleCombatData.BasicAtackEnterTime = combatData.MeleeCombatData.BasicAttackData.TimeBeforeResetConsecutive+0.1f;
                return;
            }
            stateMachine.ChangeState(stateMachine.idlingState);
        }
        protected override void OnAttackStarted(InputAction.CallbackContext context)
        {
            
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.reasubleCombatData.BasicChargeAttackEnterTime = enterTime;
        }

        #region Main
        private void ChargeAttack()
        {
            Debug.Log("Charged Attack" + ChargedAtack.BaseChargeAttackDamage);
            stateMachine.ChangeState(stateMachine.idlingState);
        }
        #endregion

    }
}
