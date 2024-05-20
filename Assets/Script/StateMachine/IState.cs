using UnityEngine;

namespace BleizEntertainment
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void HandleInput();
        public void Update();
        public void PhysicUpdate();
        public void OnAnimationEnterEvent();
        public void OnAnimationExitEvent();
        public void OnAnimationTransitionEvent();
        public void OnTriggerEnter(Collider collider);
        public void OnTriggerExit(Collider collider);
    }
}
