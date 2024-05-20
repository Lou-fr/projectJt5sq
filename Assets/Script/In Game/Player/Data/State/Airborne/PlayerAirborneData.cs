using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerAirborneData
    {
        [field: SerializeField] public PlayerJumpData jumpData { get; private set; }
        [field: SerializeField] public PlayerFallData fallData { get; private set; }
    }
}
