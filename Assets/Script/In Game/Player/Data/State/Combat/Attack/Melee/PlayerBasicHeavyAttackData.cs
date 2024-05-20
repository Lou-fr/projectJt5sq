using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerBasicHeavyAttackData 
    {
        [field: Header("Base charge attack stats")]
        [field: SerializeField][field: Range(0f, 5f)] public float TimeBitwinHeavyChargeAttack { get; private set; } = 1f;
        [field: SerializeField, Range(0, 200)] public int BaseHeavyChargeAck { get; set; } = 20;
    }
}
