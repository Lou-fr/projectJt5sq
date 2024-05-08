using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerWalkData
    {
        [field: SerializeField] public float speedModifier { get; private set; } = 0.225f;
    }
}
