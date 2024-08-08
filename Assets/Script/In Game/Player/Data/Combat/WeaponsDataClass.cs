using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class WeaponsDataClass
    {
        [field: Header("Weapons class")]
        [field: SerializeField] public bool IsMelee { get; private set; }
        [field: SerializeField] public bool IsFirearms { get; private set; }
        [field: SerializeField] public bool IsProjectile { get; private set; }
        [field: SerializeField] public bool IsMagic { get; private set; }
        [field: Header("Sub melee class")]
        [field: SerializeField] public bool IsHeavy { get; private set; }
        [field: Header("Sub Firearms weapons class")]
        [field: SerializeField] public bool IsPistol { get; private set; }
        [field: SerializeField] public bool IsRifles { get; private set; }

    }
}
