using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerJumpData
    {
        [field: SerializeField]public PlayerRotationData RotationData {  get; private set; }
        [field: SerializeField][field: Range(0f, 5f)] public float JumpToGroundTayDistance { get; private set; } = 2f;
        [field: SerializeField][field: Range(0f, 10f)] public float DecelerationForce { get; private set; } = 1.5f;
        [field: SerializeField] public AnimationCurve JumpForceModifierOnSlopeUpWards { get; private set; }
        [field: SerializeField] public AnimationCurve JumpForceModifierOnSlopeDownWards { get; private set; }
        [field: SerializeField] public Vector3 StationnaryForce { get; private set; }
        [field: SerializeField] public Vector3 WeakForce { get; private set; }
        [field: SerializeField] public Vector3 MediumForce { get; private set; }
        [field: SerializeField] public Vector3 StrongForce { get; private set; }
    }
}
