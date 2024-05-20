using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerRunData
    {
        [field: SerializeField] public float speedModifier { get; private set; } = 1f;
    }
}
