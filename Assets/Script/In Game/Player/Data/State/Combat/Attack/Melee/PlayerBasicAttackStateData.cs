using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerBasicAttackDataState
    {
        [field: Header("Base basic attack stats")]
        [field: Tooltip("Time before the attack is reset to the first one")]
        [field: SerializeField][field: Range(0f, 5f)] public float TimeBeforeResetConsecutive { get; private set; } = 1f;
        [field: SerializeField][field: Range(0f, 5f)] public float TimeBetwinAttack { get; private set; } = 0.5f;
        [field: Header("Consecutive attack")]
        [field: SerializeField][field: Range(2, 5)] public int maxConsecutiveAttack { get; private set; } = 2;
        [field: Header("First attack base stat")]
        [field: SerializeField][field: Range(20, 160)] public int FirstAttackDmg { get; private set; }
        [field: Header("Second attack base multiplier")]
        [field: SerializeField][field: Range(1f, 5f)] public float SecondAttackM { get; private set; }
        [field: Header("Third attack base multiplier")]
        [field: Tooltip("Don't forget to set the max consecutive attack")]
        [field: SerializeField][field: Range(1.1f, 5f)] public float ThirdAttackM { get; private set; }
        [field: Header("Fourth attack base multiplier")]
        [field: Tooltip("Don't forget to set the max consecutive attack")]
        [field: SerializeField][field: Range(1.2f, 5f)] public float FourthAttackM { get; private set; }
        [field: Header("Fifth attack base multiplier")]
        [field: Tooltip("Don't forget to set the max consecutive attack")]
        [field: SerializeField][field: Range(1.3f, 5f)] public float FifthAttackM { get; private set; }
    }
}
