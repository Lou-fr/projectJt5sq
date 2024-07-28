using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class DefaultColliderData
    {
        [field: SerializeField] public float Height { get; set; } = 1.75f;
        [field: SerializeField] public float CenterY { get; private set; } = 0.87f;
        [field: SerializeField] public float Radius { get; private set; } = 0.2f;
    }
}
