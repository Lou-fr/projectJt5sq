using System;
using Unity.Netcode;
using UnityEngine;

namespace BleizEntertainment
{
    public abstract class StateMachine
    {
        protected IState currentState;

        public void ChangeState(IState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
        public void HandleInput()
        {
            currentState?.HandleInput();
        }
        public void Update()
        {
            currentState?.Update();
        }
        public void PhysicUpdate()
        {
            currentState?.PhysicUpdate();
        }
        public void OnAnimationEnterEvent()
        {
            currentState?.OnAnimationEnterEvent();
        }
        public void OnAnimationExitEvent()
        {
            currentState?.OnAnimationExitEvent();
        }
        public void OnAnimationTransitionEvent()
        {
            currentState?.OnAnimationTransitionEvent();
        }
        public void OnTriggerEnter(Collider collider)
        {
            currentState?.OnTriggerEnter(collider);
        }
        public void OnTriggerExit(Collider collider)
        {
            currentState?.OnTriggerExit(collider);
        }
    }
}
