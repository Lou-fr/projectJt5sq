using System;
using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        private float enterTime;
        private bool IsPlayedSinceLast;
        private bool animationNumber;
        bool debugedOnce;
        public PlayerIdlingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            enterTime = Time.time;
            stateMachine.reasubleData.MovementSpeedModifier = 0f;
            stateMachine.reasubleData.CurrentJumpForce = airborneData.jumpData.StationnaryForce;
            StartAnimation(stateMachine.Player.animationData.iddleParameterHash);
            animationNumber = stateMachine.Player.animationData.PlayOtherAnimation;
            ResetVelocity();
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.animationData.iddleParameterHash);
            StopAnimation(stateMachine.Player.animationData.TidlleAnimationParameterHash);
            StopAnimation(stateMachine.Player.animationData.idlleAnimationParameterHash);
            stateMachine.Player.animationData.PlayOtherAnimation = animationNumber;
        }
        public override void Update()
        {
            base.Update();
            if (stateMachine.reasubleData.MovementInput == Vector2.zero) return;
            OnMove();
        }
        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            if(stateMachine.reasubleData.MovementInput != Vector2.zero)return;
            if (IsPlayedSinceLast)
            {
                enterTime = Time.time;
                IsPlayedSinceLast = !IsPlayedSinceLast;
                animationNumber = !animationNumber;
                debugedOnce = !debugedOnce;
                Debug.Log(animationNumber);
                return;
            }
            if(Time.time - enterTime > stateMachine.Player.animationData.timeBeforeIdleAnimation)
            {
                int calledAnimation;
                if (animationNumber)
                {
                    calledAnimation = stateMachine.Player.animationData.TidlleAnimationParameterHash;
                }
                else calledAnimation = stateMachine.Player.animationData.idlleAnimationParameterHash;
                if(!debugedOnce) { Debug.Log(calledAnimation);debugedOnce = !debugedOnce; }

                StartAnimation(calledAnimation);
            }
        }
        public override void OnAnimationTransitionEvent()
        {
            IsPlayedSinceLast = !IsPlayedSinceLast;
            if(!animationNumber){ StopAnimation(stateMachine.Player.animationData.idlleAnimationParameterHash); return; }
            StopAnimation(stateMachine.Player.animationData.TidlleAnimationParameterHash);


        }

        #endregion
    }
}
