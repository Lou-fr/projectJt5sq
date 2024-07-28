/*using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerState : IState
    {
        protected PlayerStateMachine stateMachine;
        public PlayerState(PlayerStateMachine StateMachine)
        {
            stateMachine = StateMachine;
        }
        public virtual void Enter()
        {
            if(!stateMachine.ActiveStateMachine)return;
        }

        public virtual void Exit()
        {
            if (!stateMachine.ActiveStateMachine) return;
        }

        public virtual void HandleInput()
        {
            if (!stateMachine.ActiveStateMachine) return;
        }

        public virtual void OnAnimationEnterEvent()
        {
            if (!stateMachine.ActiveStateMachine) return;
        }

        public virtual void OnAnimationExitEvent()
        {
            if (!stateMachine.ActiveStateMachine) return;
        }

        public virtual void OnAnimationTransitionEvent()
        {
            if (!stateMachine.ActiveStateMachine) return;
        }

        public virtual  void OnTriggerEnter(Collider collider)
        {
            if (!stateMachine.ActiveStateMachine) return;
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            if (!stateMachine.ActiveStateMachine) return;
        }

        public virtual  void PhysicUpdate()
        {
            if (!stateMachine.ActiveStateMachine) return;
        }

        public virtual void Update()
        {
            if (!stateMachine.ActiveStateMachine) return;
        }
    }
}
*/