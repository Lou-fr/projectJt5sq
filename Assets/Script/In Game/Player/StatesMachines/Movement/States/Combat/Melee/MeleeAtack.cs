using UnityEngine;

namespace BleizEntertainment
{
    public class MeleeBaseAtack : CombatStates
    {
        private PlayerBasicAttackDataState basicAttackData;
        private float timebetwinAttack;
        private float enterTime;
        private float lastAttack;
        private float timeBeforeResetConsecutive;
        private int currentConsecutiveAttack;
        private int maxConsecutiveAttack;
        private bool isConsecutive;
        public MeleeBaseAtack(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            basicAttackData = combatData.MeleeCombatData.BasicAttackData;
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine._Player.Input.playerActions.movement.Disable();
            timeBeforeResetConsecutive = basicAttackData.TimeBeforeResetConsecutive;
            timebetwinAttack = basicAttackData.TimeBetwinAttack;
            maxConsecutiveAttack = basicAttackData.maxConsecutiveAttack;
            isConsecutive = false;
            currentConsecutiveAttack = 1;
            enterTime = Time.time;
            if (enterTime - stateMachine.reasubleCombatData.BasicAtackEnterTime < timeBeforeResetConsecutive)
            {
                isConsecutive = true;
                currentConsecutiveAttack = stateMachine.reasubleCombatData.currentConsecutiveAttack;
            }
            Attack();
        }
        public override void Exit()
        {
            enterTime = Time.time;
            StopAnimation(stateMachine._Player.animationData.combatAnimationParameterHash);
            base.Exit();
            stateMachine.reasubleCombatData.BasicAtackEnterTime = enterTime;
            stateMachine.reasubleCombatData.currentConsecutiveAttack = currentConsecutiveAttack;
            stateMachine.reasubleCombatData.LastAtackEnterTime = lastAttack;
        }
        #region Main
        void Attack()
        {
            if (maxConsecutiveAttack < currentConsecutiveAttack)
            {
                isConsecutive = false;
                currentConsecutiveAttack = 1;
            }
            changeAnimationCount(stateMachine._Player.animationData.ackNumberAnimationParameterHash, currentConsecutiveAttack);
            StartAnimation(stateMachine._Player.animationData.combatAnimationParameterHash);
            if (!isConsecutive)
            {
                FirstAck();
                return;
            }
            if (enterTime - lastAttack < timebetwinAttack) { stateMachine.ChangeState(stateMachine.idlingState); return; }
            if (currentConsecutiveAttack == 2)
            {
                SecondAck();
                return;
            }
            if (currentConsecutiveAttack == 3)
            {
                ThirdAck();
                return;
            }
            if (currentConsecutiveAttack == 4)
            {
                FourthAck();
                return;
            }
            if (currentConsecutiveAttack == 5)
            {
                FifthAck();
                return;
            }
            Debug.LogWarning("Warning: attack hasn't run has intend, returning to idling without casting any attack");
            stateMachine.ChangeState(stateMachine.idlingState);
        }


        void FirstAck()
        {
            Debug.Log("First attack" + basicAttackData.FirstAttackDmg);
            lastAttack = Time.time;
            currentConsecutiveAttack++;    
        }
        private void SecondAck()
        {
            Debug.Log("Second attack" + basicAttackData.FirstAttackDmg * basicAttackData.SecondAttackM);
            lastAttack = Time.time;
            currentConsecutiveAttack++;
        }
        private void ThirdAck()
        {
            Debug.Log("Third attack" + basicAttackData.FirstAttackDmg * basicAttackData.ThirdAttackM);
            lastAttack = Time.time;
            currentConsecutiveAttack++;
        }
        private void FourthAck()
        {
            Debug.Log("Fourth attack" + basicAttackData.FirstAttackDmg * basicAttackData.FourthAttackM);
            lastAttack = Time.time;
            currentConsecutiveAttack++;
        }
        private void FifthAck()
        {
            Debug.Log("Fifth attack" + basicAttackData.FirstAttackDmg * basicAttackData.FifthAttackM);
            lastAttack = Time.time;
            currentConsecutiveAttack++;
        }
        public override void OnAnimationTransitionEvent()
        {
            stateMachine._Player.Input.playerActions.movement.Enable();   
        }
        public override void OnAnimationExitEvent()
        {
            stateMachine.ChangeState(stateMachine.idlingState);
        }
        #endregion
    }
}
