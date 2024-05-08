using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerFallData
    {
        [field: SerializeField][field: Range(1f, 15f)] public float FallSpeedLimit { get; private set; } = 15f;
        [field: SerializeField][field: Range(1f, 30f)] public float MinimuToBeConsideredHardFall { get; private set; } = 15f;
    }
}
