using Unity.Netcode;
using UnityEngine;

namespace BleizEntertainment
{
    public class PlayerAnimationEventTrigger : MonoBehaviour
    {
        private PlayerOffHandler AssignedPlayer;
        private Animator assignedAnimator;
        public void Init(Animator animator, PlayerOffHandler player)
        {
            assignedAnimator = animator;
            AssignedPlayer = player;
        }

        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            AssignedPlayer.TriggerOnMovementStateAnimationEnterEvent();
        }

        public void TriggerOnMovementStateAnimationExitEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            AssignedPlayer.OnMovementStateAnimationExitEvent();
        }

        public void TriggerOnMovementStateAnimationTransitionEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            AssignedPlayer.OnMovementStateAnimationTransitionEvent();
        }

        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return assignedAnimator.IsInTransition(layerIndex);
        }
    }
#if BleizOnline
    public class PlayerAnimationEventTrigger : NetworkBehaviour
    {
        private PlayerHandler AssignedPlayer;
        private Animator assignedAnimator;
        public void Init(Animator animator, PlayerHandler player)
        {
            assignedAnimator = animator;
            AssignedPlayer = player;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner) return;
        }

        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            if (!IsOwner) return;
            if (IsInAnimationTransition())
            {
                return;
            }

            AssignedPlayer.TriggerOnMovementStateAnimationEnterEvent();
        }

        public void TriggerOnMovementStateAnimationExitEvent()
        {
            if (!IsOwner) return;
            if (IsInAnimationTransition())
            {
                return;
            }

            AssignedPlayer.OnMovementStateAnimationExitEvent();
        }

        public void TriggerOnMovementStateAnimationTransitionEvent()
        {
            if (!IsOwner) return;
            if (IsInAnimationTransition())
            {
                return;
            }

            AssignedPlayer.OnMovementStateAnimationTransitionEvent();
        }

        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return assignedAnimator.IsInTransition(layerIndex);
        }
    }
#endif
}
