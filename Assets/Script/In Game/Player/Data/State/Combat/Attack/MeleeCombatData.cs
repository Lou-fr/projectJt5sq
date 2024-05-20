using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class MeleeCombatData
    {
        [field: SerializeField] public PlayerBasicAttackDataState BasicAttackData { get; private set; }
        [field: SerializeField] public PlayerBasicChargedAtack BasicChargedAtack { get; private set; }
        [field: Tooltip("Only for heavy weapons")]
        [field: SerializeField] public PlayerBasicHeavyAttackData BasicHeavyChargedAtack { get; private set; }
    }
}
