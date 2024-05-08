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
        public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {

        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            enterTime = Time.time;
            stateMachine.reasubleData.MovementSpeedModifier = 0f;
            stateMachine.reasubleData.CurrentJumpForce = airborneData.jumpData.StationnaryForce;
            StartAnimation(stateMachine._Player.animationData.iddleParameterHash);
            animationNumber = stateMachine._Player.animationData.PlayOtherAnimation;
            ResetVelocity();
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine._Player.animationData.iddleParameterHash);
            StopAnimation(stateMachine._Player.animationData.TidlleAnimationParameterHash);
            StopAnimation(stateMachine._Player.animationData.idlleAnimationParameterHash);
            stateMachine._Player.animationData.PlayOtherAnimation = animationNumber;
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
            if(Time.time - enterTime > stateMachine._Player.animationData.timeBeforeIdleAnimation)
            {
                int calledAnimation;
                if (animationNumber)
                {
                    calledAnimation = stateMachine._Player.animationData.TidlleAnimationParameterHash;
                }
                else calledAnimation = stateMachine._Player.animationData.idlleAnimationParameterHash;
                if(!debugedOnce) { Debug.Log(calledAnimation);debugedOnce = !debugedOnce; }

                StartAnimation(calledAnimation);
            }
        }
        public override void OnAnimationTransitionEvent()
        {
            IsPlayedSinceLast = !IsPlayedSinceLast;
            if(!animationNumber){ StopAnimation(stateMachine._Player.animationData.idlleAnimationParameterHash); return; }
            StopAnimation(stateMachine._Player.animationData.TidlleAnimationParameterHash);


        }

        #endregion
    }
}
