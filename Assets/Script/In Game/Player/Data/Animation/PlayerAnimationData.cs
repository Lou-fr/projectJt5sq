using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerAnimationData
    {
        [Header("State Group parameter Names")]
        [SerializeField] private string groundedParameterName = "Grounded";
        [SerializeField] private string movingParameterName = "Moving";
        [SerializeField] private string stoppingParameterName = "Stopping";
        [SerializeField] private string landingParameterName = "Landing";
        [SerializeField] private string airborneParameterName = "Airborne";

        [Header("State Grounded parameter Names")]
        [SerializeField] private string iddleParameterName = "IsIdling";
        [SerializeField] private string dashParameterName = "IsDashing";
        [SerializeField] private string walkParameterName = "IsWalking";
        [SerializeField] private string runParameterName = "IsRunning";
        [SerializeField] private string sprintParameterName = "IsSprinting";
        [SerializeField] private string mStopParameterName = "IsMediumStop";
        [SerializeField] private string hStopParameterName = "IsHard";
        [SerializeField] private string RollParameterName = "IsRolling";
        [SerializeField] private string hLandParameterName = "IsHardFalling";

        [Header("State Airborne parameter Names")]
        [SerializeField] private string fallParameterName = "IsFalling";

        [Header("Idlling State: IdleAnimation parameter Names")]
        [SerializeField] private string idlleAnimationParameterName = "IdlleAnimation1";
        [SerializeField] private string TidlleAnimationParameterName = "IdlleAnimation2";

        [Header("Idlling State: IdleAnimation switch parameter")]
        [field: SerializeField][field: Range(0f, 60f)] public float timeBeforeIdleAnimation = 10f;

        [Header("Combat state: combat parameter Names")]
        [SerializeField] private string combatAnimationParameterName = "IsIncombat";
        [SerializeField] private string ackNumberAnimationParameterName = "attackNumber";

        public int GroundedParameterHash { get; private set; }
        public int movingParameterHash { get; private set; }
        public int stoppingParameterHash { get; private set; }
        public int landingParameterHash { get; private set; }
        public int airborneParameterHash { get; private set; }
        public int iddleParameterHash { get; private set; }
        public int dashParameterHash { get; private set; }
        public int walkParameterHash { get; private set; }
        public int runParameterHash { get; private set; }
        public int sprintParameterHash { get; private set; }
        public int mStopParameterHash { get; private set; }
        public int hStopParameterHash { get; private set; }
        public int RollParameterHash { get; private set; }
        public int hLandParameterHash { get; private set; }
        public int FallParameterHash { get; private set; }
        public int idlleAnimationParameterHash { get; private set; }
        public int TidlleAnimationParameterHash { get; private set; }
        public bool PlayOtherAnimation { get; set; } = false;
        public int combatAnimationParameterHash { get; private set; }
        public int ackNumberAnimationParameterHash { get; private set; }

        public void Initialize()
        {
            GroundedParameterHash = Animator.StringToHash(groundedParameterName);
            movingParameterHash = Animator.StringToHash(movingParameterName);
            stoppingParameterHash = Animator.StringToHash(stoppingParameterName);
            landingParameterHash = Animator.StringToHash(landingParameterName);
            airborneParameterHash = Animator.StringToHash(airborneParameterName);
            iddleParameterHash = Animator.StringToHash(iddleParameterName);
            dashParameterHash = Animator.StringToHash(dashParameterName);
            walkParameterHash = Animator.StringToHash(walkParameterName);
            runParameterHash = Animator.StringToHash(runParameterName);
            sprintParameterHash = Animator.StringToHash(sprintParameterName);
            mStopParameterHash = Animator.StringToHash(mStopParameterName);
            hStopParameterHash = Animator.StringToHash(hStopParameterName);
            RollParameterHash = Animator.StringToHash(RollParameterName);
            hLandParameterHash = Animator.StringToHash(hLandParameterName);
            FallParameterHash = Animator.StringToHash(fallParameterName);
            idlleAnimationParameterHash = Animator.StringToHash(idlleAnimationParameterName);
            TidlleAnimationParameterHash = Animator.StringToHash(TidlleAnimationParameterName);
            combatAnimationParameterHash = Animator.StringToHash(combatAnimationParameterName);
            ackNumberAnimationParameterHash = Animator.StringToHash(ackNumberAnimationParameterName);
        }

    }
}
