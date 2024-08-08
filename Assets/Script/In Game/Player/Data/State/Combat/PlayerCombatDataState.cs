using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerCombatDataState 
    {
        [field: SerializeField] public WeaponsDataClass WeaponsDataClass { get; private set; }
        [field: SerializeField] public MeleeCombatData MeleeCombatData { get; private set;}
    }
}
